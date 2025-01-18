using GithubCommitsToMusic.Enums;

namespace GithubCommitsToMusic.Models
{
    public class Sheet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Note Note { get; set; }
        public string VirtualPath { get; set; }
    }
}
