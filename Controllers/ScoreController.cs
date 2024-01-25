using Azure.Core;
using DataAccess;
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

        // GET: api/gettopfivescores (true or false)
        [HttpGet("gettopfiveoverall")]
        public List<ScoreView> GetTopFiveScores(bool isDaily)
        {
            DateTime targetDate = isDaily ? DateTime.Now.Date : DateTime.MinValue;

            var topFiveScores = _context.ScoreModel
                .Where(score => isDaily ? score.Date.Date == targetDate : true)
                .OrderByDescending(g => g.HighScore)
                .Take(5)
                .Select(score => new ScoreView
                {
                    Id = score.Id, 
                    HighScore = score.HighScore
                })
                .ToList();

            return topFiveScores;
        }


        // POST: api/score
        [HttpPost]
        public IActionResult AddScore(int playerId, int gameId)
        {
            var game = _context.GameModel
                .FirstOrDefault(g => g.Id == gameId);

            if (game == null)
            {
                return NotFound("Game not found");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            var score = _context.ScoreModel;

            if (user == null)
            {
                return NotFound("Player not found");
            }
            var highScore = ScoreHelper.CalculateScore(game);

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

/*
Gamla TopFiveScores Controller action


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
*/