using GithubCommitsToMusic.Dtos;
using GithubCommitsToMusic.Models;

namespace GithubCommitsToMusic.Extensions
{
    public static class CommitExtension
    {
        public static CommitDto GetCommitDto(this Commit commit)
        {
            return new CommitDto
            {
                Count = commit.Count,
                Month = commit.Month,
                Day = commit.Day
            };
        }

        public static List<CommitDto> GetCommitsDto(this List<Commit> commits)
        {
            List<CommitDto> commitDtos = new();
            foreach (var commit in commits)
            {
                commitDtos.Add(new CommitDto
                {
                    Count = commit.Count,
                    Month = commit.Month,
                    Day = commit.Day
                });
            }
            return commitDtos;
        }
    }

}
