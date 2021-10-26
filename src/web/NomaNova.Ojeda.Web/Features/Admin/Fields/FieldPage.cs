using System;
using Microsoft.AspNetCore.Components;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Web.Shared.Base;

namespace NomaNova.Ojeda.Web.Features.Admin.Fields
{
    public abstract class FieldPage : FormPage<FieldDto>
    {
        protected void OnTypeChange(ChangeEventArgs args)
        {
            if (args.Value != null)
            {
                var type = Enum.Parse<FieldTypeDto>((string) args.Value);

                switch (type)
                {
                    case FieldTypeDto.Text:
                        SetFieldProperties(new TextFieldPropertiesDto());
                        break;
                    case FieldTypeDto.Number:
                        SetFieldProperties(new NumberFieldPropertiesDto());
                        break;
                    default:
                        throw new NotImplementedException(nameof(type));
                }
            }
        }

        protected abstract void SetFieldProperties(FieldPropertiesDto fieldProperties);
    }
}