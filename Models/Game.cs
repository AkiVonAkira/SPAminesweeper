using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPAmineseweeper.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        //"[ForeignKey("Player")]"" is a data annotation that tells the database that the PlayerId property is a foreign key to the Player table.
        public int PlayerId { get; set; }
        [Required]
        public DateTime? GameStarted { get; set; }
        [Required]
        public DateTime? GameEnded { get; set; }
        [Required]
        public double Score { get; set; }

        public virtual Board? Board { get; set; } // navigerings property till Board

        public List<Player>? Players { get; set; }
    }
}
