namespace SPAmineseweeper.Models.ViewModels
{
    public class UserView
    {
        public string? Username { get; set; }
        public string? Nickname { get; set; }
        public double HighScore { get; set; }
        public int Score { get; set; }
        public int GamesPlayed { get; set; }
    }
}
