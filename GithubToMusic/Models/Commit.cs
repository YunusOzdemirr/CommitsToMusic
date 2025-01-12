namespace GithubCommitsToMusic.Models;

public class Commit
{
    public int Id { get; set; }
    public int Count { get; set; }
    public string Month { get; set; }
    public int Day { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}