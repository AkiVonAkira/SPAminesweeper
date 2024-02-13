using SPAmineseweeper.Models;

namespace SPAmineseweeper.Helper
{
    public class ScoreHelper
    {
        public static void CalculateScore(Game game)
        {
            double baseScore = 10000; // Base score value
            double timeFactor = 0.5; // Score reduction factor per second
            double revealedTileFactor = 100; // Score addition per revealed tile
            double bombPenalty = 5000; // Score penalty per bomb

            DateTime endTime = game.GameEnded ?? DateTime.UtcNow; // Use current time if GameEnded is null
            DateTime startTime = game.GameStarted ?? DateTime.UtcNow; // Use current time if GameStarted is null

            // Calculate the time spent in seconds
            double timeElapsedSeconds = (endTime - startTime).TotalSeconds;
            // Ensure timeElapsedSeconds is non-negative
            timeElapsedSeconds = Math.Max(timeElapsedSeconds, 0);

            // Calculate the score based on time
            double timeScoreReduction = timeFactor * timeElapsedSeconds;

            // Calculate the score based on the number of revealed tiles
            int revealedTileCount = game.Tiles.Count(tile => tile.IsRevealed);
            double revealedTileScore = revealedTileCount * revealedTileFactor;

            // Calculate the penalty based on the number of mines
            int mineCount = game.Tiles.Count(tile => tile.IsMine);
            double bombPenaltyScore = mineCount * bombPenalty;

            // Calculate the final score
            double finalScore = baseScore + revealedTileScore - timeScoreReduction - bombPenaltyScore;

            // Ensure the final score is non-negative
            double finalScoreNonNegative = Math.Max(finalScore, 0);

            // Update the existing Score object attached to the Game object
            if (game.Score == null)
            {
                game.Score = new Score();
            }

            game.Score.HighScore = finalScoreNonNegative;
            game.Score.UserId = game.UserId;
            game.Score.User = game.User;
            game.Score.Date = DateTime.Now;
        }
    }
}
