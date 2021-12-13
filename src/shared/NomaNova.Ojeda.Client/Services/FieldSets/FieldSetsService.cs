using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Client.Utils;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Client.Services.FieldSets
{
    internal class FieldSetsService : BaseService, IFieldSetsService
    {
        private const string BasePath = "api/field-sets";

        public FieldSetsService(OjedaHttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<OjedaDataResult<FieldSetDto>> GetByIdAsync(string id,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<FieldSetDto>(HttpMethod.Get, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<PaginatedListDto<FieldSetDto>>> GetAsync(
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

            return await SendForDataAsync<PaginatedListDto<FieldSetDto>>(HttpMethod.Get, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<FieldSetDto>> CreateAsync(CreateFieldSetDto fieldSet,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}";
            return await SendForDataAsync<FieldSetDto>(HttpMethod.Post, path, fieldSet, cancellationToken);
        }

        public async Task<OjedaDataResult<FieldSetDto>> UpdateAsync(string id, UpdateFieldSetDto fieldSet,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<FieldSetDto>(HttpMethod.Put, path, fieldSet, cancellationToken);
        }


        public async Task<OjedaDataResult<DryRunUpdateFieldSetDto>> DryRunUpdateAsync(string id,
            UpdateFieldSetDto fieldSet, CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}/dry-run";
            return await SendForDataAsync<DryRunUpdateFieldSetDto>(HttpMethod.Put, path, fieldSet, cancellationToken);
        }

        public async Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}";
            return await SendAsync(HttpMethod.Delete, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<DryRunDeleteFieldSetDto>> DryRunDeleteAsync(string id,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}/dry-run";
            return await SendForDataAsync<DryRunDeleteFieldSetDto>(HttpMethod.Delete, path, null, cancellationToken);
        }
    }
}