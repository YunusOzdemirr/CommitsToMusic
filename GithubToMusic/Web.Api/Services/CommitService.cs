using AngleSharp;
using GithubCommitsToMusic.Args;
using GithubCommitsToMusic.Dtos;
using GithubCommitsToMusic.Exceptions;
using GithubCommitsToMusic.Extensions;
using GithubCommitsToMusic.Interfaces;
using GithubCommitsToMusic.Models;
using GithubCommitsToMusic.Time;
using Microsoft.EntityFrameworkCore;

namespace GithubCommitsToMusic.Services
{
    public class CommitService : ICommitService
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        public CommitService(IApplicationDbContext applicationDbContext, IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        string[] patterns = new string[] { "nd.", "rd.", "st.", "th." };
        private const string NoContributions = "No contributions";
        private const string CommitHtmlClass = "sr-only position-absolute";
        public async Task<List<CommitDto>> GetAllCommitsAsync(GetCommitsArgs args, CancellationToken cancellationToken = default)
        {
            ValidateArgs(args);
            var user = await _applicationDbContext.Users.Select(a => new User { Id = a.Id, UserName = a.UserName }).FirstOrDefaultAsync(a => a.UserName == args.UserName);
            if (user != null)
            {
                var availableForNewQuery = await CheckPreviousQueriesAsync(args, user, cancellationToken);
                if (availableForNewQuery)
                {
                    var commitsdb = await _applicationDbContext.Commits.Where(a => a.UserId == user.Id).Take(200).ToListAsync(cancellationToken);
                    return commitsdb.GetCommitsDto();
                }
            }
            //https://github.com/users/yunusozdemirr/contributions?from=2024-01-01&to=2025-01-11
            var url = $"https://github.com/users/{args.UserName}/contributions";
            if (args.StartDate.HasValue && args.EndDate.HasValue)
            {
                //url+= "?from=2024-01-01&to=2025-01-1"
                var startDate = args.StartDate.Value.ToString("yyyy-MM-dd");
                var endDate = args.EndDate.Value.ToString("yyyy-MM-dd");
                url += $"?from={startDate}&to={endDate}";
            }
            var baseAddress = new Uri(url);
            var httpClient = new HttpClient() { BaseAddress = baseAddress };

            var result = await httpClient.GetAsync(baseAddress, cancellationToken);
            var content = await result.Content.ReadAsStringAsync(cancellationToken);

            var commits = await ConvertHtmlToCommits(content);
            await CreateUserAsync(args, commits.Take(200).ToList(), cancellationToken);
            return commits.GetCommitsDto();
        }

        private async Task<bool> CheckPreviousQueriesAsync(GetCommitsArgs args, User user, CancellationToken cancellationToken)
        {
            if (args.StartDate.HasValue && args.EndDate.HasValue)
            {
                var queries = await _applicationDbContext.Queries.Where(a => a.UserName == user.UserName).ToListAsync(cancellationToken);
                if (queries.Count > 10)
                    throw new BadRequestException("You have already queried this date range before. Please try again later.");
                //var startDateResult = queries.Any(a => a.StartDate <= args.StartDate.Value.AddMonths(3)
                //|| a.StartDate >= args.StartDate.Value.AddMonths(-3));
                //var endDateResult = queries.Any(a => a.EndDate <= args.EndDate.Value.AddMonths(3)
                //|| a.EndDate >= args.EndDate.Value.AddMonths(-3));
                //if (startDateResult || endDateResult)
                //{
                //    throw new BadRequestException("You have already queried this date range before. Please choose a date 3 months away.");
                //}
                return true;
            }
            return false;
        }

        private async Task CreateUserAsync(GetCommitsArgs args, IList<Commit> commits, CancellationToken cancellationToken)
        {
            var userExist = _applicationDbContext.Users.AsNoTracking().Any(a => a.UserName == args.UserName);
            if (userExist) return;
            var queryResult = await _applicationDbContext.Queries.AnyAsync(a => a.UserName == args.UserName
            && (a.StartDate != args.StartDate || a.EndDate != args.EndDate), cancellationToken);
            if (queryResult && args.StartDate.HasValue && args.EndDate.HasValue)
                _applicationDbContext.Queries.Add(new Query()
                {
                    UserName = args.UserName,
                    StartDate = args.StartDate,
                    EndDate = args.EndDate,
                });

            _applicationDbContext.Users.Add(new User
            {
                UserName = args.UserName,
                CreatedOn = _dateTimeProvider.UtcNow,
                Commits = commits,
                IpAddress = args.IpAddress
            });
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<List<Commit>> ConvertHtmlToCommits(string content)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(content));
            var commits = document.GetElementsByClassName(CommitHtmlClass);

            List<Commit> newCommits = new List<Commit>();
            foreach (var commit in commits)
            {
                if (commit.InnerHtml.Contains(NoContributions)) continue;
                var splittedData = commit.InnerHtml.Split(" ");
                if (splittedData.Length < 4) continue;
                var commitCount = splittedData[0];
                var commitMonth = splittedData[3];
                int commitDay = ParseCommitDay(splittedData[4]);
                int.TryParse(commitCount, out int parsedCommitCount);
                newCommits.Add(new Commit { Count = parsedCommitCount, Month = commitMonth, Day = commitDay });
            }
            return newCommits;
        }
        private void ValidateArgs(GetCommitsArgs args)
        {
            if (string.IsNullOrEmpty(args.UserName))
                throw new BadRequestException("Username is required");
            if (args.StartDate.HasValue && !args.EndDate.HasValue)
                throw new BadRequestException("End Date is required");
            if (!args.StartDate.HasValue && args.EndDate.HasValue)
                throw new BadRequestException("Start Date is required");
        }
        private int ParseCommitDay(string commit)
        {
            foreach (var patternValue in patterns)
            {
                string commitDay = commit.Split(patternValue)[0];
                var parsed = int.TryParse(commitDay, out int parsedCommitDay);
                if (parsed)
                {
                    return parsedCommitDay;
                }
            }
            return 0;
        }
    }
}
