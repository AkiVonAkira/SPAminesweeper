using DataAccess;
using Microsoft.AspNetCore.Mvc;
using SPAmineseweeper.Data;
using SPAmineseweeper.Models;

namespace SPAmineseweeper.Controllers
{
    public class ScoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScoreController(ApplicationDbContext context)
        {
            _context = context;
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
            var player = _context.PlayerModel.Find(playerId);
            var score = _context.ScoreModel;

            if (player == null)
            {
                return NotFound("Player not found");
            }

            var newScore = new Score
            {
                HighScore = highScore,
                Player = player,
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
