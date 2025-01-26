namespace GithubCommitsToMusic.Models
{
    public class Query
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
