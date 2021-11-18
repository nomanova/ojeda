using System.Threading.Tasks;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using NomaNova.Ojeda.Client.Results;

namespace NomaNova.Ojeda.Web.Shared.Base
{
    public abstract class DeleteItemModal : ComponentBase
    {
        [CascadingParameter] 
        public BlazoredModalInstance ModalInstance { get; set; }
        
        [Parameter] 
        public string Id { get; set; }
        
        protected bool IsLoading;
        
        protected string Error;
        
        protected async Task OnCancelAsync()
        {
            await ModalInstance.CancelAsync();
        }

        protected async Task OnDeleteAsync()
        {
            IsLoading = true;
            StateHasChanged();

            var result = await DeleteAsync();

            if (result.Success)
            {
                Error = null;
                await ModalInstance.CloseAsync();
            }
            else
            {
                Error = $"Could not delete item. {result.Error?.Message} ({result.Error?.Code}).";
            }
            
            IsLoading = false;
            StateHasChanged();
        }

        protected abstract Task<OjedaResult> DeleteAsync();
    }
}