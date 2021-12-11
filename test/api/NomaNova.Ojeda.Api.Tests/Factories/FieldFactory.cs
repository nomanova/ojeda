using System;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Models.Dtos.Fields;

namespace NomaNova.Ojeda.Api.Tests.Factories
{
    public static class FieldFactory
    {
        public static Field NewRandom(string id = null)
        {
            return new Field
            {
                Id = id ?? Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = FactoryHelper.RandomString(),
                Description = FactoryHelper.RandomString(),
                Properties = new TextFieldProperties()
            };
        }

        public static CreateFieldDto NewRandomCreateDto()
        {
            return new CreateFieldDto
            {
                Name = FactoryHelper.RandomString(),
                Description = FactoryHelper.RandomString(),
                Properties = new TextFieldPropertiesDto()
            };
        }

        public static UpdateFieldDto NewRandomUpdateDto()
        {
            return new UpdateFieldDto
            {
                Name = FactoryHelper.RandomString(),
                Description = FactoryHelper.RandomString(),
                Properties = new TextFieldPropertiesDto()
            };
        }
    }
}