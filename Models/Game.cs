using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPAmineseweeper.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        [Required]
        public DateTime? GameStarted { get; set; }
        [Required]
        public DateTime? GameEnded { get; set; }
        [Required]
        public double Score { get; set; }

        public virtual Board? Board { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
