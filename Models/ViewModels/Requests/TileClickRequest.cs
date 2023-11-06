namespace SPAmineseweeper.Models.ViewModels.Requests
{
    public class TileClickRequest
    {
        public int GameId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
