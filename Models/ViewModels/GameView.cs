namespace SPAmineseweeper.Models.ViewModels
{
    public class GameView
    {
        public int Id { get; set; }
        public DateTime? GameStarted { get; set; }
        public DateTime? GameEnded { get; set; }
        public bool GameWon { get; set; }
        public double Score { get; set; }
        public int BoardSize { get; set; }
        public int BombPercentage { get; set; }
        public string? Difficulty { get; set; }
        public List<TileView>? Tiles { get; set; }
    }
}
