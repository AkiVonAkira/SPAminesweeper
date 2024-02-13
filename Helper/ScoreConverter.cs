using SPAmineseweeper.Models;
using SPAmineseweeper.Models.ViewModels;

namespace SPAmineseweeper.Helper
{
    public class ScoreConverter
    {
        public static ScoreView ConvertScore(Score score)
        {
            var scoreView = new ScoreView();

            scoreView.Id = score.Id;
            scoreView.HighScore = score.HighScore;
            scoreView.UserId = score.UserId;
            scoreView.Date = score.Date;

            return scoreView;
        }
    }
}
