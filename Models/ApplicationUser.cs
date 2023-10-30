using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SPAmineseweeper.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MinLength(1), MaxLength(40)]
        public string? Username { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public List<Game>? Games { get; set; }
        public List<Score>? Scores { get; set; }
    }
}