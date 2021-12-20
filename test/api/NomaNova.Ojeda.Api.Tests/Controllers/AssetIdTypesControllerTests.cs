using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NomaNova.Ojeda.Api.Tests.Builders;
using NomaNova.Ojeda.Api.Tests.Controllers.Base;
using NomaNova.Ojeda.Api.Tests.Factories;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Utils.Services.Interfaces;
using Xunit;

namespace NomaNova.Ojeda.Api.Tests.Controllers;

public class AssetIdTypesControllerTests : ApiTests
{
    [Fact]
    public async Task GetById_WhenAssetIdTypeExists_ShouldReturnOk()
    {
        // Arrange
        var assetIdType = await new AssetIdTypeBuilder()
            .Build(DatabaseContext);

        var request = new RequestBuilder($"/api/asset-id-types/{assetIdType.Id}")
            .Build();

        // Act
        var response = await ApiClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WhenAssetIdTypeDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var assetIdTypeId = Guid.NewGuid().ToString();

        var request = new RequestBuilder($"/api/asset-id-types/{assetIdTypeId}")
            .Build();

        // Act
        var response = await ApiClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_WhenAssetIdTypesExist_ShouldReturnOk()
    {
        // Arrange
        await new AssetIdTypeBuilder()
            .Build(DatabaseContext);

        await new AssetIdTypeBuilder()
            .Build(DatabaseContext);

        var request = new RequestBuilder("/api/asset-id-types")
            .Build();

        // Act
        var response = await ApiClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var fieldsDto = await GetPayloadAsync<PaginatedListDto<AssetIdTypeDto>>(response);
        Assert.Equal(2, fieldsDto.TotalCount);
    }

    [Fact]
    public async Task Get_WhenNoAssetIdTypesExist_ShouldReturnOkEmpty()
    {
        // Arrange
        var request = new RequestBuilder("/api/asset-id-types")
            .Build();

        // Act
        var response = await ApiClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var fieldsDto = await GetPayloadAsync<PaginatedListDto<AssetIdTypeDto>>(response);
        Assert.Equal(0, fieldsDto.TotalCount);
    }

    [Fact]
    public async Task Create_WhenValid_ShouldReturnCreated()
    {
        // Arrange
        var createAssetIdTypeDto = AssetIdTypeFactory.NewRandomCreateDto();

        var request = new RequestBuilder(HttpMethod.Post, "/api/asset-id-types")
            .WithPayload(GetService<ISerializer>(), createAssetIdTypeDto)
            .Build();

        // Act
        var response = await ApiClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var location = response.Headers.Location;
        Assert.NotNull(location);

        var assetIdTypeDto = await GetPayloadAsync<AssetIdTypeDto>(response);
        Assert.NotNull(assetIdTypeDto);

        var assetIdType = await DatabaseHelper.Get<AssetIdType>(DatabaseContext, assetIdTypeDto.Id);
        Assert.NotNull(assetIdType);
    }

    [Fact]
    public async Task Update_WhenValidAndExists_ShouldReturnOk()
    {
        // Arrange
        var assetIdType = await new AssetIdTypeBuilder()
            .Build(DatabaseContext);

        var updateAssetIdTypeDto = AssetIdTypeFactory.NewRandomUpdateDto();

        var request = new RequestBuilder(HttpMethod.Put, $"/api/asset-id-types/{assetIdType.Id}")
            .WithPayload(GetService<ISerializer>(), updateAssetIdTypeDto)
            .Build();

        // Act
        var response = await ApiClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedAssetIdType = await DatabaseHelper.Get<AssetIdType>(DatabaseContext, assetIdType.Id);

        Assert.NotNull(updatedAssetIdType);
        Assert.Equal(updatedAssetIdType.Name, updateAssetIdTypeDto.Name);
        Assert.Equal(updatedAssetIdType.Description, updateAssetIdTypeDto.Description);
    }

    [Fact]
    public async Task Delete_WhenAssetIdTypeExists_ShouldReturnOk()
    {
        // Arrange
        var assetIdType = await new AssetIdTypeBuilder()
            .Build(DatabaseContext);

        var request = new RequestBuilder(HttpMethod.Delete, $"/api/asset-id-types/{assetIdType.Id}")
            .Build();

        // Act
        var response = await ApiClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var deletedAssetIdType = await DatabaseHelper.Get<AssetIdType>(DatabaseContext, assetIdType.Id);
        Assert.Null(deletedAssetIdType);
    }

    [Fact]
    public async Task Delete_WhenAssetDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var assetIdTypeId = Guid.NewGuid().ToString();

        var request = new RequestBuilder(HttpMethod.Delete, $"/api/asset-id-types/{assetIdTypeId}")
            .Build();

        // Act
        var response = await ApiClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}