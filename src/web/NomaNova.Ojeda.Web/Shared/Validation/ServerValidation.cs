using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace NomaNova.Ojeda.Web.Shared.Validation
{
    public class ServerValidation : ComponentBase
    {
        private ValidationMessageStore _messageStore;

        [CascadingParameter] private EditContext CurrentEditContext { get; set; }

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(ServerValidation)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. " +
                    $"For example, you can use {nameof(ServerValidation)} " +
                    $"inside an {nameof(EditForm)}.");
            }

            _messageStore = new ValidationMessageStore(CurrentEditContext);

            CurrentEditContext.OnValidationRequested += (s, e) =>
                _messageStore.Clear();
            CurrentEditContext.OnFieldChanged += (s, e) =>
                _messageStore.Clear(e.FieldIdentifier);
        }
        
        public void DisplayErrors(Dictionary<string, List<string>> errors)
        {
            foreach (var (key, value) in errors)
            {
                _messageStore.Add(CurrentEditContext.Field(key), value);
            }
    
            CurrentEditContext.NotifyValidationStateChanged();
        }
        
        public void ClearErrors()
        {
            _messageStore.Clear();
            CurrentEditContext.NotifyValidationStateChanged();
        }
    }
}