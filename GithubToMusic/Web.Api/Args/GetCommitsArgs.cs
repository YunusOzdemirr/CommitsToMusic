namespace GithubCommitsToMusic.Args
{
    public class GetCommitsArgs
    {
        public string UserName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string IpAddress { get; set; }
    }
}
