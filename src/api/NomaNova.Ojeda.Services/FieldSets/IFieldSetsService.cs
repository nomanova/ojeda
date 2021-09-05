using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.FieldSets;

namespace NomaNova.Ojeda.Services.FieldSets
{
    public interface IFieldSetsService
    {
        Task<FieldSetDto> GetFieldSetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<PaginatedListDto<FieldSetDto>> GetFieldSetsAsync(
            string searchQuery, string orderBy, bool orderAsc, int pageNumber, int pageSize,
            CancellationToken cancellationToken = default);
        
        Task<FieldSetDto> CreateFieldSetAsync(
            UpsertFieldSetDto upsertFieldSetDto, CancellationToken cancellationToken = default);

        Task<FieldSetDto> UpdateFieldSetAsync(
            string id, UpsertFieldSetDto upsertFieldSetDto, CancellationToken cancellationToken = default);
        
        Task DeleteFieldSetAsync(string id, CancellationToken cancellationToken = default);
    }
}