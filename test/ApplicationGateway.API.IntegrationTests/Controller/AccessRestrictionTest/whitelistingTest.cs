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
    public class whitelistingTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public whitelistingTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Whitelisting()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";
            string OriginUrl = $"http://localhost:8080/" + newid.ToString() + "/";
            //read json file 
            var myJsonString = File.ReadAllText("../../../JsonData/AccessRestrictionTest/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid}/";
            requestModel1.targetUrl = "http://httpbin.org/get";
            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);

            var id = result.key;
            await HotReload();
            Thread.Sleep(2000);

            //getorigin
            var ipaddress = await getsOrigin(OriginUrl);
            var ip = ipaddress.origin;
            string[] values = ip.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
            }
            var ipvalue = values[0];

            //read update json file
            var myupdateJsonString = File.ReadAllText("../../../JsonData/AccessRestrictionTest/updateApiData.json");
            UpdateRequest updaterequestModel1 = JsonConvert.DeserializeObject<UpdateRequest>(myupdateJsonString);
            updaterequestModel1.name = newid.ToString();
            updaterequestModel1.listenPath = $"/{newid}/";
            updaterequestModel1.id = new Guid(id);
            updaterequestModel1.whitelist = new List<string>
            {
                ipvalue
            };
            //updateappi
            var updateRequestJson = JsonConvert.SerializeObject(updaterequestModel1);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway/UpdateApi/updateapi", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            await HotReload();
            Thread.Sleep(2000);

            //downstream
            var downstreamResponse = await DownStream(Url);
            downstreamResponse.EnsureSuccessStatusCode();

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
        public async Task<Docker> getsOrigin(string path)
        {
            var client = HttpClientFactory.Create();
            var response = await client.GetAsync(path);
            var jsonString = response.Content.ReadAsStringAsync();
            Docker result = JsonConvert.DeserializeObject<Docker>(jsonString.Result);
            return result;
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
