using GithubCommitsToMusic.Args;
using GithubCommitsToMusic.Dtos;
using GithubCommitsToMusic.Models;

namespace GithubCommitsToMusic.Services
{
    public interface ICommitService
    {
        Task<List<CommitDto>> GetAllCommitsAsync(GetCommitsArgs args, CancellationToken cancellationToken = default);
    }
}
