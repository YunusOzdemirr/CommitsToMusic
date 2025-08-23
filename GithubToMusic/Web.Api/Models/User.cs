namespace GithubCommitsToMusic.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string IpAddress { get; set; }
        public int TotalCommit { get; set; }
        public ICollection<Commit> Commits { get; set; }
        public ICollection<Music> Musics { get; set; }
    }
}
