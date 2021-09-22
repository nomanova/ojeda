using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Dtos.FieldSets.Base;
using NomaNova.Ojeda.Web.Shared.Base;
using NomaNova.Ojeda.Web.Shared.Extensions;

namespace NomaNova.Ojeda.Web.Features.Admin.FieldSets
{
    public abstract class FieldSetPage<T, TS> : FormPage<FieldSetDto> 
        where T : UpsertFieldSetDto<TS> where TS : UpsertFieldSetFieldDto, new()
    {
        [CascadingParameter]
        public IModalService Modal { get; set; }
        
        protected override string ReturnPath => "/admin/field-sets";

        protected abstract T UpsertFieldSet { get; set; }

        protected List<FieldDto> Fields { get; set; }

        protected async Task OnAddField()
        {
            if (IsSubmitting)
            {
                return;
            }

            var parameters = new ModalParameters();

            var excludedIds = UpsertFieldSet.Fields.Select(_ => _.Id).ToList();
            parameters.Add("ExcludedIds", excludedIds);

            var selectFieldModal = Modal.Show<SelectFieldModal>("Add Field", parameters, Constants.DefaultModalOptions);
            var result = await selectFieldModal.Result;

            if (!result.Cancelled)
            {
                var field = (FieldDto) result.Data;

                Fields.Add(field);

                UpsertFieldSet.Fields.Add(new TS
                {
                    Order = UpsertFieldSet.Fields.Count == 0 ? 1 : UpsertFieldSet.Fields.Max(f => f.Order) + 1,
                    Id = field.Id
                });

                StateHasChanged();
            }
        }
        
        protected void OnMoveFieldUp(TS fieldSetFieldDto)
        {
            if (IsSubmitting)
            {
                return;
            }

            var index = UpsertFieldSet.Fields.IndexOf(fieldSetFieldDto);

            UpsertFieldSet.Fields.RemoveAt(index);
            UpsertFieldSet.Fields.Insert(index - 1, fieldSetFieldDto);

            UpdateOrder();
        }

        protected void OnMoveFieldDown(TS fieldSetFieldDto)
        {
            if (IsSubmitting)
            {
                return;
            }

            var index = UpsertFieldSet.Fields.IndexOf(fieldSetFieldDto);

            UpsertFieldSet.Fields.RemoveAt(index);
            UpsertFieldSet.Fields.Insert(index + 1, fieldSetFieldDto);

            UpdateOrder();
        }

        protected void OnRemoveField(TS fieldSetFieldDto)
        {
            if (IsSubmitting)
            {
                return;
            }

            UpsertFieldSet.Fields.Remove(fieldSetFieldDto);
            Fields.Remove(GetField(fieldSetFieldDto.Id));

            UpdateOrder();
        }
        
        protected string GetFieldName(string id)
        {
            return GetField(id).Name.Truncate();
        }

        protected string GetFieldDescription(string id)
        {
            return GetField(id).Description.Truncate();
        }

        protected string GetFieldType(string id)
        {
            return GetField(id).Type.ToString();
        }

        private FieldDto GetField(string id)
        {
            var field = Fields.FirstOrDefault(_ => _.Id.Equals(id));
            if (field == null)
            {
                throw new Exception($"Invalid state: field {id} not available.");
            }
            return field;
        }
        
        private void UpdateOrder()
        {
            var order = 1;
            foreach (var fieldSetField in UpsertFieldSet.Fields)
            {
                fieldSetField.Order = order;
                order++;
            }
        }
    }
}