using System;
using System.Linq;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using NomaNova.Ojeda.Client;
using NomaNova.Ojeda.Models.Dtos.Assets;

namespace NomaNova.Ojeda.Web.Features.App.Assets
{
    public abstract class UpsertAssetPage : ComponentBase
    {
        [Inject] protected NavigationManager NavManager { get; set; }

        [Inject] protected IToastService ToastService { get; set; }

        [Inject] protected OjedaClient Ojeda { get; set; }

        protected AssetDto Asset;

        protected string GetFieldSetName(string id)
        {
            return GetFieldSet(id).Name;
        }

        protected string GetFieldSetDescription(string id)
        {
            return GetFieldSet(id).Description;
        }

        protected string GetFieldLabel(string fieldSetId, string fieldId)
        {
            var field = GetField(fieldSetId, fieldId);

            var name = field.Name;
            var isRequired = field.IsRequired;

            return isRequired ? $"{name}*" : name;
        }

        protected string GetFieldDescription(string fieldSetId, string fieldId)
        {
            return GetField(fieldSetId, fieldId).Description;
        }

        protected FieldDataTypeDto GetFieldDataType(string fieldSetId, string fieldId)
        {
            return GetField(fieldSetId, fieldId).Data.Type;
        }

        private AssetFieldSetDto GetFieldSet(string id)
        {
            var fieldSet = Asset.FieldSets.FirstOrDefault(_ => _.Id.Equals(id));

            if (fieldSet == null)
            {
                throw new Exception($"Invalid state: field set {id} not available.");
            }

            return fieldSet;
        }

        private AssetFieldDto GetField(string fieldSetId, string fieldId)
        {
            var fieldSet = GetFieldSet(fieldSetId);
            var field = fieldSet.Fields.FirstOrDefault(_ => _.Id.Equals(fieldId));

            if (field == null)
            {
                throw new Exception($"Invalid state: field {fieldId} not available.");
            }

            return field;
        }
    }
}