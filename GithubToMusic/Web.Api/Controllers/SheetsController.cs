using GithubCommitsToMusic.Features.Queries.Sheets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GithubCommitsToMusic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SheetsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SheetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetSheetsAsync(CancellationToken cancellationToken = default)
        {
            var query = new GetSheetsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            if (result == null || result.Count == 0)
                return NotFound();
            return Ok(result);
        }
    }
}
