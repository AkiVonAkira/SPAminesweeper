using SPAmineseweeper.Models;
using SPAmineseweeper.Models.ViewModels;

namespace SPAmineseweeper.Helper
{
    public class BoardConverter
    {
        public static BoardView ConvertBoard(Board board)
        {
            var boardView = new BoardView();

            boardView.BoardSize = board.BoardSize;
            boardView.Difficulty = board.Difficulty;
            boardView.BombPercentage = board.BombPercentage;

            var _tiles = new List<TileView>();
            foreach (var t in board.Tiles)
            {
                _tiles.Add(TileConverter.ConvertTiles(t));
            }
            boardView.Tiles = _tiles;

            return boardView;
        }
    }
}
