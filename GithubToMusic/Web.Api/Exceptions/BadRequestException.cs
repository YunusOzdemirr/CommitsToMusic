namespace GithubCommitsToMusic.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
            ErrorMessage = message;
        }
        public string ErrorMessage { get; set; }
    }
}
