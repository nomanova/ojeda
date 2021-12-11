using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Client.Services.FieldSets
{
    public interface IFieldSetsService
    {
        Task<OjedaDataResult<FieldSetDto>> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<PaginatedListDto<FieldSetDto>>> GetAsync(
            string query = null,
            string orderBy = null, 
            bool orderAsc = true,
            IEnumerable<string> excludedIds = null,
            int pageNumber = 1, 
            int pageSize = 10,
            CancellationToken cancellationToken = default);

        Task<OjedaDataResult<FieldSetDto>> CreateAsync(
            CreateFieldSetDto fieldSet,
            CancellationToken cancellationToken = default);

        Task<OjedaDataResult<FieldSetDto>> UpdateAsync(
            string id, 
            UpdateFieldSetDto fieldSet,
            CancellationToken cancellationToken = default);

        Task<OjedaDataResult<DryRunUpdateFieldSetDto>> DryRunUpdateAsync(
            string id,
            UpdateFieldSetDto fieldSet,
            CancellationToken cancellationToken = default);
        
        Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<DryRunDeleteFieldSetDto>> DryRunDeleteAsync(string id,
            CancellationToken cancellationToken = default);
    }
}