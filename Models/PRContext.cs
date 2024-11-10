using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace PRWebAPI.Models
{
    public class PRContext : IdentityDbContext<IdentityUser>
    {
        public PRContext(DbContextOptions<PRContext> options) : base(options)
        {

        }

        public DbSet<ContactDetails> tblContactDetails { get; set; }
        public DbSet<InteractionDetails> tblInteractionDetails { get; set; }
        public DbSet<Users> tblUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
        new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName ="Admin" },
        new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "User" },
        new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "HR" }
    );
        }
    }
}
