using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Models.Shared.Interfaces;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets.Base
{
    public abstract class UpsertFieldSetDto<T> : INamedDto where T : UpsertFieldSetFieldDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public List<T> Fields { get; set; }
    }

    public abstract class UpsertFieldSetFieldDto : IIdentityDto
    {
        public string Id { get; set; }
        
        public int Order { get; set; }
    }

    public class UpsertFieldSetDtoFieldValidator<T> :
        CompositeValidator<UpsertFieldSetDto<T>> where T : UpsertFieldSetFieldDto
    {
        public UpsertFieldSetDtoFieldValidator()
        {
            RegisterBaseValidator(new NamedFieldValidator());
            
            RuleFor(_ => _.Fields).NotEmpty().WithMessage("At least one field is required.");

            RuleFor(_ => _.Fields).Must(fields =>
            {
                if (fields == null)
                {
                    return true;
                }

                var fieldIds = fields.Select(f => f.Id).ToList();
                return fieldIds.Count == fieldIds.Distinct().Count();
            }).WithMessage("A field must not be added more than once.");
            
            RuleForEach(_ => _.Fields).SetValidator(new UpsertFieldSetFieldDtoFieldValidator());
        }
    }

    public class UpsertFieldSetFieldDtoFieldValidator : AbstractValidator<UpsertFieldSetFieldDto>
    {
        public UpsertFieldSetFieldDtoFieldValidator()
        {
            RuleFor(_ => _.Order).GreaterThanOrEqualTo(0);
            RuleFor(_ => _.Id).NotEmpty();
        }
    }
}