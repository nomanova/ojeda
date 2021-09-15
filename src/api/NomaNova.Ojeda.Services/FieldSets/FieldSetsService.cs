using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Exceptions;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Core.Extensions;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.FieldSets;

namespace NomaNova.Ojeda.Services.FieldSets
{
    public class FieldSetsService : BaseService, IFieldSetsService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Field> _fieldsRepository; 
        private readonly IRepository<FieldSet> _fieldSetsRepository;
        
        public FieldSetsService(
            IMapper mapper,
            IRepository<Field> fieldsRepository,
            IRepository<FieldSet> fieldSetsRepository)
        {
            _mapper = mapper;
            _fieldsRepository = fieldsRepository;
            _fieldSetsRepository = fieldSetsRepository;
        }
        
        public async Task<FieldSetDto> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var fieldSet = await _fieldSetsRepository.GetByIdAsync(id, query =>
            {
                return query
                    .Include(s => s.FieldSetFields)
                    .ThenInclude(f => f.Field);
            }, cancellationToken);

            if (fieldSet == null)
            {
                throw new NotFoundException();
            }

            var fieldSetDto = _mapper.Map<FieldSetDto>(fieldSet);
            
            fieldSetDto.Fields = fieldSetDto.Fields
                .OrderBy(_ => _.Order)
                .ThenBy(_ => _.Field.Name)
                .ToList();

            return fieldSetDto;
        }

        public async Task<PaginatedListDto<FieldSetDto>> GetAsync(
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
                orderBy = nameof(FieldSet.Name);
            }
            
            var paginatedFieldSets = await _fieldSetsRepository.GetAllPaginatedAsync(
                searchQuery, query =>
                {
                    if (excludedIds.HasElements())
                    {
                        query = query.Where(e => !excludedIds.Contains(e.Id));
                    }
                    
                    return query
                        .Include(s => s.FieldSetFields)
                        .ThenInclude(f => f.Field);
                }, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);
            
            var paginatedFieldSetsDto = _mapper.Map<PaginatedListDto<FieldSetDto>>(paginatedFieldSets);
            paginatedFieldSetsDto.Items = paginatedFieldSets.Select(f => _mapper.Map<FieldSetDto>(f)).ToList();
            
            return paginatedFieldSetsDto;
        }

        public async Task<FieldSetDto> CreateAsync(
            FieldSetDto fieldSetDto, CancellationToken cancellationToken)
        {
            await Validate(null, fieldSetDto, cancellationToken);

            var fieldSetId = Guid.NewGuid().ToString();

            var fieldSet = _mapper.Map<FieldSet>(fieldSetDto);
            fieldSet.Id = fieldSetId;
            
            fieldSet.FieldSetFields = fieldSetDto.Fields.Select(f => _mapper.Map<FieldSetField>(f, opt =>
                opt.AfterMap((_, dest) =>
                {
                    dest.FieldSetId = fieldSetId;
                }))).ToList();

            fieldSet = await _fieldSetsRepository.InsertAsync(fieldSet, cancellationToken);
            
            return _mapper.Map<FieldSetDto>(fieldSet);
        }

        public async Task<FieldSetDto> UpdateAsync(
            string id, FieldSetDto fieldSetDto, CancellationToken cancellationToken)
        {
            var fieldSet = await _fieldSetsRepository.GetByIdAsync(id, query =>
            {
                return query.Include(s => s.FieldSetFields);
            }, cancellationToken);

            if (fieldSet == null)
            {
                throw new NotFoundException();
            }

            await Validate(id, fieldSetDto, cancellationToken);

            fieldSet = _mapper.Map(fieldSetDto, fieldSet);
            fieldSet.Id = id;

            fieldSet.FieldSetFields = fieldSetDto.Fields.Select(f => _mapper.Map<FieldSetField>(f, opt =>
                opt.AfterMap((_, dest) => dest.FieldSetId = id))).ToList();

            fieldSet = await _fieldSetsRepository.UpdateAsync(fieldSet, cancellationToken);

            return _mapper.Map<FieldSetDto>(fieldSet);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var fieldSet = await _fieldSetsRepository.GetByIdAsync(id, cancellationToken);

            if (fieldSet == null)
            {
                throw new NotFoundException();
            }
            
            await _fieldSetsRepository.DeleteAsync(fieldSet, cancellationToken);
        }
        
        private async Task Validate(string id, FieldSetDto fieldSetDto, CancellationToken cancellationToken)
        {
            fieldSetDto.Id = id;
            await Validate(new FieldSetDtoBusinessValidator(_fieldsRepository, _fieldSetsRepository), fieldSetDto, cancellationToken);
        }
    }
}