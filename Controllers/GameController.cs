using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPAmineseweeper.Data;
using SPAmineseweeper.Helper;
using SPAmineseweeper.Models;
using SPAmineseweeper.Models.ViewModels.Requests;
using System.Security.Claims;

namespace SPAmineseweeper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public GameController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("{id}")]
        public IActionResult GetGame(int id)
        {
            var game = _context.GameModel
                             .Include(g => g.Tiles)
                             .FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        [HttpPost("startgame")]
        public IActionResult StartGame([FromBody] CreateGameRequest _game)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingGame = _context.GameModel
                .Include(g => g.Tiles)
                .FirstOrDefault(g => g.UserId == userId && g.GameEnded == null);

            if (existingGame != null)
            {
                var existingGameView = GameConverter.ConvertGame(existingGame);
                return Ok(existingGameView);
            }

            var game = new Game
            {
                GameStarted = DateTime.Now,
                Score = _game.Score,
                BoardSize = _game.BoardSize,
                BombPercentage = _game.BombPercentage,
                Difficulty = _game.Difficulty,
                Tiles = new List<Tile>(),
                UserId = userId
            };

            var minePositions = GetMinePositions(game.BoardSize, game.BombPercentage);

            for (int x = 0; x < game.BoardSize; x++)
            {
                for (int y = 0; y < game.BoardSize; y++)
                {
                    var tile = new Tile
                    {
                        X = x,
                        Y = y,
                        IsMine = minePositions.Any(position => PositionMatch(position, x, y)),
                        IsRevealed = false,
                        IsFlagged = false,
                        AdjacentMines = 0,
                        Game = game
                    };

                    game.Tiles.Add(tile);
                    _context.TileModel.Add(tile);
                }
            }
            CalculateAdjacentMines(game);

            _context.GameModel.Add(game);
            _context.SaveChanges();

            var gameView = GameConverter.ConvertGame(game);
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, gameView);
        }

        [HttpPost("revealtile")]
        public IActionResult RevealTile([FromBody] TileClickRequest request)
        {
            var game = _context.GameModel
                .Include(g => g.Tiles)
                .FirstOrDefault(g => g.Id == request.GameId);

            if (game == null)
            {
                return NotFound("Game not found");
            }

            // Find the clicked tile
            var clickedTile = game.Tiles.FirstOrDefault(tile => tile.X == request.X && tile.Y == request.Y);

            if (clickedTile == null)
            {
                return NotFound("Tile not found");
            }

            if (clickedTile.IsRevealed)
            {
                return BadRequest("Tile is already revealed");
            }

            // Recursively reveal tiles
            var revealedTiles = new List<Tile>();
            RevealTileRecursive(game, clickedTile, revealedTiles);

            // Check for game over conditions
            bool isGameOver = CheckGameOver(game, revealedTiles);

            // Update the game state
            _context.SaveChanges();

            // Return the updated game data
            var updatedGame = _context.GameModel
                .Include(g => g.Tiles)
                .FirstOrDefault(g => g.Id == request.GameId);

            if (isGameOver)
            {
                return Ok(new { game = updatedGame, message = "Game over" });
            }
            else
            {
                return Ok(updatedGame);
            }
        }

        private void RevealTileRecursive(Game game, Tile tile, List<Tile> revealedTiles)
        {
            if (revealedTiles.Contains(tile) || tile.IsFlagged)
            {
                return;
            }

            revealedTiles.Add(tile);
            tile.IsRevealed = true;

            // If the tile has no adjacent mines, reveal neighboring tiles
            if (tile.AdjacentMines == 0)
            {
                var neighbors = GetNeighbors(game, tile);
                foreach (var neighbor in neighbors)
                {
                    RevealTileRecursive(game, neighbor, revealedTiles);
                }
            }
        }

        private List<Tile> GetNeighbors(Game game, Tile tile)
        {
            var neighbors = new List<Tile>();
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    int neighborX = tile.X + dx;
                    int neighborY = tile.Y + dy;

                    var neighbor = game.Tiles.FirstOrDefault(t => t.X == neighborX && t.Y == neighborY);
                    if (neighbor != null)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }
            return neighbors;
        }

        private bool CheckGameOver(Game game, List<Tile> revealedTiles)
        {
            // Check for game over conditions, e.g., all non-mine tiles are revealed
            int totalTiles = game.Tiles.Count;
            int totalMines = game.Tiles.Count(tile => tile.IsMine);

            int revealedNonMineTiles = revealedTiles.Count(tile => !tile.IsMine);

            return revealedNonMineTiles == totalTiles - totalMines;
        }

        [HttpPut("{id}/endgame")]
        public IActionResult EndGame(int id)
        {
            var game = _context.GameModel.Include(g => g.Tiles).FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound("Game not found");
            }

            bool isGameOver = CheckGameOver(game, game.Tiles);

            game.Score = CalculateScore(game);

            _context.SaveChanges();

            if (isGameOver)
            {
                game.GameEnded = DateTime.Now;
                var gameView = GameConverter.ConvertGame(game);
                return Ok(new { game = gameView, message = "Game over" });
            }
            else
            {
                return Ok(game);
            }
        }

        private double CalculateScore(Game game)
        {
            throw new NotImplementedException();
        }

        private void CalculateAdjacentMines(Game game)
        {
            foreach (var tile in game.Tiles)
            {
                if (!tile.IsMine)
                {
                    var neighbors = GetNeighbors(game, tile);
                    tile.AdjacentMines = neighbors.Count(neighbor => neighbor.IsMine);
                }
            }
        }

        private List<(int, int)> GetMinePositions(int boardSize, int bombPercentage)
        {
            var random = new Random();
            var minePositions = new List<(int, int)>();
            for (int i = 0; i < bombPercentage; i++)
            {
                var x = random.Next(boardSize);
                var y = random.Next(boardSize);
                minePositions.Add((x, y));
            }
            return minePositions;
        }

        private bool PositionMatch((int, int) position, int x, int y)
        {
            return position.Item1 == x && position.Item2 == y;
        }
    }
}
