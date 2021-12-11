using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NomaNova.Ojeda.Api.Tests.Builders;
using NomaNova.Ojeda.Api.Tests.Controllers.Base;
using NomaNova.Ojeda.Api.Tests.Factories;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Utils.Services.Interfaces;
using Xunit;

namespace NomaNova.Ojeda.Api.Tests.Controllers
{
    public class FieldSetsControllerTests : ApiTests
    {
        [Fact]
        public async Task GetById_WhenFieldSetExists_ShouldReturnOk()
        {
            // Arrange
            var fieldSet = await new FieldSetBuilder()
                .Build(DatabaseContext);

            var request = new RequestBuilder($"/api/field-sets/{fieldSet.Id}")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task GetById_WhenFieldSetDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var fieldSetId = Guid.NewGuid().ToString();

            var request = new RequestBuilder($"/api/field-sets/{fieldSetId}")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_WhenFieldSetsExist_ShouldReturnOk()
        {
            // Arrange
            await new FieldSetBuilder()
                .Build(DatabaseContext);

            await new FieldSetBuilder()
                .Build(DatabaseContext);

            var request = new RequestBuilder("/api/field-sets")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var fieldSetsDto = await GetPayloadAsync<PaginatedListDto<FieldSetDto>>(response);
            Assert.Equal(2, fieldSetsDto.TotalCount);
        }

        [Fact]
        public async Task Get_WhenNoFieldSetsExist_ShouldReturnOkEmpty()
        {
            // Arrange
            var request = new RequestBuilder("/api/field-sets")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fieldSetsDto = await GetPayloadAsync<PaginatedListDto<FieldSetDto>>(response);
            Assert.Equal(0, fieldSetsDto.TotalCount);
        }

        [Fact]
        public async Task Create_WhenValid_ShouldReturnCreated()
        {
            // Arrange
            var field = await new FieldBuilder()
                .Build(DatabaseContext);
            
            var createFieldSetDto = FieldSetFactory.NewRandomCreateDto(field.Id);
            
            var request = new RequestBuilder(HttpMethod.Post, "/api/field-sets")
                .WithPayload(GetService<ISerializer>(), createFieldSetDto)
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var location = response.Headers.Location;
            Assert.NotNull(location);
            
            var fieldSetDto = await GetPayloadAsync<FieldSetDto>(response);
            Assert.NotNull(fieldSetDto);
            
            var fieldSet = await DatabaseHelper.Get<FieldSet>(DatabaseContext, fieldSetDto.Id);
            Assert.NotNull(fieldSet);
        }

        [Fact]
        public async Task Update_WhenValidAndExists_ShouldReturnOk()
        {
            // Arrange
            var field = await new FieldBuilder()
                .Build(DatabaseContext);

            var fieldSet = await new FieldSetBuilder()
                .AddField(field)
                .Build(DatabaseContext);

            var updateFieldSetDto = FieldSetFactory.NewRandomUpdateDto(field.Id);
            
            var request = new RequestBuilder(HttpMethod.Put, $"/api/field-sets/{fieldSet.Id}")
                .WithPayload(GetService<ISerializer>(), updateFieldSetDto)
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var updatedFieldSet = await DatabaseHelper.Get<FieldSet>(DatabaseContext, fieldSet.Id);
            
            Assert.NotNull(updatedFieldSet);
            Assert.Equal(updatedFieldSet.Name, updateFieldSetDto.Name);
            Assert.Equal(updatedFieldSet.Description, updateFieldSetDto.Description);
        }

        [Fact]
        public async Task DryRunUpdate_WhenNoFieldsRemoved_ShouldReturnOkEmpty()
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
            
            var updateFieldSetDto = FieldSetFactory.NewRandomUpdateDto(field.Id);
            
            var request = new RequestBuilder(HttpMethod.Put, $"/api/field-sets/{fieldSet.Id}/dry-run")
                .WithPayload(GetService<ISerializer>(), updateFieldSetDto)
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var dryRunUpdateFieldSetDto = await GetPayloadAsync<DryRunUpdateFieldSetDto>(response);
            Assert.Empty(dryRunUpdateFieldSetDto.Assets);
        }

