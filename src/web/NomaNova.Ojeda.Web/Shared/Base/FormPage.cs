using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NomaNova.Ojeda.Client;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Web.Shared.Validation;

namespace NomaNova.Ojeda.Web.Shared.Base
{
    public abstract class FormPage<TReturn> : ComponentBase
    {
        [Inject]
        protected NavigationManager NavManager { get; set; }
        
        [Inject] 
        protected IToastService ToastService { get; set; }

        [Inject] 
        protected OjedaClient Ojeda { get; set; }

        protected EditContext EditContext;
        protected ServerSideFluentValidation ServerValidation;

        protected bool IsSubmitting { get; set; }
        
        protected abstract string ReturnPath { get; }
        
        protected void OnReset()
        {
            if (IsSubmitting)
            {
                return;
            }

            Init();
        }
        
        protected void OnCancel()
        {
            if (IsSubmitting)
            {
                return;
            }
        
            NavManager.NavigateTo(ReturnPath);
        }

        protected async Task OnSubmitForm()
        {
            if (IsSubmitting)
            {
                return;
            }
        
            IsSubmitting = true;
            ServerValidation.ClearErrors();

            var result = await OnSubmitEntity();

            if(result.StatusCode == HttpStatusCode.BadRequest && result.Error.ValidationErrors.Any())
            {
                ServerValidation.DisplayErrors(result.Error.ValidationErrors);
            }
            else if (!result.Success)
            {
                ToastService.ShowError($"Could not submit item. {result.Error?.Message} ({result.Error?.Code}).");
            }
            else
            {
                NavManager.NavigateTo(ReturnPath);
            }

            IsSubmitting = false;
        }
        
        protected abstract Task<OjedaDataResult<TReturn>> OnSubmitEntity();
        
        protected abstract void Init();
    }
}