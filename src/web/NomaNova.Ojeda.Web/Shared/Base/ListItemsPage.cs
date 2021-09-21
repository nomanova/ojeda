using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Web.Shared.Base
{
    public abstract class ListItemsPage<T> : ComponentBase where T : IIdentityDto
    {
        protected bool IsLoading;
        
        protected string SearchQuery = "";
        protected ICollection<T> Items;

        protected int PageCount;
        protected int CurrentPage;
        protected int PageSize;
        protected int TotalCount;

        [Inject] 
        private ILocalStorageService LocalStorageService { get; set; }

        [Inject] 
        private NavigationManager NavManager { get; set; }

        [Inject] 
        private IToastService ToastService { get; set; }
        
        protected abstract string Path { get; }

        protected override async Task OnInitializedAsync()
        {
            var pageSize = await GetPageSize();
            await GetItemsAsync(null, 1, pageSize);
        }
        
        protected async Task OnSearchAsync(string query)
        {
            if (query.Equals(SearchQuery))
            {
                return;
            }

            SearchQuery = query;
            await GetItemsAsync(query, 1, PageSize);
        }
        
        protected void OnAdd()
        {
            NavManager.NavigateTo($"{Path}/add");
        }

        protected void OnEdit(T item)
        {
            NavManager.NavigateTo($"{Path}/edit/{item.Id}");
        }
        
        protected async Task OnDelete(T item)
        {
            IsLoading = true;
            StateHasChanged();

            var result = await DeleteItemAsync(item);

            if (result.Success)
            {
                var toRemove = Items.FirstOrDefault(_ => _.Id.Equals(item.Id));
                if (toRemove != null)
                {
                    Items.Remove(toRemove);
                    TotalCount--;
                }
            }
            else
            {
                ToastService.ShowError($"Could not delete item. {result.Error?.Message} ({result.Error?.Code}).");
            }

            if (!Items.Any() && CurrentPage > 1)
            {
                await GetItemsAsync(SearchQuery, CurrentPage - 1, PageSize);
            }
            else
            {
                IsLoading = false;
                StateHasChanged();   
            }
        }
        
        protected async Task OnPageChanged(int pageNumber)
        {
            await GetItemsAsync(SearchQuery, pageNumber, PageSize);
        }

        protected async Task OnPageSizeChanged(int pageSize)
        {
            await StorePageSize(pageSize);
            await GetItemsAsync(SearchQuery, 1, pageSize);
        }
        
        private async Task GetItemsAsync(
            string query = null, 
            int pageNumber = Constants.DefaultPageNumber, 
            int pageSize = Constants.DefaultPageSize)
        {
            IsLoading = true;
            StateHasChanged();
        
            var result = await FetchItemsAsync(query, pageNumber, pageSize);

            if (result.Success)
            {
                var paginatedData = result.Data;
        
                Items = paginatedData.Items;
                PageCount = paginatedData.TotalPages;
                CurrentPage = paginatedData.PageNumber;
                PageSize = pageSize;
                TotalCount = paginatedData.TotalCount;
            }
            else
            {
                Items = new List<T>();
                ToastService.ShowError($"Could not load items. {result.Error?.Message} ({result.Error?.Code}).");
            }
        
            IsLoading = false;
            StateHasChanged();
        }

        private async Task<int> GetPageSize()
        {
            var storedPageSize = await LocalStorageService.GetItemAsync<int>(Constants.StorageKeyPageSize);
            return storedPageSize == 0 ? Constants.DefaultPageSize : storedPageSize;
        }

        private async Task StorePageSize(int pageSize)
        {
            await LocalStorageService.SetItemAsync(Constants.StorageKeyPageSize, pageSize);
        }

        protected abstract Task<OjedaResult> DeleteItemAsync(T item);

        protected abstract Task<OjedaDataResult<PaginatedListDto<T>>> FetchItemsAsync(
            string query = null, 
            int pageNumber = Constants.DefaultPageNumber,
            int pageSize = Constants.DefaultPageSize);
    }
}