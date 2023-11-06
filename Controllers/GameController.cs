using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPAmineseweeper.Data;
using SPAmineseweeper.Helper;
using SPAmineseweeper.Models;
using SPAmineseweeper.Models.ViewModels;
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
        public IActionResult StartGame([FromBody] CreateGameView _game)
        {
            var game = new Game
            {
                GameStarted = DateTime.Now,
                Score = _game.Score,
                BoardSize = _game.BoardSize,
                BombPercentage = _game.BombPercentage,
                Difficulty = _game.Difficulty,
                Tiles = new List<Tile>(),
            };
            var gameStarted = game.GameStarted;
            var score = game.Score;
            var boardSize = game.BoardSize;
            var bombPercentage = game.BombPercentage;

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            var minePositions = GetMinePositions(boardSize, bombPercentage);

            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    var tile = new Tile
                    {
                        X = x,
                        Y = y,
                        IsMine = minePositions.Any(position => PositionMatch(position, x, y)),
                        IsRevealed = false,
                        IsFlagged = false,
                        AdjacentMines = 0,
                        Game = game,
                    };

                    game.Tiles.Add(tile);
                    _context.TileModel.Add(tile);
                    var _tileView = TileConverter.ConvertTiles(tile);
                }
            }

            var gameView = GameConverter.ConvertGame(game);
            _context.GameModel.Add(game);
            _context.SaveChanges();

            gameView.Id = game.Id;
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, gameView);
        }

        [HttpPut("{id}/end")]
        public IActionResult EndGame(int id)
        {
            var game = _context.GameModel.Include(g => g.Tiles).FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            game.GameEnded = DateTime.Now;
            // Fixa Logik för att räkna ut poäng eller annathär
            // game.Score = CalculateScore(game);

            _context.SaveChanges();

            return Ok(game);
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

        //[HttpPost("clicktile")]
        //public IActionResult ClickTile([FromBody] TileClickRequest request)
        //{
        //    // Handle the click event, update the game state, and return the updated game board
        //    // You need to implement the game logic here.
        //    return null;
        //}


        /*
        private List<Tile> GenerateTiles(int boardSize, int bombPercentage)
        {
            var minePositions = GetMinePositions(boardSize, bombPercentage);
            var tiles = new List<Tile>();

            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    var tile = new Tile
                    {
                        X = x,
                        Y = y,
                        IsMine = minePositions.Any(position => PositionMatch(position, x, y)),
                        IsRevealed = false,
                        IsFlagged = false,
                        AdjacentMines = 0,
                        // BoardId = board.Id, // Om det är nödvändigt att hålla reda på BoardId här, annars sätt det i Board-objektet när det skapas
                    };

                    tiles.Add(tile);
                }
            }

            return tiles;
        }
        */


    }
}

