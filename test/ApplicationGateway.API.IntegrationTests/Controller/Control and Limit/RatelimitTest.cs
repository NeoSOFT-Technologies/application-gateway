using ApplicationGateway.API.IntegrationTests.Base;
using ApplicationGateway.API.IntegrationTests.Helper;
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
    public class RatelimitTest : IClassFixture<CustomWebApplicationFactory>
    {

        private readonly CustomWebApplicationFactory _factory;

        public RatelimitTest(CustomWebApplicationFactory factory)
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
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH+"/ControlandLimit/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);
            var id = result.key;
            await HotReload();
            // Thread.Sleep(4000);

            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH+"/ControlandLimit/masterJson.json");
            UpdateRequest data = JsonConvert.DeserializeObject<UpdateRequest>(myJsonString1);
            data.name = newid.ToString();
            data.listenPath = $"/{newid.ToString()}/";
            data.id = Guid.Parse(id);
            data.targetUrl = ApplicationConstants.TARGET_URL;
            data.rateLimit.rate = 8;
            data.rateLimit.per = 10;


            // Update_Api
            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/ApplicationGateway/UpdateApi/updateapi", content1);
            response1.EnsureSuccessStatusCode();
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
            var response = await client.GetAsync("/api/v1/ApplicationGateway/HotReload/HotReload");
            response.EnsureSuccessStatusCode();
        }

        private async Task<HttpResponseMessage> DeleteApi(string id)
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/v1/ApplicationGateway/DeleteApi/deleteApi?apiId=" + id);
            // await HotReload();
            return response;
        }
    }
}
