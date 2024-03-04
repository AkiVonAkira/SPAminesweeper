using SPAmineseweeper.Models;

namespace SPAmineseweeper.Helper
{
    public class ScoreHelper
    {
        public static double CalculateScore(Game game)
        {
            double baseScore = 1000; // Base score value
            double timeFactor = 1; // Score reduction factor per second
            double revealedTileFactor = 100; // Score addition per revealed tile

            DateTime endTime = game.GameEnded ?? DateTime.Now; // Use current time if GameEnded is null
            DateTime startTime = game.GameStarted ?? DateTime.Now; // Use current time if GameStarted is null
             
            // Calculate the time spent in seconds
            double timeElapsedSeconds = (endTime - startTime).TotalSeconds;
            // Ensure timeElapsedSeconds is non-negative
            timeElapsedSeconds = Math.Max(timeElapsedSeconds, 0);

            // Calculate the score based on time
            double timeScoreReduction = timeFactor * timeElapsedSeconds;

            // Calculate the score based on the number of revealed tiles that are't mines
            int revealedTileCount = game.Tiles.Count(tile => tile.IsRevealed && !tile.IsMine);
            double revealedTileScore = revealedTileCount * revealedTileFactor;

            // Calculate the final score
            double finalScore = baseScore + revealedTileScore - timeScoreReduction;
            finalScore = Math.Ceiling(finalScore * 10) / 10;

            // Ensure the final score is non-negative
            return Math.Max(finalScore, 0);
        }
    }
}
