using GithubCommitsToMusic.Models;
using MediatR;

namespace GithubCommitsToMusic.Features.Queries.Sheets
{
    public class GetSheetsQuery : IRequest<IList<Sheet>>
    {
    }
}
