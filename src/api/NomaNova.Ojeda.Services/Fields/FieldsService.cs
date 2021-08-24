using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NomaNova.Ojeda.Api.Exceptions;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Fields;

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

        public async Task<FieldDto> GetFieldByIdAsync(string id, CancellationToken cancellationToken)
        {
            var field = await _fieldsRepository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<FieldDto>(field);
        }

        public async Task<PaginatedListDto<FieldDto>> GetFieldsAsync(
            string query, string orderBy, bool orderAsc, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = nameof(Field.Name);
            }

            var paginatedFields = 
                    await _fieldsRepository.GetAsync(query, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);

            var paginatedFieldsDto = _mapper.Map<PaginatedListDto<FieldDto>>(paginatedFields);
            paginatedFieldsDto.Items = paginatedFields.Select(f => _mapper.Map<FieldDto>(f)).ToList();

            return paginatedFieldsDto;
        }

        public async Task<FieldDto> CreateFieldAsync(FieldDto fieldDto, CancellationToken cancellationToken)
        {
            await Validate(new FieldDtoValidator(), fieldDto, cancellationToken);

            var field = _mapper.Map<Field>(fieldDto);
            field.Id = Guid.NewGuid().ToString();

            field = await _fieldsRepository.InsertAsync(field, cancellationToken);

            return _mapper.Map<FieldDto>(field);
        }

        public async Task<FieldDto> UpdateFieldAsync(string id, FieldDto fieldDto,
            CancellationToken cancellationToken)
        {
            await Validate(new FieldDtoValidator(), fieldDto, cancellationToken);
            
            var field = await _fieldsRepository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            field = _mapper.Map(fieldDto, field);
            field.Id = id;
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