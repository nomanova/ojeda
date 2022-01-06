using AutoMapper;
using NomaNova.Ojeda.Services.Features.AssetAttachments;
using NomaNova.Ojeda.Services.Features.AssetIdTypes;
using NomaNova.Ojeda.Services.Features.Assets;
using NomaNova.Ojeda.Services.Features.AssetTypes;
using NomaNova.Ojeda.Services.Features.Fields;
using NomaNova.Ojeda.Services.Features.FieldSets;
using Xunit;

namespace NomaNova.Ojeda.Services.Tests;

public class AutoMapperProfileTests
{
    [Fact]
    public void AutoMapperProfile_WhenCorrectConfiguration_ShouldNotThrowError()
    {
        var configuration = new MapperConfiguration(c =>
        {
            c.AddProfile<AssetIdTypeProfile>();
            c.AddProfile<FieldProfile>();
            c.AddProfile<FieldSetProfile>();
            c.AddProfile<AssetTypeProfile>();
            c.AddProfile<AssetProfile>();
            c.AddProfile<AssetAttachmentProfile>();
        });
        configuration.AssertConfigurationIsValid();
    }
}