using GithubCommitsToMusic.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GithubCommitsToMusic.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasMany(a => a.Commits).WithOne(a => a.User).HasForeignKey(a => a.UserId);
        builder.ToTable("Users");
    }
}
