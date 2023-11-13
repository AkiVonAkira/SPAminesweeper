﻿using Microsoft.AspNetCore.Authorization;
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
        public IActionResult StartGame([FromBody] CreateGameRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if an existing game is in progress
            var existingGame = CheckExistingGame(userId);
            if (existingGame != null)
            {
                _context.SaveChanges();
                return Ok(GameConverter.ConvertGame(existingGame));
            }

            // Create a new game
            var game = CreateNewGame(request, userId);

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

        private Game CheckExistingGame(string userId)
        {
            return _context.GameModel
                .Include(g => g.Tiles)
                .FirstOrDefault(g => g.UserId == userId && g.GameEnded == null);
        }

        private Game CreateNewGame(CreateGameRequest request, string userId)
        {
            var game = new Game
            {
                GameStarted = DateTime.Now,
                Score = request.Score,
                BoardSize = request.BoardSize,
                BombPercentage = request.BombPercentage,
                Difficulty = request.Difficulty,
                Tiles = new List<Tile>(),
                UserId = userId
            };

            for (int x = 0; x < game.BoardSize; x++)
            {
                for (int y = 0; y < game.BoardSize; y++)
                {
                    var tile = new Tile
                    {
                        X = x,
                        Y = y,
                        IsMine = false,
                        IsRevealed = false,
                        IsFlagged = false,
                        AdjacentMines = 0,
                        Game = game
                    };

                    game.Tiles.Add(tile);
                    _context.TileModel.Add(tile);
                }
            }

            GameHelper.CreateMines(game);
            GameHelper.CalculateAdjacentMines(game);
            _context.GameModel.Add(game);
            return game;
        }
    }
}
