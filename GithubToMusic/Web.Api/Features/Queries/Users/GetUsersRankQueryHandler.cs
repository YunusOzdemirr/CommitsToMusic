using GithubCommitsToMusic.Enums;
using GithubCommitsToMusic.Interfaces;
using GithubCommitsToMusic.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GithubCommitsToMusic.Features.Queries.Users
{
    public class GetUsersRankQueryHandler(IApplicationDbContext applicationDbContext) : IRequestHandler<GetUsersRankQuery, IList<User>>
    {
        public async Task<IList<User>> Handle(GetUsersRankQuery request, CancellationToken cancellationToken)
        {
            var users = await applicationDbContext.Users.Include(a => a.Commits).Where(a => a.TotalCommit > 0).ToListAsync(cancellationToken);
            if (users == null || users.Count == 0)
                return null;
            if (request.OrderBy == OrderBy.Ascending)
            {
                return users.OrderByDescending(a => a.CreatedOn).ToList();
            }
            else
            {
                return users.OrderByDescending(a => a.TotalCommit).ToList();
            }
        }
    }
}
