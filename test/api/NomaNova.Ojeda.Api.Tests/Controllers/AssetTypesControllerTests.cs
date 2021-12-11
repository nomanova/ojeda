using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NomaNova.Ojeda.Api.Tests.Builders;
using NomaNova.Ojeda.Api.Tests.Controllers.Base;
using NomaNova.Ojeda.Api.Tests.Factories;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Utils.Services.Interfaces;
using Xunit;

namespace NomaNova.Ojeda.Api.Tests.Controllers
{
    public class AssetTypesControllerTests : ApiTests
    {
        [Fact]
        public async Task GetById_WhenAssetTypeExists_ShouldReturnOk()
        {
            // Arrange
            var assetType = await new AssetTypeBuilder()
                .Build(DatabaseContext);
            
            var request = new RequestBuilder($"/api/asset-types/{assetType.Id}")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetById_WhenAssetTypeDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var assetTypeId = Guid.NewGuid().ToString();

            var request = new RequestBuilder($"/api/asset-types/{assetTypeId}")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_WhenAssetTypesExist_ShouldReturnOk()
        {
            // Arrange
            await new AssetTypeBuilder()
                .Build(DatabaseContext);

            await new AssetTypeBuilder()
                .Build(DatabaseContext);

            var request = new RequestBuilder("/api/asset-types")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var assetTypesDto = await GetPayloadAsync<PaginatedListDto<AssetTypeDto>>(response);
            Assert.Equal(2, assetTypesDto.TotalCount);
        }

        [Fact]
        public async Task Get_WhenNoAssetTypesExist_ShouldReturnOkEmpty()
        {
            // Arrange
            var request = new RequestBuilder("/api/asset-types")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var assetTypesDto = await GetPayloadAsync<PaginatedListDto<AssetTypeDto>>(response);
            Assert.Equal(0, assetTypesDto.TotalCount);
        }

        [Fact]
        public async Task Create_WhenValid_ShouldReturnCreated()
        {
            // Arrange
            var fieldSet = await new FieldSetBuilder()
                .Build(DatabaseContext);
            
            var createAssetTypeDto = AssetTypeFactory.NewRandomCreateDto(fieldSet.Id);
            
            var request = new RequestBuilder(HttpMethod.Post, "/api/asset-types")
                .WithPayload(GetService<ISerializer>(), createAssetTypeDto)
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var location = response.Headers.Location;
            Assert.NotNull(location);
            
            var assetTypeDto = await GetPayloadAsync<AssetTypeDto>(response);
            Assert.NotNull(assetTypeDto);
            
            var assetType = await DatabaseHelper.Get<AssetType>(DatabaseContext, assetTypeDto.Id);
            Assert.NotNull(assetType);
        }

        [Fact]
        public async Task Update_WhenValidAndExists_ShouldReturnOk()
        {
            // Arrange
            var fieldSet = await new FieldSetBuilder()
                .Build(DatabaseContext);

            var assetType = await new AssetTypeBuilder()
                .AddFieldSet(fieldSet)
                .Build(DatabaseContext);

            var updateAssetTypeDto = AssetTypeFactory.NewRandomUpdateDto(fieldSet.Id);
            
            var request = new RequestBuilder(HttpMethod.Put, $"/api/asset-types/{assetType.Id}")
                .WithPayload(GetService<ISerializer>(), updateAssetTypeDto)
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var updatedAssetType = await DatabaseHelper.Get<AssetType>(DatabaseContext, assetType.Id);
            
            Assert.NotNull(updatedAssetType);
            Assert.Equal(updatedAssetType.Name, updateAssetTypeDto.Name);
            Assert.Equal(updatedAssetType.Description, updateAssetTypeDto.Description);
        }

        [Fact]
        public async Task DryRunUpdate_WhenNoFieldSetsRemoved_ShouldReturnOkEmpty()
        {
            // Arrange
            var field = await new FieldBuilder()
                .Build(DatabaseContext);
            
            var fieldSet = await new FieldSetBuilder()
                .AddField(field)
                .Build(DatabaseContext);
            
            var assetType = await new AssetTypeBuilder()
                .AddFieldSet(fieldSet)
                .Build(DatabaseContext);
            
            await new AssetBuilder(assetType.Id)
                .AddFieldValue(field, fieldSet, "123")
                .Build(DatabaseContext);
            
            var updateAssetTypeDto = AssetTypeFactory.NewRandomUpdateDto(fieldSet.Id);
            
            var request = new RequestBuilder(HttpMethod.Put, $"/api/asset-types/{assetType.Id}/dry-run")
                .WithPayload(GetService<ISerializer>(), updateAssetTypeDto)
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var dryRunUpdateAssetTypeDto = await GetPayloadAsync<DryRunUpdateAssetTypeDto>(response);
            Assert.Empty(dryRunUpdateAssetTypeDto.Assets);
        }

        [Fact]
        public async Task DryRunUpdate_WhenFieldSetsRemoved_ShouldReturnOkWithAsset()
        {
            // Arrange
            var field1 = await new FieldBuilder()
                .WithName("Field 1")
                .Build(DatabaseContext);
            
            var field2 = await new FieldBuilder()
                .WithName("Field 2")
                .Build(DatabaseContext);
            
            var field3 = await new FieldBuilder()
                .WithName("Field 3")
                .Build(DatabaseContext);
            
            var fieldSet1 = await new FieldSetBuilder()
                .WithName("Field Set 1")
                .AddFields(field1, field2)
                .Build(DatabaseContext);
            
            var fieldSet2 = await new FieldSetBuilder()
                .WithName("Field Set 2")
                .AddFields(field2, field3)
                .Build(DatabaseContext);
            
            var assetType = await new AssetTypeBuilder()
                .AddFieldSets(fieldSet1, fieldSet2)
                .Build(DatabaseContext);

            await new AssetBuilder(assetType.Id)
                .WithName("Asset 1")
                .AddFieldValue(field1, fieldSet1, "123")
                .Build(DatabaseContext);
            
            var asset2 = await new AssetBuilder(assetType.Id)
                .WithName("Asset 2")
                .AddFieldValue(field3, fieldSet2, "123")
                .Build(DatabaseContext);
            
            await new AssetBuilder(assetType.Id)
                .WithName("Asset 3")
                .AddFieldValue(field3, fieldSet2, null)
                .Build(DatabaseContext);
            
            var updateAssetTypeDto = AssetTypeFactory.NewRandomUpdateDto(fieldSet1.Id);
            
            var request = new RequestBuilder(HttpMethod.Put, $"/api/asset-types/{assetType.Id}/dry-run")
                .WithPayload(GetService<ISerializer>(), updateAssetTypeDto)
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var dryRunUpdateAssetTypeDto = await GetPayloadAsync<DryRunUpdateAssetTypeDto>(response);
            
            Assert.Single(dryRunUpdateAssetTypeDto.Assets);
            Assert.Equal(asset2.Id, dryRunUpdateAssetTypeDto.Assets[0].Id);
        }

        [Fact]
        public async Task Delete_WhenAssetTypeExists_ShouldReturnOk()
        {
            // Arrange
            var fieldSet = await new FieldSetBuilder()
                .Build(DatabaseContext);
            
            var assetType = await new AssetTypeBuilder()
                .AddFieldSet(fieldSet)
                .Build(DatabaseContext);
            
            var request = new RequestBuilder(HttpMethod.Delete, $"/api/asset-types/{assetType.Id}")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var deletedAssetType = await DatabaseHelper.Get<AssetType>(DatabaseContext, assetType.Id);
            Assert.Null(deletedAssetType);
        }

        [Fact]
        public async Task Delete_WhenAssetTypeDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var assetTypeId = Guid.NewGuid().ToString();

            var request = new RequestBuilder(HttpMethod.Delete, $"/api/asset-types/{assetTypeId}")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DryRunDelete_WhenAssetTypeNotUsed_ShouldReturnOkEmpty()
        {
            // Arrange
            var assetType = await new AssetTypeBuilder()
                .Build(DatabaseContext);
            
            var request = new RequestBuilder(HttpMethod.Delete, $"/api/asset-types/{assetType.Id}/dry-run")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var dryRunDeleteAssetTypeDto = await GetPayloadAsync<DryRunDeleteAssetTypeDto>(response);
            Assert.Empty(dryRunDeleteAssetTypeDto.Assets);
        }

        [Fact]
        public async Task DryRunDelete_WhenAssetsUseAssetType_ShouldReturnOkWithAssets()
        {
            // Arrange
            var field = await new FieldBuilder()
                .Build(DatabaseContext);
            
            var fieldSet = await new FieldSetBuilder()
                .AddField(field)
                .Build(DatabaseContext);
            
            var assetType1 = await new AssetTypeBuilder()
                .AddFieldSet(fieldSet)
                .Build(DatabaseContext);
            
            var assetType2 = await new AssetTypeBuilder()
                .AddFieldSet(fieldSet)
                .Build(DatabaseContext);
            
            var asset1 = await new AssetBuilder(assetType1.Id)
                .Build(DatabaseContext);
            
            var asset2 = await new AssetBuilder(assetType1.Id)
                .Build(DatabaseContext);
            
            await new AssetBuilder(assetType2.Id)
                .Build(DatabaseContext);
            
            var request = new RequestBuilder(HttpMethod.Delete, $"/api/asset-types/{assetType1.Id}/dry-run")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var dryRunDeleteAssetTypeDto = await GetPayloadAsync<DryRunDeleteAssetTypeDto>(response);
            
            Assert.Equal(2, dryRunDeleteAssetTypeDto.Assets.Count);
            Assert.Contains(asset1.Id, dryRunDeleteAssetTypeDto.Assets.Select(_ => _.Id));
            Assert.Contains(asset2.Id, dryRunDeleteAssetTypeDto.Assets.Select(_ => _.Id));
        }
    }
}