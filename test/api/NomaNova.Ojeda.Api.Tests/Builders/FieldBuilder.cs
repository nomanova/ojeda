using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Tests.Factories;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.Fields;

namespace NomaNova.Ojeda.Api.Tests.Builders
{
    public class FieldBuilder
    {
        private readonly Field _field;
        
        public FieldBuilder(string id = null)
        {
            _field = FieldFactory.NewRandom(id);
        }
        
        public FieldBuilder WithName(string name)
        {
            _field.Name = name;
            return this;
        }

        public FieldBuilder WithDescription(string description)
        {
            _field.Description = description;
            return this;
        }

        public FieldBuilder WithProperties(FieldProperties properties)
        {
            _field.Properties = properties;
            return this;
        }
        
        public async Task<Field> Build(DbContext context)
        {
            await DatabaseHelper.Add(context, _field);
            return _field;
        }
    }
}