using Microsoft.EntityFrameworkCore;

namespace TeamListAPI.Models
{
    public class TeamListContext : DbContext
    {
        public TeamListContext(DbContextOptions<TeamListContext> options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
