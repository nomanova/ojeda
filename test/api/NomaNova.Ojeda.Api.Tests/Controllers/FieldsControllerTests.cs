using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NomaNova.Ojeda.Api.Tests.Controllers.Base;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Utils.Services.Interfaces;
using Xunit;

namespace NomaNova.Ojeda.Api.Tests.Controllers
{
    public class FieldsControllerTests : ApiTests
    {
        [Fact]
        public async Task CreateField_WhenValid_ShouldReturnCreated()
        {
            // Create
            var fieldDto = new CreateFieldDto
            {
                Name = "Serial Number",
                Description = "Unique device identification as provided by the manufacturer",
                Properties = new TextFieldPropertiesDto
                {
                    Type = FieldTypeDto.Text   
                }
            };
            
            var request = new RequestBuilder(HttpMethod.Post, "/api/fields")
                .WithPayload(GetService<ISerializer>(), fieldDto)
                .Build();
            
            var response = await ApiClient.SendAsync(request);
            
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // Get
            var location = response.Headers.Location;
            Assert.NotNull(location);
            
            request = new RequestBuilder(location.ToString())
                .Build();
            
            response = await ApiClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fetchedFieldDto = await GetPayloadAsync<FieldDto>(response);
            Assert.NotNull(fetchedFieldDto);
            Assert.NotNull(fetchedFieldDto.Id);
            Assert.Equal(fieldDto.Name, fetchedFieldDto.Name);
            Assert.Equal(fieldDto.Description, fetchedFieldDto.Description);
        }
    }
}