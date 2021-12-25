using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Core.Exceptions;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.FieldSets.Interfaces;
using NomaNova.Ojeda.Services.FieldSets.Validators;
using NomaNova.Ojeda.Utils.Extensions;

namespace NomaNova.Ojeda.Services.FieldSets
{
    public class FieldSetsService : BaseEntityService<FieldSet>, IFieldSetsService
    {
        private readonly IRepository<Field> _fieldsRepository;
        private readonly IRepository<AssetType> _assetTypeRepository;
        private readonly IRepository<Asset> _assetsRepository;
        private readonly IRepository<FieldValue> _fieldValueRepository;

        public FieldSetsService(
            IMapper mapper,
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<Field> fieldsRepository,
            IRepository<AssetType> assetTypeRepository,
            IRepository<Asset> assetsRepository,
            IRepository<FieldValue> fieldValueRepository) : base(mapper, fieldSetsRepository)
        {
            _fieldsRepository = fieldsRepository;
            _assetTypeRepository = assetTypeRepository;
            _assetsRepository = assetsRepository;
            _fieldValueRepository = fieldValueRepository;
        }

        public async Task<FieldSetDto> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var fieldSet = await Repository.GetByIdAsync(id, query =>
            {
                return query
                    .Include(s => s.FieldSetFields)
                    .ThenInclude(f => f.Field);
            }, cancellationToken);

            if (fieldSet == null)
            {
                throw new NotFoundException();
            }

            var fieldSetDto = Mapper.Map<FieldSetDto>(fieldSet);
            EnsureNormalizedFieldOrder(fieldSetDto);

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

            var paginatedFieldSets = await Repository.GetAllPaginatedAsync(
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

            var paginatedFieldSetsDto = Mapper.Map<PaginatedListDto<FieldSetDto>>(paginatedFieldSets);
            paginatedFieldSetsDto.Items = paginatedFieldSets.Select(f =>
            {
                var fieldSetDto = Mapper.Map<FieldSetDto>(f);
                EnsureNormalizedFieldOrder(fieldSetDto);
                return fieldSetDto;
            }).ToList();

            return paginatedFieldSetsDto;
        }

        public async Task<FieldSetDto> CreateAsync(
            CreateFieldSetDto fieldSetDto, CancellationToken cancellationToken)
        {
            await ValidateAndThrowAsync(new CreateFieldSetDtoBusinessValidator(_fieldsRepository, Repository),
                fieldSetDto, cancellationToken);

            var fieldSetId = Guid.NewGuid().ToString();

            var fieldSet = Mapper.Map<FieldSet>(fieldSetDto);
            fieldSet.Id = fieldSetId;

            fieldSet.FieldSetFields = fieldSetDto.Fields.Select(f => Mapper.Map<FieldSetField>(f, opt =>
                opt.AfterMap((_, dest) => { dest.FieldSetId = fieldSetId; }))).ToList();

            fieldSet = await Repository.InsertAsync(fieldSet, cancellationToken);

            return Mapper.Map<FieldSetDto>(fieldSet);
        }

        public async Task<FieldSetDto> UpdateAsync(
            string id, UpdateFieldSetDto fieldSetDto, CancellationToken cancellationToken)
        {
            var fieldSet = await PrepareUpdateAsync(id, fieldSetDto, cancellationToken);
            var removedFieldIds = GetRemovedFieldIds(fieldSet, fieldSetDto);

            fieldSet = Mapper.Map(fieldSetDto, fieldSet);
            fieldSet.Id = id;

            fieldSet.FieldSetFields = fieldSetDto.Fields.Select(f => Mapper.Map<FieldSetField>(f, opt =>
                opt.AfterMap((_, dest) => dest.FieldSetId = id))).ToList();

            fieldSet = await Repository.UpdateAsync(fieldSet, cancellationToken);

            // Delete any impacted field values
            if (removedFieldIds.HasElements())
            {
                var fieldValues = await _fieldValueRepository.GetAllAsync(query =>
                {
                    return query.Where(_ => _.FieldSetId == id && removedFieldIds.Contains(_.FieldId));
                }, cancellationToken);

                await _fieldValueRepository.DeleteRangeAsync(fieldValues, cancellationToken);
            }

            return Mapper.Map<FieldSetDto>(fieldSet);
        }

