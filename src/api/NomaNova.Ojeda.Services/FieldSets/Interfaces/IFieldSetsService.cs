using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.FieldSets.Interfaces
{
    public interface IFieldSetsService
    {
        Task<FieldSetDto> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<PaginatedListDto<FieldSetDto>> GetAsync(
            string searchQuery, 
            string orderBy, 
            bool orderAsc,
            IList<string> excludedIds,
            int pageNumber, 
            int pageSize,
            CancellationToken cancellationToken = default);
        
        Task<FieldSetDto> CreateAsync(
            CreateFieldSetDto fieldSetDto, CancellationToken cancellationToken = default);

        Task<FieldSetDto> UpdateAsync(
            string id, UpdateFieldSetDto fieldSetDto, CancellationToken cancellationToken = default);
        
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}