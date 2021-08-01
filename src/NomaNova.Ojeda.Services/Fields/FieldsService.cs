using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NomaNova.Ojeda.Api.Exceptions;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Helpers.Interfaces;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Services.Fields
{
    public class FieldsService : IFieldsService
    {
        private readonly ITimeKeeper _timeKeeper;
        private readonly IMapper _mapper;
        private readonly IRepository<Field> _fieldsRepository;
        
        public FieldsService(
            ITimeKeeper timeKeeper,
            IMapper mapper,
            IRepository<Field> fieldsRepository)
        {
            _timeKeeper = timeKeeper;
            _mapper = mapper;
            _fieldsRepository = fieldsRepository;
        }

        public async Task<FieldDto> GetFieldByIdAsync(string id, CancellationToken cancellationToken)
        {
            var field = await _fieldsRepository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<FieldDto>(field);
        }

        public async Task<FieldDto> CreateFieldAsync(CreateFieldDto createFieldDto, CancellationToken cancellationToken)
        {
            // TODO: validation
            
            var field = _mapper.Map<Field>(createFieldDto);
            field.Id = Guid.NewGuid().ToString();

            field = await _fieldsRepository.InsertAsync(field, cancellationToken);

            return _mapper.Map<FieldDto>(field);
        }

        public async Task<FieldDto> UpdateFieldAsync(string id, UpdateFieldDto updateFieldDto,
            CancellationToken cancellationToken)
        {
            // TODO: validation
            
            var field = await _fieldsRepository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            field = _mapper.Map(updateFieldDto, field);
            field = await _fieldsRepository.UpdateAsync(field, cancellationToken);

            return _mapper.Map<FieldDto>(field);
        }

        public async Task DeleteFieldAsync(string id, CancellationToken cancellationToken)
        {
            var field = await _fieldsRepository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            await _fieldsRepository.DeleteAsync(field, cancellationToken);
        }
    }
}