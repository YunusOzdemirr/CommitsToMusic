﻿using AngleSharp;
using GithubCommitsToMusic.Args;
using GithubCommitsToMusic.Dtos;
using GithubCommitsToMusic.Exceptions;
using GithubCommitsToMusic.Extensions;
using GithubCommitsToMusic.Interfaces;
using GithubCommitsToMusic.Models;
using Microsoft.EntityFrameworkCore;

namespace GithubCommitsToMusic.Services
{
    public class CommitService : ICommitService
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public CommitService(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        string[] patterns = new string[] { "nd.", "rd.", "st.", "th." };
        private const string NoContributions = "No contributions";
        private const string CommitHtmlClass = "sr-only position-absolute";
        public async Task<List<CommitDto>> GetAllCommitsAsync(GetCommitsArgs args, CancellationToken cancellationToken = default)
        {
            ValidateArgs(args);
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
            await CreateUserAsync(args, commits, cancellationToken);
            return commits.GetCommitsDto();
        }
        private async Task CreateUserAsync(GetCommitsArgs args, IList<Commit> commits, CancellationToken cancellationToken)
        {
            var userExist = _applicationDbContext.Users.AsNoTracking().Any(a => a.UserName == args.UserName);
            if (userExist) return;
         
            _applicationDbContext.Users.Add(new User
            {
                UserName = args.UserName,
                CreatedOn = DateTime.Now,
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
