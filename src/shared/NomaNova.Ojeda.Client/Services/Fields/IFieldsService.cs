using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Client.Services.Fields
{
    public interface IFieldsService
    {
        Task<OjedaDataResult<FieldDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<FieldDto>> CreateAsync(CreateFieldDto field,
            CancellationToken cancellationToken = default);

        Task<OjedaDataResult<FieldDto>> UpdateAsync(UpdateFieldDto field,
            CancellationToken cancellationToken = default);

        Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}