        [Fact]
        public async Task DryRunUpdate_WhenFieldsRemoved_ShouldReturnOkWithAsset()
        {
            // Arrange
            var field1 = await new FieldBuilder()
                .Build(DatabaseContext);
            
            var field2 = await new FieldBuilder()
                .Build(DatabaseContext);

            var fieldSet = await new FieldSetBuilder()
                .AddFields(field1, field2)
                .Build(DatabaseContext);
            
            var assetType = await new AssetTypeBuilder()
                .AddFieldSet(fieldSet)
                .Build(DatabaseContext);
            
            await new AssetBuilder(assetType.Id)
                .AddFieldValue(field1, fieldSet, "123")
                .Build(DatabaseContext);
            
            var asset2 = await new AssetBuilder(assetType.Id)
                .AddFieldValue(field2, fieldSet, "456")
                .Build(DatabaseContext);
            
            var updateFieldSetDto = FieldSetFactory.NewRandomUpdateDto(field1.Id);
            
            var request = new RequestBuilder(HttpMethod.Put, $"/api/field-sets/{fieldSet.Id}/dry-run")
                .WithPayload(GetService<ISerializer>(), updateFieldSetDto)
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var dryRunUpdateFieldSetDto = await GetPayloadAsync<DryRunUpdateFieldSetDto>(response);
            
            Assert.Single(dryRunUpdateFieldSetDto.Assets);
            Assert.Equal(asset2.Id, dryRunUpdateFieldSetDto.Assets[0].Id);
        }

        [Fact]
        public async Task Delete_WhenFieldSetExists_ShouldReturnOk()
        {
            // Arrange
            var field = await new FieldBuilder()
                .Build(DatabaseContext);
            
            var fieldSet = await new FieldSetBuilder()
                .AddField(field)
                .Build(DatabaseContext);
            
            var request = new RequestBuilder(HttpMethod.Delete, $"/api/field-sets/{fieldSet.Id}")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var deletedFieldSet = await DatabaseHelper.Get<FieldSet>(DatabaseContext, fieldSet.Id);
            Assert.Null(deletedFieldSet);
        }

        [Fact]
        public async Task Delete_WhenFieldSetDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var fieldSetId = Guid.NewGuid().ToString();

            var request = new RequestBuilder(HttpMethod.Delete, $"/api/field-sets/{fieldSetId}")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DryRunDelete_WhenFieldSetNotUsed_ShouldReturnOkEmpty()
        {
            // Arrange
            var fieldSet = await new FieldSetBuilder()
                .Build(DatabaseContext);

            var request = new RequestBuilder(HttpMethod.Delete, $"/api/field-sets/{fieldSet.Id}/dry-run")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var dryRunDeleteFieldSetDto = await GetPayloadAsync<DryRunDeleteFieldSetDto>(response);
            Assert.Empty(dryRunDeleteFieldSetDto.AssetTypes);
            
            Assert.Empty(dryRunDeleteFieldSetDto.Assets);
        }

        [Fact]
        public async Task DryRunDelete_WhenFieldSetIncludedInAssetType_ShouldReturnOkWithAssetType()
        {
            // Arrange
            var fieldSet = await new FieldSetBuilder()
                .Build(DatabaseContext);

            var assetType = await new AssetTypeBuilder()
                .AddFieldSet(fieldSet)
                .Build(DatabaseContext);

            var request = new RequestBuilder(HttpMethod.Delete, $"/api/field-sets/{fieldSet.Id}/dry-run")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var dryRunDeleteFieldSetDto = await GetPayloadAsync<DryRunDeleteFieldSetDto>(response);
            
            Assert.NotEmpty(dryRunDeleteFieldSetDto.AssetTypes);
            Assert.Equal(dryRunDeleteFieldSetDto.AssetTypes[0].Id, assetType.Id);
            
            Assert.Empty(dryRunDeleteFieldSetDto.Assets);
        }

        [Fact]
        public async Task DryRunDelete_WhenFieldOfFieldSetHasAssetValue_ShouldReturnOkWithAsset()
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
            
            var asset = await new AssetBuilder(assetType.Id)
                .AddFieldValue(field, fieldSet, "123")
                .Build(DatabaseContext);
            
            await new AssetBuilder(assetType.Id)
                .Build(DatabaseContext);
            
            var request = new RequestBuilder(HttpMethod.Delete, $"/api/field-sets/{fieldSet.Id}/dry-run")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var dryRunDeleteFieldSetDto = await GetPayloadAsync<DryRunDeleteFieldSetDto>(response);
            
            Assert.Single(dryRunDeleteFieldSetDto.AssetTypes);
            Assert.Equal(dryRunDeleteFieldSetDto.AssetTypes[0].Id, assetType.Id);
            
            Assert.Single(dryRunDeleteFieldSetDto.Assets);
            Assert.Equal(dryRunDeleteFieldSetDto.Assets[0].Id, asset.Id);
        }
    }
}