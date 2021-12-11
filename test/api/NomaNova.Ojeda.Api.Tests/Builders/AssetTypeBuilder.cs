using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Tests.Factories;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.FieldSets;

namespace NomaNova.Ojeda.Api.Tests.Builders
{
    public class AssetTypeBuilder
    {
        private readonly AssetType _assetType;
        private readonly ICollection<AssetTypeFieldSet> _assetTypeFieldSets;

        public AssetTypeBuilder(string id = null)
        {
            _assetType = AssetTypeFactory.NewRandom(id);
            _assetTypeFieldSets = new List<AssetTypeFieldSet>();
        }

        public AssetTypeBuilder WithName(string name)
        {
            _assetType.Name = name;
            return this;
        }

        public AssetTypeBuilder WithDescription(string description)
        {
            _assetType.Description = description;
            return this;
        }

        public AssetTypeBuilder AddFieldSet(FieldSet fieldSet)
        {
            _assetTypeFieldSets.Add(new AssetTypeFieldSet
            {
                FieldSetId = fieldSet.Id,
                AssetTypeId = _assetType.Id,
                Order = (uint)_assetTypeFieldSets.Count + 1
            });
            return this;
        }

        public AssetTypeBuilder AddFieldSets(params FieldSet[] fieldSets)
        {
            foreach (var fieldSet in fieldSets)
            {
                AddFieldSet(fieldSet);
            }

            return this;
        }
        
        public async Task<AssetType> Build(DbContext context)
        {
            await DatabaseHelper.Add(context, _assetType);

            foreach (var assetTypeFieldSet in _assetTypeFieldSets)
            {
                await DatabaseHelper.Add(context, assetTypeFieldSet);
            }

            return _assetType;
        }
    }
}