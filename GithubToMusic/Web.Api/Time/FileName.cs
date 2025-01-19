namespace GithubCommitsToMusic.Time
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
    public interface IDateTimeProvider
    {
        public DateTime UtcNow { get; }
    }

}
