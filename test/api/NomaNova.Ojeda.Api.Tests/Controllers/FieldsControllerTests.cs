using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NomaNova.Ojeda.Api.Tests.Builders;
using NomaNova.Ojeda.Api.Tests.Controllers.Base;
using NomaNova.Ojeda.Api.Tests.Factories;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Utils.Services.Interfaces;
using Xunit;

namespace NomaNova.Ojeda.Api.Tests.Controllers
{
    public class FieldsControllerTests : ApiTests
    {
        [Fact]
        public async Task GetById_WhenFieldExists_ShouldReturnOk()
        {
            // Arrange
            var field = await new FieldBuilder()
                .Build(DatabaseContext);

            var request = new RequestBuilder($"/api/fields/{field.Id}")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetById_WhenFieldDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var fieldId = Guid.NewGuid().ToString();

            var request = new RequestBuilder($"/api/fields/{fieldId}")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_WhenFieldsExist_ShouldReturnOk()
        {
            // Arrange
            await new FieldBuilder()
                .Build(DatabaseContext);

            await new FieldBuilder()
                .Build(DatabaseContext);

            var request = new RequestBuilder("/api/fields")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fieldsDto = await GetPayloadAsync<PaginatedListDto<FieldDto>>(response);
            Assert.Equal(2, fieldsDto.TotalCount);
        }

        [Fact]
        public async Task Get_WhenNoFieldsExist_ShouldReturnOkEmpty()
        {
            // Arrange
            var request = new RequestBuilder("/api/fields")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fieldsDto = await GetPayloadAsync<PaginatedListDto<FieldDto>>(response);
            Assert.Equal(0, fieldsDto.TotalCount);
        }

        [Fact]
        public async Task Create_WhenValid_ShouldReturnCreated()
        {
            // Arrange
            var createFieldDto = FieldFactory.NewRandomCreateDto();

            var request = new RequestBuilder(HttpMethod.Post, "/api/fields")
                .WithPayload(GetService<ISerializer>(), createFieldDto)
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var location = response.Headers.Location;
            Assert.NotNull(location);

            var fieldDto = await GetPayloadAsync<FieldDto>(response);
            Assert.NotNull(fieldDto);

            var field = await DatabaseHelper.Get<Field>(DatabaseContext, fieldDto.Id);
            Assert.NotNull(field);
        }

        [Fact]
        public async Task Update_WhenValidAndExists_ShouldReturnOk()
        {
            // Arrange
            var field = await new FieldBuilder()
                .Build(DatabaseContext);

            var updateFieldDto = FieldFactory.NewRandomUpdateDto();

            var request = new RequestBuilder(HttpMethod.Put, $"/api/fields/{field.Id}")
                .WithPayload(GetService<ISerializer>(), updateFieldDto)
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updatedField = await DatabaseHelper.Get<Field>(DatabaseContext, field.Id);

            Assert.NotNull(updatedField);
            Assert.Equal(updatedField.Name, updateFieldDto.Name);
            Assert.Equal(updatedField.Description, updateFieldDto.Description);
        }

        [Fact]
        public async Task Delete_WhenFieldExists_ShouldReturnOk()
        {
            // Arrange
            var field = await new FieldBuilder()
                .Build(DatabaseContext);

            var request = new RequestBuilder(HttpMethod.Delete, $"/api/fields/{field.Id}")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var deletedField = await DatabaseHelper.Get<Field>(DatabaseContext, field.Id);
            Assert.Null(deletedField);
        }

        [Fact]
        public async Task Delete_WhenFieldDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var fieldId = Guid.NewGuid().ToString();

            var request = new RequestBuilder(HttpMethod.Delete, $"/api/fields/{fieldId}")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DryRunDelete_WhenFieldNotUsed_ShouldReturnOkEmpty()
        {
            // Arrange
            var field = await new FieldBuilder()
                .Build(DatabaseContext);

            var request = new RequestBuilder(HttpMethod.Delete, $"/api/fields/{field.Id}/dry-run")
                .Build();

            // Act
            var response = await ApiClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var dryRunDeleteFieldDto = await GetPayloadAsync<DryRunDeleteFieldDto>(response);
            Assert.Empty(dryRunDeleteFieldDto.FieldSets);
            
            Assert.Empty(dryRunDeleteFieldDto.Assets);
        }

        [Fact]
        public async Task DryRunDelete_WhenFieldIncludedInFieldSet_ShouldReturnOkWithFieldSet()
        {
            // Arrange
            var field = await new FieldBuilder()
                .Build(DatabaseContext);

            var fieldSet = await new FieldSetBuilder()
                .WithField(field)
                .Build(DatabaseContext);
            
            var request = new RequestBuilder(HttpMethod.Delete, $"/api/fields/{field.Id}/dry-run")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var dryRunDeleteFieldDto = await GetPayloadAsync<DryRunDeleteFieldDto>(response);
            
            Assert.NotEmpty(dryRunDeleteFieldDto.FieldSets);
            Assert.Equal(dryRunDeleteFieldDto.FieldSets[0].Id, fieldSet.Id);
            
            Assert.Empty(dryRunDeleteFieldDto.Assets);
        }

        [Fact]
        public async Task DryRunDelete_WhenFieldHasAssetValue_ShouldReturnOkWithAsset()
        {
            // Arrange
            var field = await new FieldBuilder()
                .Build(DatabaseContext);
            
            var fieldSet = await new FieldSetBuilder()
                .WithField(field)
                .Build(DatabaseContext);

            var assetType = await new AssetTypeBuilder()
                .WithFieldSet(fieldSet)
                .Build(DatabaseContext);

            var asset = await new AssetBuilder(assetType.Id)
                .WithFieldValue(field, fieldSet, "123")
                .Build(DatabaseContext);
            
            await new AssetBuilder(assetType.Id)
                .Build(DatabaseContext);
            
            var request = new RequestBuilder(HttpMethod.Delete, $"/api/fields/{field.Id}/dry-run")
                .Build();
            
            // Act
            var response = await ApiClient.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var dryRunDeleteFieldDto = await GetPayloadAsync<DryRunDeleteFieldDto>(response);
            
            Assert.Single(dryRunDeleteFieldDto.FieldSets);
            Assert.Equal(dryRunDeleteFieldDto.FieldSets[0].Id, fieldSet.Id);

            Assert.Single(dryRunDeleteFieldDto.Assets);
            Assert.Equal(dryRunDeleteFieldDto.Assets[0].Id, asset.Id);
        }
    }
}