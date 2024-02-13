using SPAmineseweeper.Models;
using SPAmineseweeper.Models.ViewModels.Requests;

namespace SPAmineseweeper.Helper
{
    public class GameHelper
    {
        public static void CreateMines(Game game)
        {
            var minePositions = RandomizeMinePositions(game.BoardSize, game.BombPercentage);

            foreach (var tile in game.Tiles)
            {
                tile.IsMine = minePositions.Any(position => PositionMatch(position, tile.X, tile.Y));
            }
        }

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

        public static bool PositionMatch((int, int) position, int x, int y)
        {
            return position.Item1 == x && position.Item2 == y;
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

        public static bool CheckGameOver(Game game)
        {
            int totalTiles = game.Tiles.Count;
            int totalMines = game.Tiles.Count(tile => tile.IsMine);

            int revealedNonMineTiles = game.Tiles.Count(tile => !tile.IsMine && tile.IsRevealed);
            int revealedMineTiles = game.Tiles.Count(tile => tile.IsMine && tile.IsRevealed);

            if (revealedNonMineTiles == totalTiles - totalMines)
            {
                game.GameEnded = DateTime.Now;
                game.GameWon = true;
                return true;
            }

            if (revealedMineTiles > 0)
            {
                game.GameEnded = DateTime.Now;
                game.GameWon = false;
                return true;
            }

            game.Score = ScoreHelper.CalculateScore(game);

            return false;
        }

        internal static void SetDifficultyParameters(Game game, CreateGameRequest request)
        {
            int gridSize = 10;
            int bombPercentage = 5;

            switch (game.Difficulty.ToLower())
            {
                case "easy":
                    gridSize = 8;
                    bombPercentage = 10;
                    break;
                case "medium":
                    gridSize = 10;
                    bombPercentage = 15;
                    break;
                case "hard":
                    gridSize = 12;
                    bombPercentage = 20;
                    break;
                case "extreme":
                    gridSize = 14;
                    bombPercentage = 25;
                    break;
                case "custom":
                    gridSize = request.BoardSize;
                    bombPercentage = request.BombPercentage;
                    break;
                default:
                    break;
            }

            game.BoardSize = gridSize;
            game.BombPercentage = bombPercentage;
        }
    }
}
