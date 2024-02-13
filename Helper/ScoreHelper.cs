using SPAmineseweeper.Models;

namespace SPAmineseweeper.Helper
{
    public class ScoreHelper
    {
        public static Score CalculateScore(Game game)
        {
            Score score = game.Score;
            score.UserId = game.UserId;
            score.User = game.User;
            score.Date = DateTime.Now;

            double baseScore = 10000; // Base score value
            double timeFactor = 0.5; // Score reduction factor per second
            double revealedTileFactor = 100; // Score addition per revealed tile
            double bombPenalty = 5000; // Score penalty per bomb

            DateTime endTime = game.GameEnded ?? DateTime.Now; // Use current time if GameEnded is null
            DateTime startTime = (DateTime)game.GameStarted;

            // Calculate the time spent in seconds
            double timeElapsedSeconds = (endTime - startTime).TotalSeconds;

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
            game.Score.HighScore = Math.Max(finalScore, 0);
            return game.Score;
        }
    }
}
