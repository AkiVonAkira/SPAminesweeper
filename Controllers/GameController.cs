using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SPAmineseweeper.Data;
using SPAmineseweeper.Models;

namespace SPAmineseweeper.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        // GET: /<controller>/
        public GameController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
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
        public IActionResult StartGame(int playerId, int boardSize, int bombPercentage)
        {
            var player = _context.PlayerModel.Find(playerId);
            if (player == null)
            {
                return NotFound("Player not found");
            }

            var board = new Board
            {
                Height = boardSize,
                Width = boardSize,
                BombPercentage = bombPercentage,
                //Tiles = Tiles(boardSize, bombPercentage)
            };

            var game = new Game
            {
                PlayerId = playerId,
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

