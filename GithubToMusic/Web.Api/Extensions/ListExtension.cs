using GithubCommitsToMusic.Models;

namespace GithubCommitsToMusic.Extensions
{
    public static class ListExtension
    {
        public static bool AddIfNotNull(this List<Sheet> list, Sheet addItem)
        {
            if (list != null)
            {
                list.Add(addItem);
                return true;
            }
            return false;
        }
    }
}
