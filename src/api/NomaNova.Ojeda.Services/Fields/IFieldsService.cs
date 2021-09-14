using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Services.Fields
{
    public interface IFieldsService
    {
        Task<FieldDto> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<PaginatedListDto<FieldDto>> GetAsync(
            string searchQuery, 
            string orderBy, 
            bool orderAsc,
            IList<string> excludedIds,
            int pageNumber, 
            int pageSize,
            CancellationToken cancellationToken = default);
        
        Task<FieldDto> CreateAsync(FieldDto fieldDto, CancellationToken cancellationToken = default);

        Task<FieldDto> UpdateAsync(string id, FieldDto fieldDto, CancellationToken cancellationToken = default);

        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}