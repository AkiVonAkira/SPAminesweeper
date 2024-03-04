using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPAmineseweeper.Data;
using SPAmineseweeper.Helper;
using SPAmineseweeper.Models;
using SPAmineseweeper.Models.ViewModels;
using SPAmineseweeper.Models.ViewModels.Requests;

namespace SPAmineseweeper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScoreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ScoreController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/score/gettopfivescores
        [HttpGet("gettopfivescores")]
        public List<ScoreView> GetTopFiveScores()
        {

            var topFiveScores = _context.ScoreModel
                .Where(score => true)
                .OrderByDescending(g => g.HighScore)
                .Take(5)
                .Select(score => new ScoreView
                {
                    Username = score.Game.User.UserName,
                    HighScore = score.HighScore
                })
                .ToList();

            return topFiveScores;
        }


        // POST: api/score/addscore
        [HttpPost("addscore")]
        public IActionResult AddScore([FromBody] ScoreRequest request)
        {
            var game = _context.GameModel
                .Include(g => g.Tiles)
                .Include(g => g.Score)
                .FirstOrDefault(g => g.Id == request.GameId);

            if (game == null)
            {
                return NotFound("Game not found");
            }

            double scoreValue = ScoreHelper.CalculateScore(game);

            if (game.Score == null)
            {
                var score = new Score
                {
                    Game = game,
                    HighScore = scoreValue,
                    Date = DateTime.Now
                };
                game.Score = score;
                _context.ScoreModel.Add(score);
            }
            else
            {
                game.Score.Game = game;
                game.Score.HighScore = scoreValue;
                game.Score.Date = DateTime.Now;
                _context.ScoreModel.Update(game.Score);
            }

            _context.SaveChanges();

            var updatedGame = _context.GameModel
                .Include(g => g.Tiles)
                .Include(g => g.Score)
                .FirstOrDefault(g => g.Id == request.GameId);

            return Ok(GameConverter.ConvertGame(updatedGame));
        }

        [HttpGet("{id}")]
        public IActionResult GetScore(int id)
        {
            var score = _context.ScoreModel.Find(id);
            if (score == null)
            {
                return NotFound();
            }

            return Ok(score);
        }
    }
}
