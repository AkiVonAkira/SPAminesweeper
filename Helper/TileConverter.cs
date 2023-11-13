using SPAmineseweeper.Models;
using SPAmineseweeper.Models.ViewModels;

namespace SPAmineseweeper.Helper
{
    public class TileConverter
    {
        public static TileView ConvertTiles(Tile tile)
        {
            var tileView = new TileView();

            tileView.Id = tile.Id;
            tileView.X = tile.X;
            tileView.Y = tile.Y;
            tileView.IsMine = tile.IsMine;
            tileView.AdjacentMines = tile.AdjacentMines;
            tileView.IsRevealed = tile.IsRevealed;
            tileView.IsMine = tile.IsMine;
            tileView.IsFlagged = tile.IsFlagged;

            return tileView;
        }
    }
}
