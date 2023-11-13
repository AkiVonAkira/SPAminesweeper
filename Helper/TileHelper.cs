using SPAmineseweeper.Models;

namespace SPAmineseweeper.Helper
{
    public class TileHelper
    {
        public static void RevealTileRecursive(Game game, Tile tile, List<Tile> revealedTiles)
        {
            if (revealedTiles.Contains(tile) || tile.IsFlagged)
            {
                return;
            }

            revealedTiles.Add(tile);
            tile.IsRevealed = true;

            // If the tile has no adjacent mines, reveal neighboring tiles
            if (tile.AdjacentMines == 0)
            {
                var neighbors = GetNeighbors(game, tile);
                foreach (var neighbor in neighbors)
                {
                    RevealTileRecursive(game, neighbor, revealedTiles);
                }
            }
        }

        public static List<Tile> GetNeighbors(Game game, Tile tile)
        {
            var neighbors = new List<Tile>();
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    int neighborX = tile.X + dx;
                    int neighborY = tile.Y + dy;

                    var neighbor = game.Tiles.FirstOrDefault(t => t.X == neighborX && t.Y == neighborY);
                    if (neighbor != null)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }
            return neighbors;
        }

        public static void ToggleFlag(Game game, Tile tile, List<Tile> flaggedTiles)
        {
            if (flaggedTiles.Contains(tile))
            {
                return;
            }

            flaggedTiles.Add(tile);
            tile.IsFlagged = !tile.IsFlagged;
        }
    }
}
