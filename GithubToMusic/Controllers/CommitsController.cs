using GithubCommitsToMusic.Features.Queries;
using GithubCommitsToMusic.Features.Queries.Commits;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GithubCommitsToMusic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommitsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommitsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> GetCommits(string userName, DateTime? from, DateTime? to, CancellationToken cancellationToken = default)
        {
            var query = new GetCommitsQuery()
            {
                UserName = userName,
                StartDate = from,
                EndDate = to,
                IpAddress = HttpContext.Connection.RemoteIpAddress.ToString()
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
