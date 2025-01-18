using GithubCommitsToMusic.Interfaces;
using GithubCommitsToMusic.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GithubCommitsToMusic.Features.Queries.Sheets
{
    public class GetSheetsQueryHandler : IRequestHandler<GetSheetsQuery, IList<Sheet>>
    {
        private IApplicationDbContext _applicationDbContext;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "Sheets";
        public GetSheetsQueryHandler(IApplicationDbContext applicationDbContext, IMemoryCache memoryCache)
        {
            _applicationDbContext = applicationDbContext;
            _memoryCache = memoryCache;
        }

        public async Task<IList<Sheet>> Handle(GetSheetsQuery request, CancellationToken cancellationToken)
        {
            var cachedResult = _memoryCache.Get<IList<Sheet>>(CacheKey);
            if (cachedResult != null && cachedResult.Count > 0)
                return cachedResult;
            var result = await _applicationDbContext.Sheets.ToListAsync(cancellationToken);
            _memoryCache.Set(CacheKey, result, TimeSpan.FromMinutes(5));
            return result;
        }
    }
}
