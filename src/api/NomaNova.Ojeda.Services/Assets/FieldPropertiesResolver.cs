using AutoMapper;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Models.Dtos.Assets.Base;
using NomaNova.Ojeda.Models.Dtos.Fields;

namespace NomaNova.Ojeda.Services.Assets
{
    public class FieldPropertiesResolver : IFieldPropertiesResolver
    {
        private readonly IMapper _mapper;
        private readonly FieldProperties _fieldProperties;
        
        public FieldPropertiesResolver(
            IMapper mapper, 
            FieldProperties fieldProperties)
        {
            _mapper = mapper;
            _fieldProperties = fieldProperties;
        }

        public FieldPropertiesDto Resolve(string fieldId)
        {
            return _mapper.Map<FieldPropertiesDto>(_fieldProperties);
        }
    }
}