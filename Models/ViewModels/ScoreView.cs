namespace SPAmineseweeper.Models.ViewModels
{
    public class ScoreView
    {
        public int Id { get; internal set; }
        public double HighScore { get; set; }
        public string? UserId { get; internal set; }
        public DateTime Date { get; set; }
    }
}
