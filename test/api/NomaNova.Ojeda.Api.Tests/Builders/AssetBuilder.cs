using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Tests.Factories;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;

namespace NomaNova.Ojeda.Api.Tests.Builders
{
    public class AssetBuilder
    {
        private readonly Asset _asset;
        private readonly ICollection<FieldValue> _fieldValues;

        public AssetBuilder(string assetTypeId, string id = null)
        {
            _asset = AssetFactory.NewRandom(assetTypeId, id);
            _fieldValues = new List<FieldValue>();
        }

        public AssetBuilder WithName(string name)
        {
            _asset.Name = name;
            return this;
        }

        public AssetBuilder AddFieldValue(Field field, FieldSet fieldSet, string value)
        {
            _fieldValues.Add(new FieldValue
            {
                Id = Guid.NewGuid().ToString(),
                AssetId = _asset.Id,
                FieldId = field.Id,
                FieldSetId = fieldSet.Id,
                Value = value
            });
            return this;
        }

        public async Task<Asset> Build(DbContext context)
        {
            await DatabaseHelper.Add(context, _asset);

            foreach (var fieldValue in _fieldValues)
            {
                await DatabaseHelper.Add(context, fieldValue);
            }

            return _asset;
        }
    }
}