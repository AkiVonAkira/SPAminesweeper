using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SPAmineseweeper.Models;

namespace SPAmineseweeper.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }

        public DbSet<Board> BoardModel { get; set; }
        public DbSet<Game> GameModel { get; set; }
        public DbSet<Player> PlayerModel { get; set; }
        public DbSet<Score> ScoreModel { get; set; }
        public DbSet<Tile> TileModel { get; set; }
    }
}