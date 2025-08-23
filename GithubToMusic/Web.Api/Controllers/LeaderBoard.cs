using GithubCommitsToMusic.Enums;
using GithubCommitsToMusic.Features.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GithubCommitsToMusic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderBoard : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaderBoard(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetRank(OrderBy orderBy, CancellationToken cancellationToken = default)
        {
            var query = new GetUsersRankQuery()
            {
                OrderBy = orderBy
            };
            var result = await _mediator.Send(query, cancellationToken);
            if(result==null || result.Count==0)
            {
                return NotFound();
            }
            return Ok(result.Select((a, b) => new UserRank(a.UserName, a.TotalCommit, b + 1, a.CreatedOn)));
        }
    }
    public record UserRank(string userName, int totalCommit, int rank, DateTime createdOn);
}
