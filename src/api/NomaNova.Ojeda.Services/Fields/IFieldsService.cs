using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Services.Fields
{
    public interface IFieldsService
    {
        Task<FieldDto> GetFieldByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<PaginatedListDto<FieldDto>> GetFieldsAsync(
            string searchQuery, string orderBy, bool orderAsc, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        
        Task<FieldDto> CreateFieldAsync(FieldDto fieldDto, CancellationToken cancellationToken = default);

        Task<FieldDto> UpdateFieldAsync(string id, FieldDto fieldDto, CancellationToken cancellationToken = default);

        Task DeleteFieldAsync(string id, CancellationToken cancellationToken = default);
    }
}