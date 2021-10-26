using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NomaNova.Ojeda.Models.Dtos.Assets;

namespace NomaNova.Ojeda.Web.Features.App.Assets.Validation
{
    public class AssetFluentValidationValidator : ComponentBase
    {
        [CascadingParameter] 
        private EditContext CurrentEditContext { get; set; }

        [Parameter]
        public AssetDto Asset { get; set; }

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new ArgumentNullException(nameof(CurrentEditContext));
            }

            if (Asset == null)
            {
                throw new ArgumentNullException(nameof(Asset));
            }
            
            CurrentEditContext.AddFluentValidation(Asset);
        }
    }
}