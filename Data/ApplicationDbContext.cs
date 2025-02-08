using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StockMarketApi.Model;

namespace StockMarketApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApiUser>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
            
        }

        public DbSet<Stock> Stock {  get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        //seeding the roles for associated users

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>()
                .HasKey(p => new {p.ApiUserId, p.StockId}); //Composite Primary key for Portfolio Table (the combination of StudentId and CourseId must be unique.)

            builder.Entity<Portfolio>()
                .HasOne(p => p.ApiUser)                     // One ApiUser
                .WithMany(u => u.Portfolios)                // Can be enrolled in many portfolies
                .HasForeignKey(p => p.ApiUserId);           // Foreign key is ApiUserId

            builder.Entity<Portfolio>()
                .HasOne(p => p.Stock)                       // One stock
                .WithMany(u => u.Portfolios)                // Con be in many Portfolios 
                .HasForeignKey(p => p.StockId);             // Foreign key is StockId


            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },

                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }

        // Suppress the PendingModelChangesWarning
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

    }
}
