﻿using ApplicationGateway.API.IntegrationTests.Base;
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
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Responses;
using System.Collections.Generic;
using static DownstreamApi.Controllers.WeatherForecastController;

namespace ApplicationGateway.API.IntegrationTests.Controller
{
    [Collection("Database")]
    public class Create_Api_with_key_allowedUrl: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
       
        private HttpClient client = null;
        public Create_Api_with_key_allowedUrl(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            client = _factory.CreateClient();

        }

        [Fact]
        public async Task Create_Api_with_AllowedUrl()
        {
            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

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

            //read update json file
            var myupdateJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/KeyTest/updateApiData.json");
            UpdateApiCommand updaterequestModel1 = JsonConvert.DeserializeObject<UpdateApiCommand>(myupdateJsonString);
            updaterequestModel1.Name = newid.ToString();
            updaterequestModel1.ListenPath = $"/{newid}/";
            updaterequestModel1.ApiId = id;
            updaterequestModel1.AuthType = "standard";

            //updateappi
            var updateRequestJson = JsonConvert.SerializeObject(updaterequestModel1);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            Thread.Sleep(5000);

            //read json file 
            var myJsonStringKey = File.ReadAllText(ApplicationConstants.BASE_PATH + "/KeyTest/createkeydata_allowedurl.json");
            JObject keyrequestmodel = JObject.Parse(myJsonStringKey);
            foreach (var item in keyrequestmodel["AccessRights"])
            {
                item["ApiId"] = id.ToString();
                item["ApiName"] = newid.ToString();
            }
            StringContent stringContent = new StringContent(keyrequestmodel.ToString(), System.Text.Encoding.UTF8, "application/json");

            //create key
            var responsekey = await client.PostAsync("/api/v1/Key/CreateKey", stringContent);
            responsekey.EnsureSuccessStatusCode();
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            JObject key = JObject.Parse(jsonStringkey);
            var keyid = key["data"]["keyId"];

            //hit api
            var clientkey = HttpClientFactory.Create();
            postweatherforecast obj = new postweatherforecast();
            obj.test = "test";
            //StringContent stringContent1 = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            clientkey.DefaultRequestHeaders.Add("Authorization", keyid.ToString());
            var responseclientkey = await clientkey.GetAsync(Url);
            responseclientkey.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.Forbidden);

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
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

        private async Task<HttpResponseMessage> DeleteApi(Guid id)
        {
            //var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/v1/ApplicationGateway/" + id);
            // await HotReload();
            return response;
        }
    }
}