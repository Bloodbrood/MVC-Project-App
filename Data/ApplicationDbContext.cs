using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectApp.Models;

namespace ProjectApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProjectApp.Models.Person> Person { get; set; } = default!;
        public DbSet<ProjectApp.Models.Insurance> Insurance { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>()
                .HasOne(i => i.IdentityUser)
                .WithMany()
                .HasForeignKey(i => i.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade); // Kaskádové mazání

            modelBuilder.Entity<Insurance>()
                .HasOne(i => i.Person)
                .WithMany(p => p.Insurances)
                .HasForeignKey(i => i.PersonId)
                .OnDelete(DeleteBehavior.Cascade); // Kaskádové mazání

            modelBuilder.Entity<Insurance>()
                .Property(i => i.Castka)
                .HasColumnType(null);

            modelBuilder.Entity<Insurance>()
                .Property(i => i.Castka)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" });
        }



    }
}
