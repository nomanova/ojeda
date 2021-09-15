using FluentValidation;
using NomaNova.Ojeda.Core.Domain.AssetClasses;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Assets;

namespace NomaNova.Ojeda.Services.Assets
{
    public class AssetDtoBusinessValidator : AbstractValidator<AssetDto>
    {
        public AssetDtoBusinessValidator(IRepository<AssetClass> assetClassesRepository)
        {
            RuleFor(_ => _.AssetClass.Id).MustAsync(async (id, cancellation) =>
            {
                var assetClass = await assetClassesRepository.GetByIdAsync(id, cancellation);
                return assetClass != null;
            }).WithMessage("Asset class does not exist.");
        }
    }
}