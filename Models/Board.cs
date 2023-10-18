namespace SPAmineseweeper.Models
{
    public class Board
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string? Difficulty { get; set; }
        public int BombPercentage { get; set; }
    }
}
