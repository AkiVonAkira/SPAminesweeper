using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPAmineseweeper.Helper;
using SPAmineseweeper.Models.ViewModels.Requests;
using SPAmineseweeper.Models;
using Microsoft.AspNetCore.Identity;
using SPAmineseweeper.Data;
using Microsoft.AspNetCore.Authorization;

namespace SPAmineseweeper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("revealtile")]
        public IActionResult RevealTile([FromBody] TileClickRequest request)
        {
            var game = _context.GameModel
                .Include(g => g.Tiles)
                .FirstOrDefault(g => g.Id == request.GameId);

            if (game == null)
            {
                return NotFound("Game not found");
            }

            // Find the clicked tile
            var clickedTile = game.Tiles.FirstOrDefault(tile => tile.X == request.X && tile.Y == request.Y);

            if (clickedTile == null)
            {
                return NotFound("Tile not found");
            }

            // If the clicked tile is a mine, end the game
            if (clickedTile.IsMine)
            {
                clickedTile.IsRevealed = true;
                game.GameEnded = DateTime.Now;
                _context.SaveChanges();

                return Ok(GameConverter.ConvertGame(game));
            }

            // Recursively reveal tiles
            var revealedTiles = new List<Tile>();
            TileHelper.RevealTileRecursive(game, clickedTile, revealedTiles);

            // Check for game over conditions
            bool isGameOver = GameHelper.CheckGameOver(game, revealedTiles);

            // Update the game state
            _context.SaveChanges();

            // Return the updated game data
            var updatedGame = _context.GameModel
                .Include(g => g.Tiles)
                .FirstOrDefault(g => g.Id == request.GameId);

            if (isGameOver)
            {
                return Ok(GameConverter.ConvertGame(game));
            }
            else
            {
                return Ok(GameConverter.ConvertGame(updatedGame));
            }
        }

    }
}
