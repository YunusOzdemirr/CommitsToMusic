using GithubCommitsToMusic.Constants;
using GithubCommitsToMusic.Dtos;
using GithubCommitsToMusic.Enums;
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
            var musicExist = await _applicationDbContext.Musics.FirstOrDefaultAsync(a => a.Name == request.UserName, cancellationToken);
            if (musicExist is not null)
            {
                return musicExist;
            }
            string path, specialCharacter;
            GetMusicPathAndSpecialCharacter(request, out path, out specialCharacter);

            var sheets = await _applicationDbContext.Sheets.AsNoTracking().ToListAsync(cancellationToken);
            var generatedMusics = GenerateMusic(sheets, request.Commits, request.PatternType);

            MergeMp3Files(sheets, path, specialCharacter);

            var user = await _applicationDbContext.Users
                .AsNoTracking()
                .Where(a => a.UserName == request.UserName)
                .Select(a => new { Id = a.Id, UserName = a.UserName })
                .FirstOrDefaultAsync(cancellationToken);

            var musicVirtualPath = $"/GeneratedMusics/{request.UserName}.mp3";

            var music = new Music()
            {
                Name = request.UserName,
                Path = path,
                VirtualPath = musicVirtualPath,
                UserId = user?.Id ?? 0
            };
            await _applicationDbContext.Musics.AddAsync(music, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            music.User = null;
            return music;
        }

        private static void GetMusicPathAndSpecialCharacter(GenerateMusicQuery request, out string path, out string specialCharacter)
        {
            path = Directory.GetCurrentDirectory();
            var domain = AppDomain.CurrentDomain.BaseDirectory;
            specialCharacter = string.Empty;
            if (path.Contains("/"))
                specialCharacter = "/";
            if (path.Contains(@"\"))
                specialCharacter = @"\";

            path = string.Concat(path, specialCharacter, "wwwroot", specialCharacter, "Sheets", specialCharacter, "GeneratedMusics", specialCharacter, request.UserName + ".mp3");
        }

        void MergeMp3Files(List<Sheet> inputFiles, string outputFile, string specialCharacter)
        {
            using (var waveFileWriter = new WaveFileWriter(outputFile, new WaveFormat()))
            {
                var path = Directory.GetCurrentDirectory();
                var pathss = string.Concat(path, specialCharacter, "wwwroot", specialCharacter, "Sheets", specialCharacter);
                foreach (var sheet in inputFiles)
                {
                    var filePath = pathss + sheet.Name;
                    if (Directory.Exists(filePath))
                    {
                        ConvertMp3ToWav(filePath.Replace(".wav", ".mp3"), filePath);
                    }

                    using (var reader = new WaveFileReader(filePath))
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

        private static void ConvertMp3ToWav(string _inPath_, string _outPath_)
        {
            using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
            {
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(_outPath_, pcm);
                }
            }
        }

        private List<Sheet> GenerateMusic(List<Sheet> sheets, IList<CommitDto> commits, RhythmPatternType rhythmPatternType)
        {
            List<Sheet> musicNotes = new();
            var commitAverage = (int)commits.Average(a => a.Count);
            int rhythmIndex = 0;
            var rhythmPattern = Rhythmes.GetNotesPattern(rhythmPatternType);
            var firstNote = rhythmPattern[0];
            var secondNote = rhythmPattern[1];
            var thirdNote = rhythmPattern[2];
            var fourTh = rhythmPattern[3];
            foreach (var commit in commits)
            {
                Note selectedNote;

                if (commit.Count * 1.5 > commitAverage)
                {
                    selectedNote = rhythmPattern[rhythmIndex % rhythmPattern.Length]; // Akor döngüsü
                }
                else if (commit.Count * 2 < commitAverage)
                {
                    selectedNote = firstNote;
                }
                else if (commit.Count < commitAverage)
                {
                    selectedNote = secondNote;
                }
                else if (commit.Count == commitAverage)
                {
                    selectedNote = thirdNote;
                }
                else
                {
                    selectedNote = fourTh; // Alçak commit değerleri için sakin sesler
                }

                var note = sheets.FirstOrDefault(a => a.Note == selectedNote);
                if (note == null || note == musicNotes.LastOrDefault())
                {
                    note = sheets.FirstOrDefault(a => a.Note == firstNote); // Alternatif bir nota seç
                }

                musicNotes.AddIfNotNull(note);
                rhythmIndex++;
            }

            return musicNotes;
        }
    }
}
