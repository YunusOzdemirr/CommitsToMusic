using GithubCommitsToMusic.Enums;

namespace GithubCommitsToMusic.Constants
{
    public static class Rhythmes
    {
        public static Note[] defaultRhythmPattern = new[] { Enums.Note.Do, Enums.Note.Mi, Enums.Note.Sol, Enums.Note.La };
        public static Note[] populerRhythmPattern = new[] { Enums.Note.Do, Enums.Note.La, Enums.Note.Fa, Enums.Note.Sol };
        public static Note[] happyRhythmPattern = new[] { Enums.Note.Do, Enums.Note.Sol, Enums.Note.La, Enums.Note.Fa };
        public static Note[] dramaticRhythmPattern = new[] { Enums.Note.La, Enums.Note.Fa, Enums.Note.Do, Enums.Note.Sol };
        public static Note[] sadRhythmPattern = new[] { Enums.Note.Sol, Enums.Note.Do, Enums.Note.Fa, Enums.Note.La };
        public static Note[] GetNotesPattern(RhythmPatternType rhytmPatternType)
        {
            switch (rhytmPatternType)
            {
                case RhythmPatternType.Default:
                    return defaultRhythmPattern;
                case RhythmPatternType.Populer:
                    return populerRhythmPattern;
                case RhythmPatternType.Happy:
                    return happyRhythmPattern;
                case RhythmPatternType.Dramatic:
                    return dramaticRhythmPattern;
                case RhythmPatternType.Sad:
                    return sadRhythmPattern;
                default:
                    return defaultRhythmPattern;
            }
        }
    }
}
