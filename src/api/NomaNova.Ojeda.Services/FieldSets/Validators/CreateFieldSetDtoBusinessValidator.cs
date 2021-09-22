using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Dtos.FieldSets.Validators;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Services.FieldSets.Validators
{
    public class CreateFieldSetDtoBusinessValidator : CompositeValidator<CreateFieldSetDto>
    {
        public CreateFieldSetDtoBusinessValidator(
            IRepository<Field> fieldsRepository, 
            IRepository<FieldSet> fieldSetsRepository)
        {
            Include(new CreateFieldSetDtoFieldValidator());
            RegisterBaseValidator(new UniqueNameBusinessValidator<FieldSet>(fieldSetsRepository));

            RuleForEach(_ => _.Fields)
                .SetValidator(new CreateFieldSetFieldDtoBusinessValidator(fieldsRepository));
        }
        
        private class CreateFieldSetFieldDtoBusinessValidator : CompositeValidator<CreateFieldSetFieldDto>
        {
            public CreateFieldSetFieldDtoBusinessValidator(IRepository<Field> fieldsRepository)
            {
                RegisterBaseValidator(new ExistsBusinessValidator<Field>(fieldsRepository));
            }
        }
    }
}