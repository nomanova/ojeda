using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Client.Services.Fields
{
    public interface IFieldsService
    {
        Task<OjedaDataResult<FieldDto>> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<PaginatedListDto<FieldDto>>> GetAsync(
            string query = null,
            string orderBy = null,
            bool orderAsc = true,
            IEnumerable<string> excludedIds = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
        
        Task<OjedaDataResult<FieldDto>> CreateAsync(CreateFieldDto field,
            CancellationToken cancellationToken = default);

        Task<OjedaDataResult<FieldDto>> UpdateAsync(string id, UpdateFieldDto field,
            CancellationToken cancellationToken = default);

        Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<DryRunDeleteFieldDto>> DryRunDeleteAsync(
            string id, CancellationToken cancellationToken = default);
    }
}