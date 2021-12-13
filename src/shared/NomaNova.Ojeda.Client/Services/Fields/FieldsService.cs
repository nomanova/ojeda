using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Client.Utils;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Models.Shared.Converters;

namespace NomaNova.Ojeda.Client.Services.Fields
{
    internal class FieldsService : BaseService, IFieldsService
    {
        private const string BasePath = "api/fields";

        public FieldsService(OjedaHttpClient httpClient) : base(httpClient)
        {
            JonConverters.Add(new FieldPropertiesDtoJsonConverter());
        }

        public async Task<OjedaDataResult<FieldDto>> GetByIdAsync(string id,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<FieldDto>(HttpMethod.Get, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<PaginatedListDto<FieldDto>>> GetAsync(
            string query = null,
            string orderBy = null,
            bool orderAsc = true,
            IEnumerable<string> excludedIds = null,
            int pageNumber = Constants.DefaultPageNumber,
            int pageSize = Constants.DefaultPageSize,
            CancellationToken cancellationToken = default)
        {
            var qsb = QueryStringBuilder.New();

            qsb.Add("query", query);
            qsb.Add("orderBy", orderBy);
            qsb.Add("orderAsc", orderAsc);
            qsb.Add("excludedId", excludedIds);
            qsb.Add("pageNumber", pageNumber);
            qsb.Add("pageSize", pageSize);

            var path = $"{BasePath}{qsb.Build()}";

            return await SendForDataAsync<PaginatedListDto<FieldDto>>(HttpMethod.Get, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<FieldDto>> CreateAsync(CreateFieldDto field,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}";
            return await SendForDataAsync<FieldDto>(HttpMethod.Post, path, field, cancellationToken);
        }

        public async Task<OjedaDataResult<FieldDto>> UpdateAsync(string id, UpdateFieldDto field,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<FieldDto>(HttpMethod.Put, path, field, cancellationToken);
        }

        public async Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}";
            return await SendAsync(HttpMethod.Delete, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<DryRunDeleteFieldDto>> DryRunDeleteAsync(string id,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}/dry-run";
            return await SendForDataAsync<DryRunDeleteFieldDto>(HttpMethod.Delete, path, null, cancellationToken);
        }
    }
}