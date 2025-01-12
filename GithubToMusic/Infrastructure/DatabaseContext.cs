using GithubCommitsToMusic.Infrastructure.Configurations;
using GithubCommitsToMusic.Interfaces;
using GithubCommitsToMusic.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GithubCommitsToMusic.Infrastructure
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Commit> Commits { get; set; }
        public DbSet<Sheet> Sheets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CommitConfiguration());
            modelBuilder.ApplyConfiguration(new SheetConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer(
            // "Server=localhost;Database=Ecommerce;Persist Security Info=True;User ID=sa;Password=yourStrong(!)Password; Trusted_Connection=false;");
            //optionsBuilder.UseSqlServer(
            //    "Server=westartic.com;Database=CommitToMusic;Persist Security Info=True;User ID=sa;Password=yourStrong(!)Password; Trusted_Connection=false;TrustServerCertificate=True");
            optionsBuilder.UseSqlServer(
            "Server=localhost;Database=CommitsToMusic;Persist Security Info=True;Trusted_Connection=True;Encrypt=false");

            // @"Server=localhost;Database=Ecommerce6;Trusted_Connection=True");
            //Server=localhost;Database=master;Trusted_Connection=True;

            base.OnConfiguring(optionsBuilder);
        }
    }
}
