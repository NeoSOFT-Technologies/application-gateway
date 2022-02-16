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

namespace ApplicationGateway.API.IntegrationTests.Controller
{
    public class CreateApiTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CreateApiTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateApi_ReturnsSuccessResult()
        {
            Console.WriteLine("test started");
            var client = _factory.CreateClient();

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/CreateApiTest/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateApiDto>>(jsonString.Result);
            var id = result.Data.ApiId;
            await HotReload();
            Thread.Sleep(4000);

            //downstream
            var listenpath = requestModel1.listenPath.Trim(new char[] { '/' });
            string Url = ApplicationConstants.TYK_BASE_URL + listenpath + "/WeatherForecast";
            var responseN = await DownStream(Url);
            responseN.EnsureSuccessStatusCode();

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();

        }

        public async Task<HttpResponseMessage> DownStream(string path)
        {

            try
            {
                var client = HttpClientFactory.Create();
                var response = await client.GetAsync(path);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        private async Task HotReload()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/v1/ApplicationGateway/HotReload");
            response.EnsureSuccessStatusCode();
        }

        private async Task<HttpResponseMessage> DeleteApi(Guid id)
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/v1/ApplicationGateway/" + id);
            // await HotReload();
            return response;
        }
    }
}
