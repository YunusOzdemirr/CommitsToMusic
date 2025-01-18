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
                    Note = Enums.Note.Do,
                    Name = "Do.MP3"
                },
                new Sheet
                {
                    Id = 2,
                    VirtualPath = "/Sheets/Re.MP3",
                    Note = Enums.Note.Re,
                    Name = "Re.MP3"
                },
                new Sheet
                {
                    Id = 3,
                    VirtualPath = "/Sheets/Mi.MP3",
                    Note = Enums.Note.Mi,
                    Name = "Mi.MP3"
                },
                new Sheet
                {
                    Id = 4,
                    VirtualPath = "/Sheets/Fa.MP3",
                    Note = Enums.Note.Fa,
                    Name = "Fa.MP3"
                },
                new Sheet
                {
                    Id = 5,
                    VirtualPath = "/Sheets/Sol.MP3",
                    Note = Enums.Note.Sol,
                    Name = "Sol.MP3"
                },
                new Sheet
                {
                    Id = 6,
                    VirtualPath = "/Sheets/La.MP3",
                    Note = Enums.Note.La,
                    Name = "La.MP3"
                },
                new Sheet
                {
                    Id = 7,
                    VirtualPath = "/Sheets/Si.MP3",
                    Note = Enums.Note.Si,
                    Name = "Si.MP3"
                });
            builder.ToTable("Sheets");
        }
    }
}
