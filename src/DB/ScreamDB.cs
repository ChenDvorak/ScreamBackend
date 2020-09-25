using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB.Tables;

namespace ScreamBackend.DB
{
    public class ScreamDB : DbContext
    {
        public ScreamDB(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //  optionsBuilder.UseMySql("server=localhost;Database=scream;User=root;Password=MrUNOwen");
        }

        /// <summary>
        /// Screams table
        /// </summary>
        public DbSet<Scream> Screams { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Scream>()
                .HasIndex(s => s.CreateDate)
                .IsUnique(false);

            builder.Entity<Comment>()
                .HasIndex(c => c.ScreamId)
                .IsUnique(false);
        }
    }
}
