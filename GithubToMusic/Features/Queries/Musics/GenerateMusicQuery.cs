using GithubCommitsToMusic.Dtos;
using GithubCommitsToMusic.Enums;
using GithubCommitsToMusic.Models;
using MediatR;

namespace GithubCommitsToMusic.Features.Queries.Musics
{
    public class GenerateMusicQuery : IRequest<Music>
    {
        public string UserName { get; set; }
        public IList<CommitDto> Commits { get; set; }
        public RhythmPatternType PatternType { get; set; }
    }
}
