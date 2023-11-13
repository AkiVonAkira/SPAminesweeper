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

            // Check if an existing game is in progress
            var existingGame = GameHelper.CheckExistingGame(_context, userId);
            if (existingGame != null)
            {
                _context.SaveChanges();
                return Ok(GameConverter.ConvertGame(existingGame));
            }

            // Create a new game
            var game = GameHelper.CreateNewGame(_context, _game, userId);
            GameHelper.CreateMines(game);

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

            bool isGameOver = GameHelper.CheckGameOver(game);

            game.Score = ScoreHelper.CalculateScore(game);

            _context.SaveChanges();

            return Ok(GameConverter.ConvertGame(game));
        }
    }
}
