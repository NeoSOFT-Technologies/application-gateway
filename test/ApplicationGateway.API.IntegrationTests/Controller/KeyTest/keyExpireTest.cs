using ApplicationGateway.API.IntegrationTests.Base;
using ApplicationGateway.Domain.TykData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using ApplicationGateway.API.IntegrationTests.Helper;

namespace ApplicationGateway.API.IntegrationTests.Controller
{
    public class keyExpireTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        public keyExpireTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;

        }

        [Fact]
        public async Task ExpireTest()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);

            var id = result.key;
            await HotReload();

            //read update json file
            var myupdateJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/KeyTest/updateApiData.json");
            UpdateRequest updaterequestModel1 = JsonConvert.DeserializeObject<UpdateRequest>(myupdateJsonString);
            updaterequestModel1.name = newid.ToString();
            updaterequestModel1.listenPath = $"/{newid}/";
            updaterequestModel1.id = new Guid(id);

            //update appi
            var updateRequestJson = JsonConvert.SerializeObject(updaterequestModel1);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway/UpdateApi/updateapi", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            await HotReload();
            Thread.Sleep(2000);

            // read createkey json file
            var myJsonStringKey = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createKeyData.json");
            //Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(DateTime.Now.AddSeconds(60))).TotalSeconds;
            DateTime foo = DateTime.Now.AddSeconds(30);
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
            JObject keyrequestmodel = JObject.Parse(myJsonStringKey);
            foreach (var item in keyrequestmodel["accessRights"])
            {
                item["apiId"] = id.ToString();
                item["apiName"] = newid.ToString();
            }
            keyrequestmodel["expires"] = unixTime;
            StringContent stringContent = new StringContent(keyrequestmodel.ToString(), System.Text.Encoding.UTF8, "application/json");

            //create key
            var responsekey = await client.PostAsync("/api/Key/CreateKey", stringContent);
            responsekey.EnsureSuccessStatusCode();
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            JObject key = JObject.Parse(jsonStringkey);
            var keyid = key["key"];

            //hit api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("Authorization", keyid.ToString());
            var responseclientkey = await clientkey.GetAsync(Url);
            responseclientkey.EnsureSuccessStatusCode();

            Thread.Sleep(30000);

            var responseclientkey1 = await clientkey.GetAsync(Url);
            responseclientkey1.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.Unauthorized);

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();


        }

        private async Task HotReload()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/v1/ApplicationGateway/HotReload/HotReload");
            response.EnsureSuccessStatusCode();
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

        private async Task<HttpResponseMessage> DeleteApi(string id)
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/v1/ApplicationGateway/DeleteApi/deleteApi?apiId=" + id);
            // await HotReload();
            return response;
        }
    }
}