        public async Task<DryRunUpdateFieldSetDto> DryRunUpdateAsync(
            string id, UpdateFieldSetDto fieldSetDto, CancellationToken cancellationToken)
        {
            var fieldSet = await PrepareUpdateAsync(id, fieldSetDto, cancellationToken);

            // Determine removed fields
            var removedFieldIds = GetRemovedFieldIds(fieldSet, fieldSetDto);

            // Fetch impacted assets
            var assets = await _assetsRepository.GetAllAsync(query =>
            {
                return query.Where(_ =>
                    _.FieldValues.Any(fv => fv.FieldSetId == id &&
                                            removedFieldIds.Contains(fv.FieldId) &&
                                            fv.Value != null)
                );
            }, cancellationToken);

            // Map dto
            var updateFieldSetDto = new DryRunUpdateFieldSetDto
            {
                Assets = assets.Select(_ => Mapper.Map<NamedEntityDto>(_)).ToList()
            };

            return updateFieldSetDto;
        }

        public async Task<DryRunDeleteFieldSetDto> DeleteAsync(string id, bool dryRun,
            CancellationToken cancellationToken)
        {
            var fieldSet = await Repository.GetByIdAsync(id, cancellationToken);

            if (fieldSet == null)
            {
                throw new NotFoundException();
            }

            var deleteFieldSetDto = new DryRunDeleteFieldSetDto();

            // Fetch impacted asset types
            var assetTypes = await _assetTypeRepository.GetAllAsync(
                query => { return query.Where(_ => _.AssetTypeFieldSets.Select(atf => atf.FieldSetId).Contains(id)); },
                cancellationToken);

            deleteFieldSetDto.AssetTypes = assetTypes.Select(_ => Mapper.Map<NamedEntityDto>(_)).ToList();

            // Fetch impacted assets
            var assets = await _assetsRepository.GetAllAsync(
                query =>
                {
                    return query.Where(_ =>
                        _.FieldValues.Any(fv => fv.FieldSetId == id && fv.Value != null));
                },
                cancellationToken);

            deleteFieldSetDto.Assets = assets.Select(_ => Mapper.Map<NamedEntityDto>(_)).ToList();

            if (dryRun)
            {
                return deleteFieldSetDto;
            }

            await Repository.DeleteAsync(fieldSet, cancellationToken);
            return deleteFieldSetDto;
        }

        private async Task<FieldSet> PrepareUpdateAsync(
            string id, UpdateFieldSetDto fieldSetDto, CancellationToken cancellationToken)
        {
            var fieldSet = await Repository.GetByIdAsync(id,
                query => { return query.Include(s => s.FieldSetFields); }, cancellationToken);

            if (fieldSet == null)
            {
                throw new NotFoundException();
            }

            await ValidateAndThrowAsync(
                new UpdateFieldSetDtoBusinessValidator(_fieldsRepository, Repository, id),
                fieldSetDto, cancellationToken);

            return fieldSet;
        }

        private static List<string> GetRemovedFieldIds(FieldSet dbFieldSet, UpdateFieldSetDto dtoFieldSet)
        {
            var dbFieldIds = dbFieldSet.FieldSetFields.Select(_ => _.FieldId).ToList();
            var dtoFieldIds = dtoFieldSet.Fields.Select(_ => _.Id).ToList();

            return dbFieldIds.Where(db => dtoFieldIds.All(dto => dto != db)).ToList();
        }

        private static void EnsureNormalizedFieldOrder(FieldSetDto fieldSetDto)
        {
            fieldSetDto.Fields = fieldSetDto.Fields
                .OrderBy(_ => _.Order)
                .ThenBy(_ => _.Field.Name)
                .ToList();
            
            for (var i = 0; i < fieldSetDto.Fields.Count; i++)
            {
                fieldSetDto.Fields[i].Order = i + 1;
            }
        }
    }
}