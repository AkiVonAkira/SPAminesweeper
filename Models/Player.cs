using System.ComponentModel.DataAnnotations;

namespace SPAmineseweeper.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int HighScore { get; set; }
        public DateTime Date { get; set; }  
        public List<Game>? Games { get; set; }
    }
}
