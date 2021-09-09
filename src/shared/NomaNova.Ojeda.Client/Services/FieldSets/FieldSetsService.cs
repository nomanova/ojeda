using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Client.Utils;
using NomaNova.Ojeda.Models;
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
            string orderBy, 
            bool orderAsc,
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken = default)
        {
            var qsb = QueryStringBuilder.New();
            
            qsb.Add("query" , query);
            qsb.Add("orderBy" , orderBy);
            qsb.Add("orderAsc" , orderAsc);
            qsb.Add("pageNumber" , pageNumber);
            qsb.Add("pageSize" , pageSize);

            var path = $"{BasePath}{qsb.Build()}";
            
            return await SendForDataAsync<PaginatedListDto<FieldSetDto>>(HttpMethod.Get, path, null, cancellationToken);
        }
        
        public async Task<OjedaDataResult<FieldSetDto>> CreateAsync(FieldSetDto fieldSet, CancellationToken cancellationToken)
        {
            Console.WriteLine(fieldSet);
            
            var path = $"{BasePath}";
            return await SendForDataAsync<FieldSetDto>(HttpMethod.Post, path, fieldSet, cancellationToken);
        }
        
        public async Task<OjedaDataResult<FieldSetDto>> UpdateAsync(string id, FieldSetDto fieldSet, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<FieldSetDto>(HttpMethod.Put, path, fieldSet, cancellationToken);
        }
        
        public async Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendAsync(HttpMethod.Delete, path, null, cancellationToken);
        }
    }
}