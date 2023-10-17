namespace SPAminesweeper.Model
{
    public class GameModel
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public DateTime? Date { get; set; }
        public double Score { get; set; }   
        public TimeSpan? Time {  get; set; }


    }
}
