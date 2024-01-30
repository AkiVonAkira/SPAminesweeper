using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SPAmineseweeper.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MinLength(1), MaxLength(40)]
        public string? Nickname { get; set; }
        public string? Description { get; set; }
        public List<Game>? Games { get; set; }
        public List<Score>? Scores { get; set; }
    }
}