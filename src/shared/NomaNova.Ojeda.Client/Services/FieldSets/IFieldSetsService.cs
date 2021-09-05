using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.FieldSets;

namespace NomaNova.Ojeda.Client.Services.FieldSets
{
    public interface IFieldSetsService
    {
        Task<OjedaDataResult<FieldSetDto>> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<PaginatedListDto<FieldSetDto>>> GetAsync(
            string query,
            string orderBy, bool orderAsc,
            int pageNumber, int pageSize,
            CancellationToken cancellationToken = default);
    }
}