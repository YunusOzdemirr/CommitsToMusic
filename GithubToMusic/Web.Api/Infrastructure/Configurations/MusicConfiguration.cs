using GithubCommitsToMusic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GithubCommitsToMusic.Infrastructure.Configurations
{
    public class MusicConfiguration : IEntityTypeConfiguration<Music>
    {
        public void Configure(EntityTypeBuilder<Music> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.HasOne(a=>a.User).WithMany(a=>a.Musics).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("Musics");
        }
    }
}
