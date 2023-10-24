using Microsoft.AspNetCore.Mvc;
using SPAmineseweeper.Data;
using SPAmineseweeper.Helper;
using SPAmineseweeper.Models;
using SPAmineseweeper.Models.ViewModels;

namespace SPAmineseweeper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BoardController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("createboard")]
        public IActionResult CreateBoard([FromBody] CreateBoardView _board)
        {
            var board = new Board
            {
                BoardSize = _board.BoardSize,
                BombPercentage = _board.BombPercentage,
                Tiles = new List<Tile>(),
            };
            var boardSize = board.BoardSize;
            var bombPercentage = board.BombPercentage;

            var minePositions = GetMinePositions(boardSize, bombPercentage);

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
                        Board = board,
                    };

                    board.Tiles.Add(tile);
                    _context.TileModel.Add(tile);
                    var _tileView = TileConverter.ConvertTiles(tile);
                }
            }

            var boardView = BoardConverter.ConvertBoard(board);
            _context.BoardModel.Add(board);
            _context.SaveChanges();

            return Ok(boardView);
        }

        private List<(int, int)> GetMinePositions(int boardSize, int bombPercentage)
        {
            var random = new Random();
            var minePositions = new List<(int, int)>();
            for (int i = 0; i < bombPercentage; i++)
            {
                var x = random.Next(boardSize);
                var y = random.Next(boardSize);
                minePositions.Add((x, y));
            }
            return minePositions;
        }

        private bool PositionMatch((int, int) position, int x, int y)
        {
            return position.Item1 == x && position.Item2 == y;
        }
    }
}
