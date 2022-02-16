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
    public class KeyTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        //private readonly ILogger<KeyControllerTest> _logger;
        public KeyTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
           
        }

        [Fact]
        public async Task KeyRateLimiting()
        {
            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH+"/keyTest/createApiData.json");
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
            Thread.Sleep(4000);

            //read update json file
            var myupdateJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH+"/KeyTest/updateApiData.json");
            UpdateRequest updaterequestModel1 = JsonConvert.DeserializeObject<UpdateRequest>(myupdateJsonString);
            updaterequestModel1.name = newid.ToString();
            updaterequestModel1.listenPath = $"/{newid}/";
            updaterequestModel1.id = new Guid(id);
            updaterequestModel1.authType = "standard";
            //updateappi
            var updateRequestJson = JsonConvert.SerializeObject(updaterequestModel1);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway/UpdateApi/updateapi", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            await HotReload();
            Thread.Sleep(2000);

            //read json file 
            var myJsonStringKey = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createKeyData.json");
            //CreateKeyRequest keyrequestModel = JsonConvert.DeserializeObject<CreateKeyRequest>(myJsonStringKey);
            //foreach(var x in keyrequestModel.accessRights)
            //{
            //    x.apiId = id;
            //    x.apiName = newid.ToString();
            //}
            JObject keyrequestmodel = JObject.Parse(myJsonStringKey);
            foreach(var item in keyrequestmodel["accessRights"])
            {
                item["apiId"] = id.ToString();
                item["apiName"] = newid.ToString();
            }
            StringContent stringContent = new StringContent(keyrequestmodel.ToString(), System.Text.Encoding.UTF8, "application/json");

            //create key
            //var RequestJsonkey = JsonConvert.SerializeObject(requestModel1);
            //HttpContent contentkey = new StringContent(RequestJsonkey, Encoding.UTF8, "application/json");
            var responsekey = await client.PostAsync("/api/Key/CreateKey", stringContent);
            responsekey.EnsureSuccessStatusCode();
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            JObject key = JObject.Parse(jsonStringkey);
           
            //ResponseModel resultkey = JsonConvert.DeserializeObject<ResponseModel>(jsonStringkey);
            var keyid = key["key"];

            //hit api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("Authorization", keyid.ToString());
            
            for (int i = 1; i < 4; i++)
            {
                var responseclientkey = await clientkey.GetAsync(Url);
                var check = responseclientkey.EnsureSuccessStatusCode();
                if (!check.IsSuccessStatusCode)
                {
                    
                    break;
                }
            }
            
            //CHECK RATE LIMITING 
            var responseclientkey1 = await clientkey.GetAsync(Url);
            responseclientkey1.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.TooManyRequests);
            

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
