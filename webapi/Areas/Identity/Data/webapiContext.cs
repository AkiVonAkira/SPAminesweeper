using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SPAminesweeper.Model;
using webapi.Areas.Identity.Data;

namespace webapi.Data;

public class webapiContext : IdentityDbContext<webapiUser>
{
    public webapiContext(DbContextOptions<webapiContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<BoardModel> Boards { get; set; }
    public DbSet<GameModel> Games { get; set; }
    public DbSet<PlayerModel> Player { get; set; }
}
