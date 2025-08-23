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
                    VirtualPath = "/Sheets/Do.wav",
                    Note = Enums.Note.Do,
                    Name = "Do.wav"
                },
                new Sheet
                {
                    Id = 2,
                    VirtualPath = "/Sheets/Re.wav",
                    Note = Enums.Note.Re,
                    Name = "Re.wav"
                },
                new Sheet
                {
                    Id = 3,
                    VirtualPath = "/Sheets/Mi.wav",
                    Note = Enums.Note.Mi,
                    Name = "Mi.wav"
                },
                new Sheet
                {
                    Id = 4,
                    VirtualPath = "/Sheets/Fa.wav",
                    Note = Enums.Note.Fa,
                    Name = "Fa.wav"
                },
                new Sheet
                {
                    Id = 5,
                    VirtualPath = "/Sheets/Sol.wav",
                    Note = Enums.Note.Sol,
                    Name = "Sol.wav"
                },
                new Sheet
                {
                    Id = 6,
                    VirtualPath = "/Sheets/La.wav",
                    Note = Enums.Note.La,
                    Name = "La.wav"
                },
                new Sheet
                {
                    Id = 7,
                    VirtualPath = "/Sheets/Si.wav",
                    Note = Enums.Note.Si,
                    Name = "Si.wav"
                });
            builder.ToTable("Sheets");
        }
    }
}
