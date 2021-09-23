using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Dtos.AssetTypes.Base;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Web.Shared.Base;
using NomaNova.Ojeda.Web.Shared.Extensions;

namespace NomaNova.Ojeda.Web.Features.Admin.AssetTypes
{
    public abstract class AssetTypePage<T, TS> : FormPage<AssetTypeDto>
        where T : UpsertAssetTypeDto<TS> where TS : UpsertAssetTypeFieldSetDto, new()
    {
        [CascadingParameter]
        public IModalService Modal { get; set; }
        
        protected override string ReturnPath => "/admin/asset-types";
        
        protected abstract T UpsertAssetType { get; set; }
        
        protected List<FieldSetSummaryDto> FieldSets { get; set; }
        
        protected async Task OnAddFieldSet()
        {
            if (IsSubmitting)
            {
                return;
            }
        
            var parameters = new ModalParameters();
        
            var excludedIds = UpsertAssetType.FieldSets.Select(_ => _.Id).ToList();
            parameters.Add("ExcludedIds", excludedIds);
        
            var selectFieldSetModal = Modal.Show<SelectFieldSetModal>("Add Field Set", parameters, Constants.DefaultModalOptions);
            var result = await selectFieldSetModal.Result;

            if (!result.Cancelled)
            {
                var fieldSet = (FieldSetDto) result.Data;
                
                FieldSets.Add(fieldSet);
                
                UpsertAssetType.FieldSets.Add(new TS
                {
                    Order = UpsertAssetType.FieldSets.Count == 0 ? 1 : UpsertAssetType.FieldSets.Max(f => f.Order) + 1,
                    Id = fieldSet.Id
                });
            
                EditContext.Validate();
                StateHasChanged();
            }
        }
        
        protected void OnMoveItemUp(TS item)
        {
            if (IsSubmitting)
            {
                return;
            }

            var index = UpsertAssetType.FieldSets.IndexOf(item);

            UpsertAssetType.FieldSets.RemoveAt(index);
            UpsertAssetType.FieldSets.Insert(index - 1, item);

            UpdateOrder();
        }
        
        protected void OnMoveItemDown(TS item)
        {
            if (IsSubmitting)
            {
                return;
            }

            var index = UpsertAssetType.FieldSets.IndexOf(item);

            UpsertAssetType.FieldSets.RemoveAt(index);
            UpsertAssetType.FieldSets.Insert(index + 1, item);

            UpdateOrder();
        }
        
        protected void OnRemoveItem(TS item)
        {
            if (IsSubmitting)
            {
                return;
            }

            UpsertAssetType.FieldSets.Remove(item);
            FieldSets.Remove(GetFieldSet(item.Id));

            UpdateOrder();
        }
        
        protected string GetFieldSetName(string id)
        {
            return GetFieldSet(id).Name.Truncate();
        }

        protected string GetFieldSetDescription(string id)
        {
            return GetFieldSet(id).Description.Truncate();
        }
        
        private FieldSetSummaryDto GetFieldSet(string id)
        {
            var fieldSet = FieldSets.FirstOrDefault(_ => _.Id.Equals(id));
            if (fieldSet == null)
            {
                throw new Exception($"Invalid state: field set {id} not available.");
            }
            return fieldSet;
        }
        
        private void UpdateOrder()
        {
            var order = 1;
            foreach (var item in UpsertAssetType.FieldSets)
            {
                item.Order = order;
                order++;
            }
        }
    }
}