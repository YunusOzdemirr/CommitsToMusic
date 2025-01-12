using GithubCommitsToMusic.Models;
using Microsoft.EntityFrameworkCore;

namespace GithubCommitsToMusic.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Commit> Commits { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
