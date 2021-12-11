using System;
using System.Linq;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Models.Dtos.FieldSets;

namespace NomaNova.Ojeda.Api.Tests.Factories
{
    public static class FieldSetFactory
    {
        public static FieldSet NewRandom(string id = null)
        {
            return new FieldSet
            {
                Id = id ?? Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = FactoryHelper.RandomString(),
                Description = FactoryHelper.RandomString()
            };
        }

        public static CreateFieldSetDto NewRandomCreateDto(params string[] fieldIds)
        {
            return new CreateFieldSetDto
            {
                Name = FactoryHelper.RandomString(),
                Description = FactoryHelper.RandomString(),
                Fields = fieldIds.Select(id => new CreateFieldSetFieldDto
                {
                    Id = id
                }) .ToList()
            };
        }
        
        public static UpdateFieldSetDto NewRandomUpdateDto(params string[] fieldIds)
        {
            return new UpdateFieldSetDto
            {
                Name = FactoryHelper.RandomString(),
                Description = FactoryHelper.RandomString(),
                Fields = fieldIds.Select(id => new UpdateFieldSetFieldDto
                {
                    Id = id
                }) .ToList()
            };
        }
    }
}