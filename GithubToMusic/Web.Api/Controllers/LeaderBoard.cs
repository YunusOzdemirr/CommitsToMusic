using GithubCommitsToMusic.Enums;
using GithubCommitsToMusic.Features.Queries.Users;
using GithubCommitsToMusic.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GithubCommitsToMusic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderBoard : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _applicationDbContext;
        public LeaderBoard(IMediator mediator, IApplicationDbContext applicationDbContext)
        {
            _mediator = mediator;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetRank(OrderBy orderBy, CancellationToken cancellationToken = default)
        {
            var query = new GetUsersRankQuery()
            {
                OrderBy = orderBy
            };
            var result = await _mediator.Send(query, cancellationToken);
            if (result == null || result.Count == 0)
            {
                return NotFound();
            }

            var userRanks = result.Select((user, index) => new UserRank(user.UserName, user.TotalCommit, index + 1, user.CreatedOn)).ToList();
            return Ok(userRanks);
        }

    }
    public sealed record UserRank(string userName, int totalCommit, int rank, DateTime createdOn);
}
