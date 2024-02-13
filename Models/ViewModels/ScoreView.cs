namespace SPAmineseweeper.Models.ViewModels
{
    public class ScoreView
    {
        public int Id { get; set; }
        public double HighScore { get; set; }
        public string? UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
