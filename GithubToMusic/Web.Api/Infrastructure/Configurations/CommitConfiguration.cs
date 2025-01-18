using GithubCommitsToMusic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GithubCommitsToMusic.Infrastructure.Configurations
{
    public class CommitConfiguration : IEntityTypeConfiguration<Commit>
    {
        public void Configure(EntityTypeBuilder<Commit> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.User).WithMany(a => a.Commits).HasForeignKey(a => a.UserId);
            builder.ToTable("Commits");
        }
    }
}
