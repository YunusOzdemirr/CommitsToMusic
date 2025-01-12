using GithubCommitsToMusic.Models;

namespace GithubCommitsToMusic.Dtos
{
    public class CommitDto
    {
        public int Count { get; set; }
        public string Month { get; set; }
        public int Day { get; set; }
    }
}
