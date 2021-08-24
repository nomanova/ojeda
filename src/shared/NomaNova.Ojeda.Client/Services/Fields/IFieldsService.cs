using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Client.Services.Fields
{
    public interface IFieldsService
    {
        Task<OjedaDataResult<FieldDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<PaginatedListDto<FieldDto>>> GetAsync(
            string query = null,
            string orderBy = null, bool orderAsc = true,
            int pageNumber = 1, int pageSize = 20, 
            CancellationToken cancellationToken = default);
        
        Task<OjedaDataResult<FieldDto>> CreateAsync(FieldDto field,
            CancellationToken cancellationToken = default);

        Task<OjedaDataResult<FieldDto>> UpdateAsync(string id, FieldDto field,
            CancellationToken cancellationToken = default);

        Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}