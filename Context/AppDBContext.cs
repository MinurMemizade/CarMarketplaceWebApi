using CarMarketplaceWebApi.Models;
using CarMarketplaceWebApi.Models.Identity;
using CarMarketplaceWebApi.Models.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CarMarketplaceWebApi.Context
{
    public class AppDBContext : IdentityDbContext<AppUser,Role,Guid>
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Car>()
                .Property(c => c.eStatus)
                .HasConversion<int>();

            builder.Entity<Car>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.UserCars)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Favorites>()
                .HasKey(f => f.Id);

            builder.Entity<Favorites>()
                .HasOne(f=>f.AppUser)
                .WithMany(u=>u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Favorites>()
                .HasOne(f=>f.Car)
                .WithMany()
                .HasForeignKey(f=>f.CarId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Car> cars { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
    }
}
