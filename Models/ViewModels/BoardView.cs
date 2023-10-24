namespace SPAmineseweeper.Models.ViewModels
{
    public class BoardView
    {
        public int BoardSize { get; set; }
        public string? Difficulty { get; set; }
        public int BombPercentage { get; set; }
        public List<TileView>? Tiles { get; set; }
    }
}
