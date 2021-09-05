using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Exceptions;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.FieldSets;

namespace NomaNova.Ojeda.Services.FieldSets
{
    public class FieldSetsService : BaseService, IFieldSetsService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<FieldSet> _fieldSetsRepository;
        
        public FieldSetsService(
            IMapper mapper,
            IRepository<FieldSet> fieldSetsRepository)
        {
            _mapper = mapper;
            _fieldSetsRepository = fieldSetsRepository;
        }
        
        public async Task<FieldSetDto> GetFieldSetByIdAsync(string id, CancellationToken cancellationToken)
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

            return _mapper.Map<FieldSetDto>(fieldSet);
        }

        public async Task<PaginatedListDto<FieldSetDto>> GetFieldSetsAsync(
            string searchQuery, string orderBy, bool orderAsc, int pageNumber, int pageSize,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = nameof(FieldSet.Name);
            }
            
            var paginatedFieldSets = await _fieldSetsRepository.GetAllPaginatedAsync(
                searchQuery, query =>
                {
                    return query
                        .Include(s => s.FieldSetFields)
                        .ThenInclude(f => f.Field);
                }, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);
            
            var paginatedFieldSetsDto = _mapper.Map<PaginatedListDto<FieldSetDto>>(paginatedFieldSets);
            paginatedFieldSetsDto.Items = paginatedFieldSets.Select(f => _mapper.Map<FieldSetDto>(f)).ToList();
            
            return paginatedFieldSetsDto;
        }

        public async Task<FieldSetDto> CreateFieldSetAsync(
            UpsertFieldSetDto upsertFieldSetDto, CancellationToken cancellationToken)
        {
            // TODO
            throw new NotImplementedException();
        }

        public async Task<FieldSetDto> UpdateFieldSetAsync(
            string id, UpsertFieldSetDto upsertFieldSetDto, CancellationToken cancellationToken)
        {
            // TODO
            throw new NotImplementedException();
        }

        public async Task DeleteFieldSetAsync(string id, CancellationToken cancellationToken)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}