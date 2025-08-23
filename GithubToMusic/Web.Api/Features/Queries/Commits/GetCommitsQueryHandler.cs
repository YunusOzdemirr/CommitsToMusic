using GithubCommitsToMusic.Dtos;
using GithubCommitsToMusic.Services;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace GithubCommitsToMusic.Features.Queries.Commits
{
    public class GetCommitsQueryHandler : IRequestHandler<GetCommitsQuery, IList<CommitDto>>
    {
        private readonly ICommitService _commitService;
        private readonly IMemoryCache _memoryCache;
        public GetCommitsQueryHandler(ICommitService commitService, IMemoryCache memoryCache)
        {
            _commitService = commitService;
            _memoryCache = memoryCache;
        }

        public async Task<IList<CommitDto>> Handle(GetCommitsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = request.UserName;
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                cacheKey += request.StartDate.Value.Year + request.EndDate.Value.Year;
            }
            var result = _memoryCache.Get<IList<CommitDto>>(cacheKey);
            if (result != null && result.Count > 0)
            {
                return result;
            }
            var commits = await _commitService.GetAllCommitsAsync(request, cancellationToken);
          
            if (commits != null && commits.Count > 0)
            {
                _memoryCache.Set(cacheKey, commits, TimeSpan.FromMinutes(5));
            }
            return commits;
        }
    }
}
