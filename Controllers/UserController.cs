using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SPAmineseweeper.Data;
using SPAmineseweeper.Models;
using SPAmineseweeper.Models.ViewModels;
using System.Security.Claims;

namespace SPAmineseweeper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("getuser")]
        public UserView GetUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                var games = _context.GameModel.Where(game => game.UserId == userId 
                && game.GameStarted != null 
                && game.GameEnded != null).ToList();

                double totalScore = 0;
                double highestScore = 0;

                foreach (var game in games)
                {
                    if (game.Score != null)
                    {
                        totalScore += game.Score.HighScore;
                        highestScore = Math.Max(highestScore, game.Score.HighScore);
                    }
                }

                totalScore = Math.Ceiling(totalScore * 10) / 10;

                if (user == null)
                {
                    throw new Exception("Error fetching User");
                }

                var userInfo = new UserView
                {
                    Username = user.UserName,
                    Nickname = user.Nickname,
                    HighScore = totalScore,
                    Score = highestScore,
                    GamesPlayed = games.Count(),
                };

                return userInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }


        [HttpGet("userloggedin")]
        public bool CheckLoggedIn()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
