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
    public class CreateMultipleApisTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CreateMultipleApisTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task CreateMultipleApis_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();
            string Url;
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/CreateApiTest/createMultipleApiData.json");
            IList<CreateRequest> requestModel1 = JsonConvert.DeserializeObject<List<CreateRequest>>(myJsonString);

            //create Apis
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateMultipleApi/createMultipleApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            IList<ResponseModel> responseModel = JsonConvert.DeserializeObject<List<ResponseModel>>(jsonString.Result);

            await HotReload();
            Thread.Sleep(5000);

            //downstream
            foreach (var item in requestModel1)
            {
                var path1 = item.listenPath.Trim(new char[] { '/' });
                Url = ApplicationConstants.TYK_BASE_URL + path1 + "/WeatherForecast";
                var responseN = await DownStream(Url);
                responseN.EnsureSuccessStatusCode();
            }

            //delete Api
            foreach (ResponseModel obj in responseModel)
            {
                var deleteResponse = await DeleteApi(obj.key);
                deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
                await HotReload();
            }
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
