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
                _context.SaveChanges();
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

            var minePositions = GameHelper.RandomizeMinePositions(game.BoardSize, game.BombPercentage);

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
            GameHelper.CalculateAdjacentMines(game);

            _context.GameModel.Add(game);
            _context.SaveChanges();

            var gameView = GameConverter.ConvertGame(game);
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, gameView);
        }

        [HttpPut("{id}/endgame")]
        public IActionResult EndGame(int id)
        {
            var game = _context.GameModel.Include(g => g.Tiles).FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound("Game not found");
            }

            bool isGameOver = GameHelper.CheckGameOver(game, game.Tiles);

            game.Score = ScoreHelper.CalculateScore(game);

            _context.SaveChanges();

            return Ok(GameConverter.ConvertGame(game));
        }

        private bool PositionMatch((int, int) position, int x, int y)
        {
            return position.Item1 == x && position.Item2 == y;
        }
    }
}
