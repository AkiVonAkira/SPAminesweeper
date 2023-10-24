namespace SPAmineseweeper.Models.ViewModels
{
    public class CreateBoardView
    {
        public int BoardSize { get; set; }
        public string? Difficulty { get; set; }
        public int BombPercentage { get; set; }
    }
}
