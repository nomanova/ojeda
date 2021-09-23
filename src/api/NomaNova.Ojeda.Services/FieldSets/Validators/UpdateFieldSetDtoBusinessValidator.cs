using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Services.FieldSets.Validators
{
    public class UpdateFieldSetDtoBusinessValidator : CompositeValidator<UpdateFieldSetDto>
    {
        public UpdateFieldSetDtoBusinessValidator(
            IRepository<Field> fieldsRepository, 
            IRepository<FieldSet> fieldSetsRepository,
            string id)
        {
            Include(new UpdateFieldSetDtoFieldValidator());
            RegisterBaseValidator(new UniqueNameBusinessValidator<FieldSet>(fieldSetsRepository, id));

            RuleForEach(_ => _.Fields)
                .SetValidator(new UpdateFieldSetFieldDtoBusinessValidator(fieldsRepository));
        }
        
        private class UpdateFieldSetFieldDtoBusinessValidator : CompositeValidator<UpdateFieldSetFieldDto>
        {
            public UpdateFieldSetFieldDtoBusinessValidator(IRepository<Field> fieldsRepository)
            {
                RegisterBaseValidator(new ExistsBusinessValidator<Field>(fieldsRepository));
            }
        }
    }
}