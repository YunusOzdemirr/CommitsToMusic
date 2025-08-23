using GithubCommitsToMusic.Enums;
using GithubCommitsToMusic.Features.Queries.Commits;
using GithubCommitsToMusic.Features.Queries.Musics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GithubCommitsToMusic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MusicController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateMusic(string userName, DateTime? startDate, DateTime? endDate, RhythmPatternType? rhytmPatternType, CancellationToken cancellationToken = default)
        {
            var query = new GetCommitsQuery()
            {
                UserName = userName,
                IpAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                EndDate = endDate,
                StartDate = startDate
            };
            var result = await _mediator.Send(query, cancellationToken);
            var generateMusicQuery = new GenerateMusicQuery()
            {
                Commits = result.Take(100).ToList(),
                UserName = userName,
                PatternType = rhytmPatternType.HasValue ? rhytmPatternType.Value : RhythmPatternType.Default,
            };
            var generatedMusic = await _mediator.Send(generateMusicQuery, cancellationToken);
            return Ok(generatedMusic);
        }
    }
}
