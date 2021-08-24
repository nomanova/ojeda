using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Client.Services.Fields
{
    internal class FieldsService : BaseService, IFieldsService
    {
        private const string BasePath = "api/fields";
        
        public FieldsService(OjedaHttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<OjedaDataResult<FieldDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<FieldDto>(HttpMethod.Get, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<PaginatedListDto<FieldDto>>> GetAsync(
            string query = null,
            string orderBy = null, bool orderAsc = true,
            int pageNumber = 1, int pageSize = 20, 
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}?query={query}&orderBy={orderBy}&orderAsc={orderAsc}&pageNumber={pageNumber}&pageSize={pageSize}";
            return await SendForDataAsync<PaginatedListDto<FieldDto>>(HttpMethod.Get, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<FieldDto>> CreateAsync(FieldDto field, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}";
            return await SendForDataAsync<FieldDto>(HttpMethod.Post, path, field, cancellationToken);
        }
        
        public async Task<OjedaDataResult<FieldDto>> UpdateAsync(string id, FieldDto field, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<FieldDto>(HttpMethod.Put, path, field, cancellationToken);
        }

        public async Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendAsync(HttpMethod.Delete, path, null, cancellationToken);
        }
    }
}