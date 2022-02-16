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
using System.Collections.Generic;

namespace ApplicationGateway.API.IntegrationTests.Controller
{
    public class MultipeApiwithKey : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        public MultipeApiwithKey(CustomWebApplicationFactory factory)
        {
            _factory = factory;

        }

        [Fact]
        public async Task Should_create_key()
        {
            var client = _factory.CreateClient();
            Guid newid;
            IList<string> path = new List<string>();
            string Url = "";
            List<string> apiName = new List<string>();
            IList<CreateRequest> requestModel1 = new List<CreateRequest>();
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/CreateApiTest/createMultipleApiData.json");
            requestModel1 = JsonConvert.DeserializeObject<List<CreateRequest>>(myJsonString);

            foreach (CreateRequest obj in requestModel1)
            {
                newid = Guid.NewGuid();
                obj.name = newid.ToString();
                apiName.Add(obj.name);
                obj.listenPath = $"/{newid}/";
                path.Add(newid.ToString());
            }

            //create Apis
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateMultipleApi/createMultipleApi", content);
            response.EnsureSuccessStatusCode();
            Thread.Sleep(3000);
            var jsonString = response.Content.ReadAsStringAsync();

            IList<ResponseModel> responseModel = new List<ResponseModel>();
            responseModel = JsonConvert.DeserializeObject<List<ResponseModel>>(jsonString.Result);

            //read craatekey json file 
            var myJsonStringKey = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createKeyData.json");
            JObject keyrequestmodel = JObject.Parse(myJsonStringKey);
            string[] version = new string[] { "Default" };
            JArray jarrayObj = new JArray();
            foreach (string versions in version)
            {
                jarrayObj.Add(versions);
            }
            JArray accessRight = new JArray();
            for (var i = 0; i < responseModel.Count; i++)
            {
                accessRight.Add( new JObject(
                new JProperty("apiId", responseModel[i].key),
                new JProperty("apiName", apiName[i]),
                new JProperty("versions", jarrayObj)));
            }
            keyrequestmodel["accessRights"] = accessRight;
            StringContent stringContent = new StringContent(keyrequestmodel.ToString(), System.Text.Encoding.UTF8, "application/json");
            //create key
            var responsekey = await client.PostAsync("/api/Key/CreateKey", stringContent);
            responsekey.EnsureSuccessStatusCode();

            //read response key
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            JObject key = JObject.Parse(jsonStringkey);
            var keyid = key["key"];
            foreach(var item in apiName)
            {
                var clientkey = HttpClientFactory.Create();
                clientkey.DefaultRequestHeaders.Add("Authorization", keyid.ToString());
                Url = ApplicationConstants.TYK_BASE_URL + item.ToString() + "/WeatherForecast";
                var responseclientkey = await clientkey.GetAsync(Url);
                var check = responseclientkey.EnsureSuccessStatusCode();
            }
            await HotReload();

            //delete Api
            foreach(var item in keyrequestmodel)
            {
                var deleteResponse = await DeleteApi((item.Key).ToString());
                deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
                await HotReload();
            } 
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
