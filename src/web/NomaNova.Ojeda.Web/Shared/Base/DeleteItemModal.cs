using System.Threading.Tasks;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Web.Shared.Extensions;

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
            Error = null;

            var result = await DeleteAsync();

            if (result.Success)
            {
                await ModalInstance.CloseAsync();
            }
            else
            {
                Error = result.Error.Stringify("Could not delete item.");
            }
            
            IsLoading = false;
        }

        protected abstract Task<OjedaResult> DeleteAsync();
    }
}