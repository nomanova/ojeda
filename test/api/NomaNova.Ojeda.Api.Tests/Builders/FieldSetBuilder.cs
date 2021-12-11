using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Tests.Factories;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;

namespace NomaNova.Ojeda.Api.Tests.Builders
{
    public class FieldSetBuilder
    {
        private readonly FieldSet _fieldSet;
        private readonly ICollection<FieldSetField> _fieldSetFields;

        public FieldSetBuilder(string id = null)
        {
            _fieldSet = FieldSetFactory.NewRandom(id);
            _fieldSetFields = new List<FieldSetField>();
        }

        public FieldSetBuilder WithName(string name)
        {
            _fieldSet.Name = name;
            return this;
        }

        public FieldSetBuilder WithDescription(string description)
        {
            _fieldSet.Description = description;
            return this;
        }

        public FieldSetBuilder AddField(Field field, bool isRequired = false)
        {
            _fieldSetFields.Add(new FieldSetField
            {
                FieldId = field.Id,
                FieldSetId = _fieldSet.Id,
                IsRequired = isRequired,
                Order = (uint)_fieldSetFields.Count + 1
            });
            return this;
        }

        public FieldSetBuilder AddFields(params Field[] fields)
        {
            foreach (var field in fields)
            {
                AddField(field);
            }

            return this;
        }

        public async Task<FieldSet> Build(DbContext context)
        {
            await DatabaseHelper.Add(context, _fieldSet);

            foreach (var fieldSetField in _fieldSetFields)
            {
                await DatabaseHelper.Add(context, fieldSetField);
            }

            return _fieldSet;
        }
    }
}