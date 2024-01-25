namespace SPAmineseweeper.Models.ViewModels
{
    public class UserView
    {
        public string? UserName { get; set; }
        public string? NickName { get; set; }
        public string? Description { get; set; }
        public double HighScore { get; set; }   
        public int Score { get; set; }
        public int GamesPlayed { get; set; }
    }
}
