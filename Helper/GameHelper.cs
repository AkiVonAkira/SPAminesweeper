using SPAmineseweeper.Models;

namespace SPAmineseweeper.Helper
{
    public class GameHelper
    {
        public static void CalculateAdjacentMines(Game game)
        {
            foreach (var tile in game.Tiles)
            {
                if (!tile.IsMine)
                {
                    var neighbors = TileHelper.GetNeighbors(game, tile);
                    tile.AdjacentMines = neighbors.Count(neighbor => neighbor.IsMine);
                }
            }
        }

        public static List<(int, int)> RandomizeMinePositions(int boardSize, int bombPercentage)
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

        public static bool CheckGameOver(Game game, List<Tile> revealedTiles)
        {
            // Check for game over conditions, e.g., all non-mine tiles are revealed
            int totalTiles = game.Tiles.Count;
            int totalMines = game.Tiles.Count(tile => tile.IsMine);

            int revealedNonMineTiles = revealedTiles.Count(tile => !tile.IsMine);

            game.GameEnded = DateTime.Now;

            return revealedNonMineTiles == totalTiles - totalMines;
        }
    }
}
