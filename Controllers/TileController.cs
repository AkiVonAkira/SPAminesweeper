using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPAmineseweeper.Data;
using SPAmineseweeper.Helper;
using SPAmineseweeper.Models;
using SPAmineseweeper.Models.ViewModels.Requests;

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
        private int revealedMineTiles;

        private IActionResult ProcessTileAction(TileClickRequest request, Action<Tile, Game> tileAction)
        {
            var game = _context.GameModel
                .Include(g => g.Tiles)
                .Include(g => g.Score)
                .FirstOrDefault(g => g.Id == request.GameId);

            if (game == null)
            {
                return NotFound("Game not found");
            }

            var clickedTile = game.Tiles.FirstOrDefault(tile => tile.X == request.X && tile.Y == request.Y);

            if (clickedTile == null)
            {
                return NotFound("Tile not found");
            }

            revealedMineTiles = game.Tiles.Count(tile => tile.IsMine && tile.IsRevealed);

            if (revealedMineTiles > 0)
            {
                return Ok(GameConverter.ConvertGame(game));
            }

            tileAction(clickedTile, game);

            bool isGameOver = GameHelper.CheckGameOver(game);

            _context.SaveChanges();

            var updatedGame = _context.GameModel
                .Include(g => g.Tiles)
                .Include(g => g.Score)
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

        [HttpPost("revealtile")]
        public IActionResult RevealTile([FromBody] TileClickRequest request)
        {
            return ProcessTileAction(request, (clickedTile, game) =>
            {
                if (clickedTile.IsMine)
                {
                    clickedTile.IsRevealed = true;
                    revealedMineTiles++;
                    game.GameEnded = DateTime.Now;

                    foreach (var tile in game.Tiles.Where(tile => tile.IsMine))
                    {
                        tile.IsRevealed = true;
                    }
                }
                else
                {
                    var revealedTiles = new List<Tile>();
                    TileHelper.RevealTileRecursive(game, clickedTile, revealedTiles);
                }
            });
        }

        [HttpPost("flagtile")]
        public IActionResult FlagTile([FromBody] TileClickRequest request)
        {
            return ProcessTileAction(request, (clickedTile, game) =>
            {
                if (game.GameEnded == null)
                {
                    clickedTile.IsFlagged = !clickedTile.IsFlagged;
                }
            });
        }
    }
}
