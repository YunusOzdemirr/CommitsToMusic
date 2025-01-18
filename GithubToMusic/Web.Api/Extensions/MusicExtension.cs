using GithubCommitsToMusic.Dtos;
using GithubCommitsToMusic.Models;

namespace GithubCommitsToMusic.Extensions
{
    public static class MusicExtension
    {
        public static MusicDto GetMusicDto(this Music music)
        {
            return new MusicDto
            {
                Id = music.Id,
                Name = music.Name,
                VirtualPath = music.VirtualPath
            };
        }
    }

}
