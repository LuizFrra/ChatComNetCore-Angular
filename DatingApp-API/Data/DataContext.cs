using DatingApp.API.Models;
using DatingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Value>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Value>().ToTable("tbl_values");

            modelBuilder.Entity<User>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.PasswordHash).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.ImagePath).IsRequired();
            modelBuilder.Entity<User>().ToTable("tbl_users");
        }
    }
}
