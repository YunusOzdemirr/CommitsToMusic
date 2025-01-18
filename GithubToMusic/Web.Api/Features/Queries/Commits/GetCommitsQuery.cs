using GithubCommitsToMusic.Args;
using GithubCommitsToMusic.Dtos;
using GithubCommitsToMusic.Models;
using MediatR;

namespace GithubCommitsToMusic.Features.Queries.Commits
{
    public class GetCommitsQuery : GetCommitsArgs, IRequest<IList<CommitDto>>
    {
    }
}
