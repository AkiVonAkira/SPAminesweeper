namespace SPAmineseweeper.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public DateTime? GameStarted { get; set; }
        public DateTime? GameEnded { get; set; }
        public double Score { get; set; }
    }
}
