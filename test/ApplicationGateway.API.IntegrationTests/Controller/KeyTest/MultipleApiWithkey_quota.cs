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
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;

namespace ApplicationGateway.API.IntegrationTests.Controller
{
    [Collection("Database")]
    public class MultipleApiWithkey_quota : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        public MultipleApiWithkey_quota(CustomWebApplicationFactory factory)
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
            //IList<CreateMultipleApisCommand> requestModel1 = new List<CreateMultipleApisCommand>();

            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/CreateApiTest/createMultipleApiData.json");
            var requestModel1 = JsonConvert.DeserializeObject<CreateMultipleApisCommand>(myJsonString);

            foreach (MultipleApiModel obj in requestModel1.APIs)
            {
                newid = Guid.NewGuid();
                obj.Name = newid.ToString();
                apiName.Add(obj.Name);
                obj.ListenPath = $"/{newid}/";
                path.Add(newid.ToString());
            }

            //create Apis
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateMultipleApis", content);
            response.EnsureSuccessStatusCode();
            Thread.Sleep(5000);
            var jsonString = response.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<Response<CreateMultipleApisDto>>(jsonString.Result);

            //read update json file
            for (var i = 0; i < responseModel.Data.APIs.Count; i++)
            {
                var myupdateJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/KeyTest/updateApiData.json");
                UpdateApiCommand updaterequestModel1 = JsonConvert.DeserializeObject<UpdateApiCommand>(myupdateJsonString);
                updaterequestModel1.Name = responseModel.Data.APIs[i].Name;
                updaterequestModel1.ListenPath = $"/{responseModel.Data.APIs[i].Name}/";
                updaterequestModel1.ApiId = responseModel.Data.APIs[i].ApiId;
                updaterequestModel1.AuthType = "standard";

                //updateappi
                var updateRequestJson = JsonConvert.SerializeObject(updaterequestModel1);
                HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
                var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
                updateresponse.EnsureSuccessStatusCode();
                Thread.Sleep(5000);
            }
            //read craatekey json file 
            var myJsonStringKey = File.ReadAllText(ApplicationConstants.BASE_PATH + "/KeyTest/createkeydata_limit.json");
            JObject keyrequestmodel = JObject.Parse(myJsonStringKey);
            string[] version = new string[] { "Default" };
            JArray jarrayObj = new JArray();
            JArray jarrayObj1 = new JArray();
            foreach (string versions in version)
            {
                jarrayObj.Add(versions);
            }
            JArray accessRight = new JArray();
            JObject AllowedUrls = new JObject(
                new JProperty("Url", ""),
                new JProperty("Methods", jarrayObj1)
                );
            JObject Limit = new JObject(
                new JProperty("Rate", 0),
                new JProperty("Per", 0),
                new JProperty("Throttle_interval", 0),
                new JProperty("Throttle_retry_limit", 0),
                new JProperty("Max_query_depth", 0),
                new JProperty("Quota_max", 2),
                new JProperty("Quota_renews", 3600),
                new JProperty("Quota_remaining", 0),
                new JProperty("Quota_renewal_rate", 0)
                );
            for (var i = 0; i < responseModel.Data.APIs.Count; i++)
            {

                JObject obj = new JObject();
                obj.Add("ApiId", responseModel.Data.APIs[i].ApiId);
                obj.Add("ApiName", apiName[i]);
                obj.Add("Versions", jarrayObj);
                obj.Add("AllowedUrls", jarrayObj1);
                obj.Add("Limit", Limit);
                accessRight.Add(obj);

            }
            keyrequestmodel["AccessRights"] = accessRight;
            StringContent stringContent = new StringContent(keyrequestmodel.ToString(), System.Text.Encoding.UTF8, "application/json");
            //create key
            var responsekey = await client.PostAsync("/api/v1/Key/CreateKey", stringContent);
            responsekey.EnsureSuccessStatusCode();

            //read response key
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            JObject key = JObject.Parse(jsonStringkey);
            var keyid = key["data"]["keyId"];
            foreach (var item in apiName)
            {
                //var clientkey = HttpClientFactory.Create();
                //clientkey.DefaultRequestHeaders.Add("Authorization", keyid.ToString());
                Url = ApplicationConstants.TYK_BASE_URL + item.ToString() + "/WeatherForecast";
                for (var i = 0; i < 2; i++)
                {
                    var responseclientkeys = await DownStream(Url, keyid.ToString());
                    responseclientkeys.EnsureSuccessStatusCode();
                }
                var responseclientkey = await DownStream(Url, keyid.ToString());
                responseclientkey.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.Forbidden);
            }


            //delete Api
            foreach (var item in accessRight)
            {
                var deleteResponse = await DeleteApi(((new Guid((item["ApiId"].ToString())))));
                deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);

            }
        }


        public async Task<HttpResponseMessage> DownStream(string path, string keyid)
        {

            try
            {
                var clients = HttpClientFactory.Create();
                clients.DefaultRequestHeaders.Add("Authorization", keyid);
                var response = await clients.GetAsync(path);
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
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/v1/ApplicationGateway/" + id);
            // await HotReload();
            return response;
        }

    }
}
