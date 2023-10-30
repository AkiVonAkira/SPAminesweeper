using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SPAmineseweeper.Data;
using SPAmineseweeper.Models;
using System.Security.Claims;

namespace SPAmineseweeper.Controllers
{
    public class ScoreController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScoreController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/gettopscores (true or false)
        [HttpGet]
        public IActionResult GetTopScores(bool isDaily)
        {
            DateTime targetDate = isDaily ? DateTime.Now.Date : DateTime.MinValue;

            var topScores = _context.ScoreModel
                .Where(score => isDaily ? score.Date.Date == targetDate : true)
                .OrderByDescending(g => g.HighScore)
                .Take(5)
                .Select(g => new
                {
                    g.Id,
                    g.HighScore
                })
                .ToList();

            return Ok(topScores);
        }

        // POST: api/score
        [HttpPost]
        public IActionResult AddScore(int playerId, int highScore)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            var score = _context.ScoreModel;

            if (user == null)
            {
                return NotFound("Player not found");
            }

            var newScore = new Score
            {
                HighScore = highScore,
                UserId = userId,
                User = user,
                Date = DateTime.Now
            };

            _context.ScoreModel.Add(newScore);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetScore), new { id = newScore.Id }, newScore);
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
