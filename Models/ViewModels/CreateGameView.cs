namespace SPAmineseweeper.Models.ViewModels
{
    public class CreateGameView
    {
        public DateTime? GameStarted { get; set; }
        public DateTime? GameEnded { get; set; }
        public double Score { get; set; }
        public int BoardSize { get; set; }
        public string? Difficulty { get; set; }
        public int BombPercentage { get; set; }
    }
}
