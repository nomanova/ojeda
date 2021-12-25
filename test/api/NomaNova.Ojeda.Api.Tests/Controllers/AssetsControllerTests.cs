using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using NomaNova.Ojeda.Api.Tests.Builders;
using NomaNova.Ojeda.Api.Tests.Controllers.Base;
using NomaNova.Ojeda.Api.Tests.Factories;
using NomaNova.Ojeda.Api.Tests.Fixtures;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Utils.Services.Interfaces;
using Xunit;

namespace NomaNova.Ojeda.Api.Tests.Controllers
{
    public class AssetsControllerTests : ApiTests
    {
        [Fact]
        public async Task GetById_WhenAssetExists_ShouldReturnOk()
        {
            // Arrange
            var assetType = await new AssetTypeBuilder()
                .Build(DatabaseContext);

            var asset = await new AssetBuilder(assetType.Id)
                .Build(DatabaseContext);
            
            var request = new RequestBuilder($"/api/assets/{asset.Id}")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetById_WhenAssetDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var assetId = Guid.NewGuid().ToString();

            var request = new RequestBuilder($"/api/assets/{assetId}")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetByAssetType_WhenAssetTypeExists_ShouldReturnOk()
        {
            // Arrange
            var assetIdType = await new AssetIdTypeBuilder()
                .Build(DatabaseContext);
            
            var assetType = await new AssetTypeBuilder()
                .WithAssetIdType(assetIdType.Id)
                .Build(DatabaseContext);
            
            var request = new RequestBuilder($"/api/assets/new?assetTypeId={assetType.Id}")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetByAssetType_WhenAssetTypeDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var assetTypeId = Guid.NewGuid().ToString();
            
            var request = new RequestBuilder($"/api/assets/new?assetTypeId={assetTypeId}")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_WhenAssetsExist_ShouldReturnOk()
        {
            // Arrange
            var assetType = await new AssetTypeBuilder()
                .Build(DatabaseContext);

            await new AssetBuilder(assetType.Id)
                .Build(DatabaseContext);
            
            await new AssetBuilder(assetType.Id)
                .Build(DatabaseContext);
            
            var request = new RequestBuilder("/api/assets")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var assetsDto = await GetPayloadAsync<PaginatedListDto<AssetDto>>(response);
            Assert.Equal(2, assetsDto.TotalCount);
        }

        [Fact]
        public async Task Get_WhenNoAssetsExist_ShouldReturnOkEmpty()
        {
            // Arrange
            var request = new RequestBuilder("/api/assets")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var assetsDto = await GetPayloadAsync<PaginatedListDto<AssetDto>>(response);
            Assert.Equal(0, assetsDto.TotalCount);
        }

        [Fact]
        public async Task Create_WhenValid_ShouldReturnCreated()
        {
            // Arrange
            var assetIdType = await new AssetIdTypeBuilder()
                .Build(DatabaseContext);
            
            var assetType = await new AssetTypeBuilder()
                .WithAssetIdType(assetIdType.Id)
                .Build(DatabaseContext);

            var createAssetDto = AssetFactory.NewRandomCreateDto(assetType.Id);
            
            var request = new RequestBuilder(HttpMethod.Post, "/api/assets")
                .WithPayload(GetService<ISerializer>(), createAssetDto)
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var location = response.Headers.Location;
            Assert.NotNull(location);
            
            var assetDto = await GetPayloadAsync<AssetDto>(response);
            Assert.NotNull(assetDto);
            
            var asset = await DatabaseHelper.Get<Asset>(DatabaseContext, assetDto.Id);
            Assert.NotNull(asset);
        }

        [Fact]
        public async Task Update_WhenValidAndExists_ShouldReturnOk()
        {
            // Arrange
            var assetIdType = await new AssetIdTypeBuilder()
                .Build(DatabaseContext);
            
            var assetType = await new AssetTypeBuilder()
                .WithAssetIdType(assetIdType.Id)
                .Build(DatabaseContext);

            var asset = await new AssetBuilder(assetType.Id)
                .Build(DatabaseContext);
            
            var updateAssetDto = AssetFactory.NewRandomUpdateDto(assetType.Id);

            var request = new RequestBuilder(HttpMethod.Put, $"/api/assets/{asset.Id}")
                .WithPayload(GetService<ISerializer>(), updateAssetDto)
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var updatedAsset = await DatabaseHelper.Get<Asset>(DatabaseContext, asset.Id);
            
            Assert.NotNull(updatedAsset);
            Assert.Equal(updatedAsset.Name, updateAssetDto.Name);
        }

        [Fact]
        public async Task Patch_WhenValidAndExists_ShouldReturnNoContent()
        {
            // Arrange
            var fixture = await DefaultAssetFixture.Create(DatabaseContext);

            var patchDto = new JsonPatchDocument<PatchAssetDto>();
            patchDto.Replace(_ => _.Name, "Updated Name");

            var request = new RequestBuilder(HttpMethod.Patch, $"/api/assets/{fixture.Asset1.Id}")
                .WithPayload(GetService<ISerializer>(), patchDto)
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
            var updatedAsset = await DatabaseHelper.Get<Asset>(DatabaseContext, fixture.Asset1.Id);
            
            Assert.NotNull(updatedAsset);
            Assert.Equal("Updated Name", updatedAsset.Name);
        }

        [Fact]
        public async Task Patch_WhenInvalidAndExists_ShouldReturnBadRequest()
        {
            // Arrange
            var fixture = await DefaultAssetFixture.Create(DatabaseContext);

            var patchDto = new JsonPatchDocument<PatchAssetDto>();
            patchDto.Replace(_ => _.Name, "");

            var request = new RequestBuilder(HttpMethod.Patch, $"/api/assets/{fixture.Asset1.Id}")
                .WithPayload(GetService<ISerializer>(), patchDto)
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_WhenAssetExists_ShouldReturnOk()
        {
            // Arrange
            var assetType = await new AssetTypeBuilder()
                .Build(DatabaseContext);

            var asset = await new AssetBuilder(assetType.Id)
                .Build(DatabaseContext);
            
            var request = new RequestBuilder(HttpMethod.Delete, $"/api/assets/{asset.Id}")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var deletedAsset = await DatabaseHelper.Get<Asset>(DatabaseContext, asset.Id);
            Assert.Null(deletedAsset);
        }

        [Fact]
        public async Task Delete_WhenAssetDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var assetId = Guid.NewGuid().ToString();

            var request = new RequestBuilder(HttpMethod.Delete, $"/api/assets/{assetId}")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}