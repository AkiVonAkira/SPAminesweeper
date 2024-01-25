namespace SPAmineseweeper.Models.ViewModels
{
    public class ScoreView
    {
        public string? NickName { get; set; }
        public int Score { get; set; }
        public double HighScore { get; set; }
        public int Id { get; internal set; }
    }
}
