using GithubCommitsToMusic.Enums;
using GithubCommitsToMusic.Models;
using MediatR;

namespace GithubCommitsToMusic.Features.Queries.Users
{
    public class GetUsersRankQuery : IRequest<IList<User>>
    {
        public OrderBy OrderBy { get; set; }
    }
}
