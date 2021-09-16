using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models;

namespace NomaNova.Ojeda.Web.Shared.Base
{
    public abstract class SelectItemModal<T> : ComponentBase where T : IIdentityDto
    {
        [CascadingParameter] 
        public BlazoredModalInstance ModalInstance { get; set; }
        
        [Parameter] 
        public IList<string> ExcludedIds { get; set; }
        
        protected bool IsLoading;

        protected string Error;
    
        protected string SearchQuery = "";
        protected ICollection<T> Items;
    
        protected int PageCount;
        protected int CurrentPage;

        protected override async Task OnInitializedAsync()
        {
            await GetItemsAsync();
        }
        
        protected async Task OnSearchAsync(string query)
        {
            if (query.Equals(SearchQuery))
            {
                return;
            }

            SearchQuery = query;
            await GetItemsAsync(query);
        }
        
        protected async Task OnPageChanged(int pageNumber)
        {
            await GetItemsAsync(SearchQuery, pageNumber);
        }
        
        protected void OnCancel()
        {
            ModalInstance.CancelAsync();
        }

        protected void OnItemSelected(T item)
        {
            ModalInstance.CloseAsync(ModalResult.Ok(item));
        }

        private async Task GetItemsAsync(
            string query = null, 
            int pageNumber = Constants.DefaultPageNumber, 
            int pageSize = Constants.DefaultPageSize)
        {
            IsLoading = true;
            StateHasChanged();
        
            var result = await FetchItemsAsync(query, ExcludedIds, pageNumber, pageSize);
        
            if (result.Success)
            {
                Error = null;
            
                var paginatedData = result.Data;
            
                Items = paginatedData.Items;
                PageCount = paginatedData.TotalPages;
                CurrentPage = paginatedData.PageNumber;
            }
            else
            {
                Items = new List<T>();
                Error = $"Could not load items. {result.Error?.Message} ({result.Error?.Code}).";
            }

            IsLoading = false;
            StateHasChanged();
        }
        
        protected abstract Task<OjedaDataResult<PaginatedListDto<T>>> FetchItemsAsync(
            string query = null, 
            IList<string> excludedIds = null,
            int pageNumber = Constants.DefaultPageNumber,
            int pageSize = Constants.DefaultPageSize);
    }
}