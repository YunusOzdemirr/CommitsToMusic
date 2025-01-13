using GithubCommitsToMusic.Enums;

namespace GithubCommitsToMusic.Constants
{
    public static class Rhythmes
    {
        public static Note[] defaultRhythmPattern = new[] { Note.Do, Note.Mi, Note.Sol, Note.La };
        public static Note[] populerRhythmPattern = new[] { Note.Do, Note.La, Note.Fa, Note.Sol };
        public static Note[] happyRhythmPattern = new[] { Note.Do, Note.Sol, Note.La, Note.Fa };
        public static Note[] dramaticRhythmPattern = new[] { Note.La, Note.Fa, Note.Do, Note.Sol };
        public static Note[] sadRhythmPattern = new[] { Note.Sol, Note.Do, Note.Fa, Note.La };
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
