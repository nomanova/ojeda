using System.Linq;
using Microsoft.AspNetCore.Components.Forms;

namespace NomaNova.Ojeda.Web.Shared.Validation
{
    public class BootstrapCssClassProvider : FieldCssClassProvider
    {
        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            var propertyInfo = fieldIdentifier.Model.GetType().GetProperty(fieldIdentifier.FieldName);
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
            {
                return ""; // No validation classes for checkmarks
            }

            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            if (editContext.IsModified(fieldIdentifier))
            {
                return isValid ? "is-valid" : "is-invalid";
            }

            return isValid ? "" : "is-invalid";
        }
    }
}