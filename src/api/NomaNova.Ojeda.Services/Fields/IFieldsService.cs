using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Services.Fields
{
    public interface IFieldsService
    {
        Task<FieldDto> GetFieldByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<FieldDto> CreateFieldAsync(
            CreateFieldDto createFieldDto, CancellationToken cancellationToken = default);

        Task<FieldDto> UpdateFieldAsync(string id, UpdateFieldDto updateFieldDto,
            CancellationToken cancellationToken);

        Task DeleteFieldAsync(string id, CancellationToken cancellationToken = default);
    }
}