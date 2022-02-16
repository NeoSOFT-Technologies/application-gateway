using ApplicationGateway.API.IntegrationTests.Base;
using ApplicationGateway.API.IntegrationTests.Helper;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
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
    public class ratelimitTest : IClassFixture<CustomWebApplicationFactory>
    {

        private readonly CustomWebApplicationFactory _factory;

        public ratelimitTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RateLimiting()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/ControlandLimit/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateApiDto>>(jsonString.Result);
            var id = result.Data.ApiId;
            await HotReload();
            Thread.Sleep(3000);

            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/ControlandLimit/rateLimitData.json");
            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;
            data.TargetUrl = ApplicationConstants.TARGET_URL;

            // Update_Api
            var updateRequestJson = JsonConvert.SerializeObject(data);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            await HotReload();
            Thread.Sleep(5000);

            // downstream
            for (int i = 0; i < 8; i++)
            {
                var responseh = await DownStream(Url);
                responseh.EnsureSuccessStatusCode();
            }

            //check Rate Limit Exceed
            response = await DownStream(Url);
            response.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.TooManyRequests);

            //Rate Limiting Reset
            Thread.Sleep(10000);
            client = HttpClientFactory.Create();
            response = await client.GetAsync(Url);
            response.EnsureSuccessStatusCode();


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
            return response;
        }
    }
}