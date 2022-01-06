using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.Features.Fields.Interfaces;

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
        
    Task<FieldDto> CreateAsync(CreateFieldDto fieldDto, CancellationToken cancellationToken = default);

    Task<FieldDto> UpdateAsync(string id, UpdateFieldDto fieldDto, CancellationToken cancellationToken = default);

    Task<DryRunDeleteFieldDto> DeleteAsync(string id, bool dryRun, CancellationToken cancellationToken = default);
}