using AutoMapper;
using NomaNova.Ojeda.Services.AssetIdTypes;
using NomaNova.Ojeda.Services.Assets;
using NomaNova.Ojeda.Services.AssetTypes;
using NomaNova.Ojeda.Services.Fields;
using NomaNova.Ojeda.Services.FieldSets;
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
        });
        configuration.AssertConfigurationIsValid();
    }
}