using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proxy.Models;
using SPAmineseweeper.Data;
using SPAmineseweeper.Models;


// Jobba på denna i senare skede, Skapa controller som visar top 5 
namespace SPAmineseweeper.Controllers
{
    public class ScoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTopScores()
        {
            var topScores = _context.PlayerModel
                .OrderByDescending(g => g.HighScore)
                .Take(5)
                .Select(g => new
                {
                    PlayerName = g.Id.Name, // Använd detta för att få spelarens namn
                    g.HighScore
                })
                .List();

            return Ok(topScores);
        }
        // POST: api/score
        [HttpPost]
        public IActionResult AddScore(int playerId, int highScore)
        {
            var player = _context.PlayerModel.Find(playerId);
            if (player == null)
            {
                return NotFound("Player not found");
            }

            var newScore = new Player
            {
                Id = playerId,
                HighScore = highScore,
                Date = DateTime.Now
            };

            _context.PlayerModel.Add(newScore);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetScore), new { id = newScore.Id }, newScore);
        }


        [HttpGet("{id}")]
        public IActionResult GetScore(int id)
        {
            var score = _context.PlayerModel.Find(id);
            if (score == null)
            {
                return NotFound();
            }

            return Ok(score);
        }
    }


}


