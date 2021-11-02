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
        private readonly bool _isRequired;
        
        public FieldPropertiesResolver(
            IMapper mapper, 
            FieldProperties fieldProperties,
            bool isRequired)
        {
            _mapper = mapper;
            _fieldProperties = fieldProperties;
            _isRequired = isRequired;
        }

        public (FieldPropertiesDto properties, bool isRequired) Resolve(string fieldId)
        {
            var fieldPropertiesDto = _mapper.Map<FieldPropertiesDto>(_fieldProperties);
            return (fieldPropertiesDto, _isRequired);
        }
    }
}