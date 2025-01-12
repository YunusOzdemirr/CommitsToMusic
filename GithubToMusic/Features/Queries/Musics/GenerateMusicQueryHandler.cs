using GithubCommitsToMusic.Dtos;
using GithubCommitsToMusic.Exceptions;
using GithubCommitsToMusic.Extensions;
using GithubCommitsToMusic.Interfaces;
using GithubCommitsToMusic.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NAudio.Wave;

namespace GithubCommitsToMusic.Features.Queries.Musics
{
    public class GenerateMusicQueryHandler : IRequestHandler<GenerateMusicQuery, Music>
    {
        private readonly IApplicationDbContext _applicationDbContext;
      

        public GenerateMusicQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Music> Handle(GenerateMusicQuery request, CancellationToken cancellationToken)
        {
            var path = Directory.GetCurrentDirectory();
            var pathss = path + "\\wwwroot\\Sheets\\";
            var files = Directory.GetFiles(pathss);
            var sheets = await _applicationDbContext.Sheets.AsNoTracking().ToListAsync(cancellationToken);
            //PlayNotesSequentially(files.ToList());
            var generatedMusics = GenerateMusic(sheets, request.Commits);
            MergeMp3Files(generatedMusics, path + $"\\wwwroot\\Sheets\\GeneratedMusics\\{request.UserName}.mp3");
            return default;
        }

        static void MergeMp3Files(List<Sheet> inputFiles, string outputFile)
        {
            using (var waveFileWriter = new WaveFileWriter(outputFile, new WaveFormat()))
            {
                var path = Directory.GetCurrentDirectory();
                var pathss = path + "\\wwwroot\\Sheets\\";
                foreach (var sheet in inputFiles)
                {
                    var filePath = pathss + sheet.Name;
                    if (Directory.Exists(filePath))
                        throw new BadRequestException("Dosya bulunamadı.");
                    using (var reader = new Mp3FileReader(filePath))
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead;
                        while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            waveFileWriter.Write(buffer, 0, bytesRead);
                        }
                    }
                }
            }
        }

        private List<Sheet> GenerateMusic(List<Sheet> sheets, IList<CommitDto> commits)
        {
            List<Sheet> musicNotes = new();
            var commitAverage = (int)commits.Average(a => a.Count);
            int rhythmIndex = 0;

            foreach (var commit in commits)
            {
                Enums.Note selectedNote;

                if (commit.Count * 1.5 > commitAverage)
                {
                    selectedNote = rhythmPattern[rhythmIndex % rhythmPattern.Length]; // Akor döngüsü
                }
                else if (commit.Count * 2 < commitAverage)
                {
                    selectedNote = Enums.Note.Sol;
                }
                else if (commit.Count < commitAverage)
                {
                    selectedNote = Enums.Note.Do;
                }
                else if (commit.Count == commitAverage)
                {
                    selectedNote = Enums.Note.Fa;
                }
                else
                {
                    selectedNote = Enums.Note.La; // Alçak commit değerleri için sakin sesler
                }

                var note = sheets.FirstOrDefault(a => a.Note == selectedNote);
                if (note == null || note == musicNotes.LastOrDefault())
                {
                    note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Re); // Alternatif bir nota seç
                }

                musicNotes.AddIfNotNull(note);
                rhythmIndex++;
            }

            return musicNotes;
        }


        //private List<Sheet> GenerateMusic(List<Sheet> sheets, IList<CommitDto> commits)
        //{
        //    List<Sheet> musicNotes = new();
        //    var commitAverage = (int)commits.Average(a => a.Count);
        //    var randomMultiplier = 1.5;//new Random().Next(1, 4)
        //    foreach (var commit in commits)
        //    {
        //        if (commit.Count > commitAverage)
        //        {
        //            if (commit.Count / randomMultiplier > commitAverage)
        //            {
        //                var note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Do);
        //                if (note == musicNotes.LastOrDefault())
        //                {
        //                    note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Sol);
        //                    musicNotes.AddIfNotNull(note);
        //                }
        //                else
        //                    musicNotes.AddIfNotNull(note);
        //            }
        //            else if (commit.Count / randomMultiplier < commitAverage)
        //            {
        //                var note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Mi);
        //                if (note == musicNotes.LastOrDefault())
        //                {
        //                    note = sheets.FirstOrDefault(a => a.Note == Enums.Note.La);
        //                    musicNotes.AddIfNotNull(note);
        //                }
        //                else
        //                    musicNotes.AddIfNotNull(note);
        //            }
        //            else
        //            {
        //                var note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Sol);
        //                if (note == musicNotes.LastOrDefault())
        //                {
        //                    note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Re);
        //                    musicNotes.AddIfNotNull(note);
        //                }
        //                else
        //                    musicNotes.AddIfNotNull(note);
        //            }
        //        }
        //        else if (commit.Count == commitAverage)
        //        {
        //            var note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Fa);
        //            if (note == musicNotes.LastOrDefault())
        //            {
        //                note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Si);
        //                musicNotes.AddIfNotNull(note);
        //            }
        //            else
        //                musicNotes.AddIfNotNull(note);
        //        }
        //        else if (commit.Count < commitAverage)
        //        {
        //            if (commit.Count * randomMultiplier > commitAverage)
        //            {
        //                var note = sheets.FirstOrDefault(a => a.Note == Enums.Note.La);
        //                if (note == musicNotes.LastOrDefault())
        //                {
        //                    note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Mi);
        //                    musicNotes.AddIfNotNull(note);
        //                }
        //                else
        //                    musicNotes.AddIfNotNull(note);
        //            }
        //            else if (commit.Count * randomMultiplier < commitAverage)
        //            {
        //                var note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Si);
        //                if (note == musicNotes.LastOrDefault())
        //                {
        //                    note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Fa);
        //                    musicNotes.AddIfNotNull(note);
        //                }
        //                else
        //                    musicNotes.AddIfNotNull(note);
        //            }
        //            else
        //            {
        //                var note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Re);
        //                if (note == musicNotes.LastOrDefault())
        //                {
        //                    note = sheets.FirstOrDefault(a => a.Note == Enums.Note.Sol);
        //                    musicNotes.AddIfNotNull(note);
        //                }
        //                else
        //                    musicNotes.AddIfNotNull(note);
        //            }
        //        }
        //    }
        //    return musicNotes;
        //}

    }
}
