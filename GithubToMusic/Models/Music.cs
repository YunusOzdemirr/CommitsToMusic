namespace GithubCommitsToMusic.Models
{
    public class Music
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string VirtualPath { get; set; }
        public string Path { get; set; }
    }
}
