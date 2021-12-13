using System.Threading.Tasks;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;

namespace NomaNova.Ojeda.Web.Shared.Base
{
    public abstract class ConfirmationModal<T> : ComponentBase
    {
        [CascadingParameter] 
        public BlazoredModalInstance ModalInstance { get; set; }

        [Parameter] 
        public T Item { get; set; }
        
        protected string Text;
        
        protected override void OnInitialized()
        {
            Text = GetText(Item);
        }

        protected async Task OnCancelAsync()
        {
            await ModalInstance.CancelAsync();
        }

        protected async Task OnConfirmAsync()
        {
            await ModalInstance.CloseAsync();
        }

        protected abstract string GetText(T item);
    }
}