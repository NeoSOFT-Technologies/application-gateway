﻿using ApplicationGateway.API.IntegrationTests.Base;
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
using ApplicationGateway.API.IntegrationTests.Helper;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;

namespace ApplicationGateway.API.IntegrationTests.Controller
{
    [Collection("Database")]
    public partial class VersioningKeys : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient client = null;
        public VersioningKeys(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            client = _factory.CreateClient();

        }

        [Fact]
        public async Task Key_Versioning_byHeader()
        {
            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";
            string versioncheck = "";
            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/KeyTest/createApiData.json");
            CreateApiCommand requestModel1 = JsonConvert.DeserializeObject<CreateApiCommand>(myJsonString);
            requestModel1.Name = newid.ToString();
            requestModel1.ListenPath = $"/{newid}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateApiDto>>(jsonString.Result);
            var id = result.Data.ApiId;
            Thread.Sleep(5000);

            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/KeyTest/Header_Version.json");
            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;

            // Update_Api
            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/ApplicationGateway", content1);
            response1.EnsureSuccessStatusCode();
           
            Thread.Sleep(5000);

            //create key for version v1
            //read json file 
            var myJsonStringKey = File.ReadAllText(ApplicationConstants.BASE_PATH + "/KeyTest/createKeyData.json");
            JObject keyrequestmodel = JObject.Parse(myJsonStringKey);
            foreach (var item in keyrequestmodel["AccessRights"])
            {
                item["ApiId"] = id.ToString();
                item["ApiName"] = newid.ToString();
                item["Versions"][0] = data.Versions[0].Name;
                versioncheck = data.Versions[0].Name;

            }
            StringContent stringContent = new StringContent(keyrequestmodel.ToString(), System.Text.Encoding.UTF8, "application/json");

            //create key
            var responsekey = await client.PostAsync("/api/v1/Key/CreateKey", stringContent);
            responsekey.EnsureSuccessStatusCode();
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            JObject key = JObject.Parse(jsonStringkey);
            var keyid = key["Data"]["KeyId"];

            foreach(UpdateVersionModel obj in data.Versions)
            {
                var clientV = HttpClientFactory.Create();
                clientV.DefaultRequestHeaders.Add("gateway-authorization", keyid.ToString());
                clientV.DefaultRequestHeaders.Add(data.VersioningInfo.Key, obj.Name);
                if(versioncheck == obj.Name)
                {
                    var responseV = await clientV.GetAsync(Url);
                    responseV.EnsureSuccessStatusCode();
                }
                else
                {
                    var responseV = await clientV.GetAsync(Url);
                    responseV.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.Forbidden);
                }
               
            }

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
     

        }

        public static async Task<HttpResponseMessage> DownStream(string path)
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

        private async Task<HttpResponseMessage> DeleteApi(Guid id)
        {
            //var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/v1/ApplicationGateway/"+id);
            return response;
        }
    }
}
