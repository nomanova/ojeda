using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Fields;
using NomaNova.Ojeda.Models.FieldSets;

namespace NomaNova.Ojeda.Client.Services.FieldSets
{
    internal class FieldSetsService : BaseService, IFieldSetsService
    {
        private const string BasePath = "api/fieldsets";
        
        public FieldSetsService(OjedaHttpClient httpClient) : base(httpClient)
        {
        }
        
        public async Task<OjedaDataResult<FieldSetDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<FieldSetDto>(HttpMethod.Get, path, null, cancellationToken);
        }
        
        public async Task<OjedaDataResult<PaginatedListDto<FieldSetDto>>> GetAsync(
            string query,
            string orderBy, bool orderAsc,
            int pageNumber, int pageSize, 
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}?query={query}&orderBy={orderBy}&orderAsc={orderAsc}&pageNumber={pageNumber}&pageSize={pageSize}";
            return await SendForDataAsync<PaginatedListDto<FieldSetDto>>(HttpMethod.Get, path, null, cancellationToken);
        }
    }
}