using AngleSharp;
using GithubCommitsToMusic.Args;
using GithubCommitsToMusic.Dtos;
using GithubCommitsToMusic.Exceptions;
using GithubCommitsToMusic.Extensions;
using GithubCommitsToMusic.Interfaces;
using GithubCommitsToMusic.Models;
using GithubCommitsToMusic.Time;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
                if (!availableForNewQuery)
                {
                    var commitsdb = await _applicationDbContext.Commits.Where(a => a.UserId == user.Id).Take(200).ToListAsync(cancellationToken);
                    return commitsdb.GetCommitsDto();
                }
            }
            string content = await GetGithubContent(args, cancellationToken);
            var commits = await ConvertHtmlToCommits(content);
            var allCommits = await GetAllCommitsHistory(args, cancellationToken);
            if (allCommits == null || allCommits.Count == 0)
                return default;

            commits = commits.Count == 0 ? allCommits : commits;
            await CreateQueryAsync(args, cancellationToken);
            await CreateUserAsync(args, commits.Take(200).ToList(), allCommits, cancellationToken);
            return commits.GetCommitsDto();
        }

        public async Task<List<Commit>> GetAllCommitsHistory(GetCommitsArgs args, CancellationToken cancellationToken = default)
        {
            args.StartDate = DateTime.Now.AddYears(-25);
            args.EndDate = DateTime.Now.AddYears(-5);
            List<Commit> allCommits = new();
            for (int i = 1; i < 6; i++)
            {
                args.EndDate = new DateTime(args.EndDate.Value.Year + 1, 1, 1);
                string content = await GetGithubContent(args, cancellationToken);
                var result = await ConvertHtmlToCommits(content);
                allCommits.AddRange(result);
            }

            return allCommits;
        }


        private static async Task<string> GetGithubContent(GetCommitsArgs args, CancellationToken cancellationToken)
        {
            var url = $"https://github.com/users/{args.UserName}/contributions";
            if (args.StartDate.HasValue && args.EndDate.HasValue)
            {
                var startDate = args.StartDate.Value.ToString("yyyy-MM-dd");
                var endDate = args.EndDate.Value.ToString("yyyy-MM-dd");
                url += $"?from={startDate}&to={endDate}";
            }
            var baseAddress = new Uri(url);
            var httpClient = new HttpClient() { BaseAddress = baseAddress };

            var result = await httpClient.GetAsync(baseAddress, cancellationToken);
            var content = await result.Content.ReadAsStringAsync(cancellationToken);
            return content;
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

        private async Task CreateUserAsync(GetCommitsArgs args, IList<Commit> commits, IList<Commit> allCommits, CancellationToken cancellationToken)
        {
            var existUser = await _applicationDbContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.UserName == args.UserName);
            var totalCommitCount = allCommits.Sum(a => a.Count);
            if (existUser != null)
            {
                if (existUser.TotalCommit == 0 || existUser.TotalCommit < totalCommitCount)
                {
                    existUser.TotalCommit = totalCommitCount;
                    _applicationDbContext.Users.Update(existUser);
                    await _applicationDbContext.SaveChangesAsync(cancellationToken);
                }
                return;
            }
            _applicationDbContext.Users.Add(new User
            {
                UserName = args.UserName,
                CreatedOn = _dateTimeProvider.UtcNow,
                Commits = commits,
                IpAddress = args.IpAddress,
                TotalCommit = totalCommitCount
            });
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
        private DateTime FormatDatetime(DateTime dateTime)
        {
            var formattedDate = dateTime.ToString("dd/MM/yyyy");
            if (DateTime.TryParse(formattedDate, out DateTime parsedDate))
            {
                return parsedDate;
            }
            return dateTime;
        }
        private async Task CreateQueryAsync(GetCommitsArgs args, CancellationToken cancellationToken)
        {
            string sampleInput = "";
            string firstChar;
            foreach (var character in sampleInput.ToArray())
            {
                firstChar = character.ToString();//a
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(firstChar);
                foreach (var character2 in sampleInput.Skip(1).ToArray())
                {
                    stringBuilder.Append(character2);
                    foreach (var character3 in sampleInput.ToArray())
                    {
                        stringBuilder.Append(character3);
                        Console.WriteLine(stringBuilder.ToString());
                    }
                }
            }
            if (args.StartDate.HasValue && args.EndDate.HasValue)
            {
                args.StartDate = FormatDatetime(args.StartDate.Value);
                args.EndDate = FormatDatetime(args.EndDate.Value);
            }
            var queryResult = await _applicationDbContext.Queries.AsNoTracking().Where(a => a.UserName == args.UserName).ToListAsync(cancellationToken);
            var hasInterrogatedBefore = queryResult.Any(a => a.StartDate == args.StartDate || a.EndDate == args.EndDate);
            if (!hasInterrogatedBefore && args.StartDate.HasValue && args.EndDate.HasValue)
            {
                await _applicationDbContext.Queries.AddAsync(new Query()
                {
                    UserName = args.UserName,
                    StartDate = args.StartDate,
                    EndDate = args.EndDate,
                }, cancellationToken);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
            }
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
