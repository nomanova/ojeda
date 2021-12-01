using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Web.Shared.Extensions;

namespace NomaNova.Ojeda.Web.Shared.Base
{
    public abstract class DryRunDeleteItemModal<T> : DeleteItemModal
    {
        [Parameter] public string Name { get; set; }

        protected bool IsDisabled;
        protected string Text;

        protected override async Task OnInitializedAsync()
        {
            await Init();
        }

        private async Task Init()
        {
            Error = null;
            IsLoading = true;
            IsDisabled = true;

            var result = await DryRunDeleteAsync();

            if (result.Success)
            {
                IsDisabled = false;
                Text = GetText(result.Data);
            }
            else
            {
                IsDisabled = true;
                Error = result.Error.Stringify("Could not fetch delete status.");
            }

            IsLoading = false;
        }

        protected abstract Task<OjedaDataResult<T>> DryRunDeleteAsync();

        protected abstract string GetText(T data);
    }
}