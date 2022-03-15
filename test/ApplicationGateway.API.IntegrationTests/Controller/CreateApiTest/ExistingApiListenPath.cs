using ApplicationGateway.API.IntegrationTests.Base;
using ApplicationGateway.API.IntegrationTests.Helper;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.TykData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationGateway.API.IntegrationTests.Controller.CreateApiTest
{
    [Collection("Database")]
    public partial class ExistingApiListenPath : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient client = null;
        public ExistingApiListenPath(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            client = _factory.CreateClient();
        }

       // [Fact]
        public async Task CreateApi_ReturnsSuccessResult()
        {
   
            //var client = _factory.CreateClient();

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/CreateApiTest/createApiData.json");
            CreateApiCommand requestModel1 = JsonConvert.DeserializeObject<CreateApiCommand>(myJsonString);

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateApiDto>>(jsonString.Result);
            var id = result.Data.ApiId;
            Thread.Sleep(5000);

            //downstream
            var listenpath = requestModel1.ListenPath.Trim(new char[] { '/' });
            string Url = ApplicationConstants.TYK_BASE_URL + listenpath + "/WeatherForecast";
            var responseN = await DownStream(Url);
            responseN.EnsureSuccessStatusCode();

            //create Api with existing listenpath
            var ExRequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent excontent = new StringContent(ExRequestJson, Encoding.UTF8, "application/json");
            var exresponse = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", excontent);
            exresponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.BadRequest);

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
        }

        public static async Task<HttpResponseMessage> DownStream(string path)
        {

            try
            {
                var clientdown = HttpClientFactory.Create();
                var response = await clientdown.GetAsync(path);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        private async Task<HttpResponseMessage> DeleteApi(Guid id)
        {
            //var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/v1/ApplicationGateway/" + id);
            // await HotReload();
            return response;
        }
    }
}
