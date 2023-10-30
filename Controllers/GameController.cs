using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPAmineseweeper.Data;
using SPAmineseweeper.Models;
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
                             .Include(g => g.Board)
                                .ThenInclude(b => b.Tiles)
                             .FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        [HttpPost("start")]
        public IActionResult StartGame(int boardSize, int bombPercentage, int boardId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            var board = _context.BoardModel.Find(boardId);

            var game = new Game
            {
                UserId = userId,
                Board = board,
                GameStarted = DateTime.Now
            };

            _context.GameModel.Add(game);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

        [HttpPut("{id}/end")]
        public IActionResult EndGame(int id)
        {
            var game = _context.GameModel.Include(g => g.Board.Tiles).FirstOrDefault(g => g.Id == id);
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

