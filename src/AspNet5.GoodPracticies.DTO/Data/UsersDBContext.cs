using Microsoft.EntityFrameworkCore;

namespace AspNet5.GoodPracticies.DTO.Data
{
    public class UsersDBContext : DbContext
    {
        public UsersDBContext(DbContextOptions<UsersDBContext> options) : base (options)
        {

        }

        public DbSet<UserDBModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDBModel>()
                .HasKey(p => p.UserId);

            modelBuilder.Entity<UserDBModel>()
                .Property(p => p.FirstName)
                    .HasMaxLength(40);

            modelBuilder.Entity<UserDBModel>()
                .Property(p => p.LastName)
                    .HasMaxLength(80);

            modelBuilder.Entity<UserDBModel>()
                .Property(p => p.UserType)
                    .HasMaxLength(20);
        }
    }
}