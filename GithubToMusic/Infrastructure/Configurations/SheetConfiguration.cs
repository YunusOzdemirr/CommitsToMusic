using GithubCommitsToMusic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GithubCommitsToMusic.Infrastructure.Configurations
{
    public class SheetConfiguration : IEntityTypeConfiguration<Sheet>
    {
        public void Configure(EntityTypeBuilder<Sheet> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasData(
                new Sheet
                {
                    Id = 1,
                    VirtualPath = "/Sheets/Do.MP3",
                    Name="Do"
                },
                new Sheet
                {
                    Id = 2,
                    VirtualPath = "/Sheets/Re.MP3",
                    Name = "Re"
                },
                new Sheet
                {
                    Id = 3,
                    VirtualPath = "/Sheets/Mi.MP3",
                    Name = "Mi"
                },
                new Sheet
                {
                    Id = 4,
                    VirtualPath = "/Sheets/Fa.MP3",
                    Name = "Fa"
                },
                new Sheet
                {
                    Id = 5,
                    VirtualPath = "/Sheets/Sol.MP3",
                    Name = "Sol"
                },
                new Sheet
                {
                    Id = 6,
                    VirtualPath = "/Sheets/La.MP3",
                    Name = "La"
                },
                new Sheet
                {
                    Id = 7,
                    VirtualPath = "/Sheets/Si.MP3",
                    Name = "Si"
                });
            builder.ToTable("Sheets");
        }
    }
}
