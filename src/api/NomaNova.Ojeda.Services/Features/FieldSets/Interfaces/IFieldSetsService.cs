using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.Features.FieldSets.Interfaces;

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
        
    Task<DryRunUpdateFieldSetDto> DryRunUpdateAsync(
        string id, UpdateFieldSetDto fieldSetDto, CancellationToken cancellationToken = default);
        
    Task<DryRunDeleteFieldSetDto> DeleteAsync(string id, bool dryRun, CancellationToken cancellationToken = default);
}