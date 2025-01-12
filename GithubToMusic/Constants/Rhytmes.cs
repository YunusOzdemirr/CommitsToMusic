using GithubCommitsToMusic.Enums;

namespace GithubCommitsToMusic.Constants
{
    public record class Rhytmes
    {
        public Note[] defaultRhythmPattern = new[] { Enums.Note.Do, Enums.Note.Mi, Enums.Note.Sol, Enums.Note.La };
        public Note[] populerRhythmPattern = new[] { Enums.Note.Do, Enums.Note.La, Enums.Note.Fa, Enums.Note.Sol };
        public Note[] happyRhythmPattern = new[] { Enums.Note.Do, Enums.Note.Sol, Enums.Note.La, Enums.Note.Fa };
        public Note[] dramaticRhythmPattern = new[] { Enums.Note.La, Enums.Note.Fa, Enums.Note.Do, Enums.Note.Sol };
    }
}
