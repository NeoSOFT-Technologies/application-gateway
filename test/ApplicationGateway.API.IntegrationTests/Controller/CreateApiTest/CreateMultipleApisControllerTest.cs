using ApplicationGateway.API.IntegrationTests.Base;
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
    public class CreateMultipleApisControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CreateMultipleApisControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task CreateMultipleApis_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();
            Guid newid;
            IList<string> path = new List<string>();
            string Url = "";
            IList<CreateRequest> requestModel1 = new List<CreateRequest>();
            var myJsonString = File.ReadAllText("../../../JsonData/CreateApiTest/createMultipleApiData.json");
            requestModel1 = JsonConvert.DeserializeObject<List<CreateRequest>>(myJsonString);

            foreach (CreateRequest obj in requestModel1)
            {
                newid = Guid.NewGuid();
                obj.name = newid.ToString();
                obj.listenPath = $"/{newid.ToString()}/";
                path.Add(newid.ToString());
            }

            //create Apis
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateMultipleApi/createMultipleApi", content);

            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            IList<ResponseModel> responseModel = new List<ResponseModel>();
            responseModel = JsonConvert.DeserializeObject<List<ResponseModel>>(jsonString.Result);

            await HotReload();
            Thread.Sleep(7000);


            foreach (var item in path)
            {
                //downstream
                Url = $"http://localhost:8080/" + item + "/WeatherForecast";

                var responseN = await DownStream(Url);
                responseN.EnsureSuccessStatusCode();
            }

            var id = "";
            foreach (ResponseModel obj in responseModel)
            {
                //delete Api
                id = obj.key;
                var deleteResponse = await DeleteApi(id);
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
