using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ScreamBackend.DB
{
    public class ScreamDB : IdentityDbContext<Tables.User, IdentityRole<int>, int>
    {
        public ScreamDB(DbContextOptions options) : base(options)
        {
        }
        /// <summary>
        /// Screams table
        /// </summary>
        public DbSet<Tables.Scream> Screams { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
