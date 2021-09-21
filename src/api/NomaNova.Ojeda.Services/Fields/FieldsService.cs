using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NomaNova.Ojeda.Api.Exceptions;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.Fields
{
    public class FieldsService : BaseService, IFieldsService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Field> _fieldsRepository;
        
        public FieldsService(
            IMapper mapper,
            IRepository<Field> fieldsRepository)
        {
            _mapper = mapper;
            _fieldsRepository = fieldsRepository;
        }

        public async Task<FieldDto> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var field = await _fieldsRepository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<FieldDto>(field);
        }

        public async Task<PaginatedListDto<FieldDto>> GetAsync(
            string searchQuery, 
            string orderBy, 
            bool orderAsc, 
            IList<string> excludedIds, 
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = nameof(Field.Name);
            }

            var paginatedFields = await _fieldsRepository.GetAllPaginatedAsync(
                searchQuery, orderBy, orderAsc, excludedIds, pageNumber, pageSize, cancellationToken);

            var paginatedFieldsDto = _mapper.Map<PaginatedListDto<FieldDto>>(paginatedFields);
            paginatedFieldsDto.Items = paginatedFields.Select(f => _mapper.Map<FieldDto>(f)).ToList();

            return paginatedFieldsDto;
        }

        public async Task<FieldDto> CreateAsync(CreateFieldDto fieldDto, CancellationToken cancellationToken)
        {
            await Validate(null, fieldDto, cancellationToken);

            var field = _mapper.Map<Field>(fieldDto);
            field.Id = Guid.NewGuid().ToString();

            field = await _fieldsRepository.InsertAsync(field, cancellationToken);

            return _mapper.Map<FieldDto>(field);
        }

        public async Task<FieldDto> UpdateAsync(string id, UpdateFieldDto fieldDto,
            CancellationToken cancellationToken)
        {
            var field = await _fieldsRepository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            await Validate(id, fieldDto, cancellationToken);

            field = _mapper.Map(fieldDto, field);
            field.Id = id;
            
            field = await _fieldsRepository.UpdateAsync(field, cancellationToken);

            return _mapper.Map<FieldDto>(field);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var field = await _fieldsRepository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            await _fieldsRepository.DeleteAsync(field, cancellationToken);
        }

        private async Task Validate(string id, UpsertFieldDto fieldDto, CancellationToken cancellationToken)
        {
            await Validate(new FieldDtoBusinessValidator(_fieldsRepository, id), fieldDto, cancellationToken);
        }
    }
}