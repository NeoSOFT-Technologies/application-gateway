﻿using ApplicationGateway.API.IntegrationTests.Base;
using ApplicationGateway.API.IntegrationTests.Helper;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
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

namespace ApplicationGateway.API.IntegrationTests.Controller.PolicyTest.AccessControl
{
    public class MultipleApiUrlTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient client = null;
        public MultipleApiUrlTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            client = _factory.CreateClient();
        }

        [Fact]
        public async Task CreateMultipleApiPolicyWithVersion()
        {
            var client = _factory.CreateClient();
            Guid newid;
            IList<string> path = new List<string>();
            string Url = "";
            List<string> apiName = new List<string>();
            JArray policyIds = new JArray();
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
            Thread.Sleep(3000);
            var jsonString = response.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<Response<CreateMultipleApisDto>>(jsonString.Result);


            List<UpdateApiCommand> Apidata = new List<UpdateApiCommand>();
            //read update json file
            for (var i = 0; i < responseModel.Data.APIs.Count; i++)
            {

                var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/Versions/UpdateVersioning.json");
                UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
                data.Name = responseModel.Data.APIs[i].Name;
                data.ListenPath = $"/{responseModel.Data.APIs[i].Name}/";
                data.ApiId = responseModel.Data.APIs[i].ApiId;
                data.AuthType = "standard";
                Apidata.Add(data);
                // Update_Api
                var RequestJson1 = JsonConvert.SerializeObject(data);
                HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
                var response1 = await client.PutAsync("/api/v1/ApplicationGateway", content1);
                response1.EnsureSuccessStatusCode();
                Thread.Sleep(2000);
            }

            //create policy
            for (var i = 0; i < apiName.Count; i++)
            {
                var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/AccessControls/CreatePolicy-AllowedUrls.json");
                JObject keyValues = JObject.Parse(mypolicyJsonString);
                keyValues["name"] = Guid.NewGuid().ToString();
                JArray methods = new JArray();
                methods.Add("POST");
                foreach (var obj in keyValues["apIs"])
                {
                    obj["id"] = responseModel.Data.APIs[i].ApiId;
                    obj["name"] = responseModel.Data.APIs[i].Name; 
                    foreach(var obj1 in obj["allowedUrls"])
                    {
                        obj["methods"] = methods;
                    }
                }
                
                HttpContent Policycontent = new StringContent(keyValues.ToString(), Encoding.UTF8, "application/json");
                var PolicyResponse = await client.PostAsync("/api/v1/Policy", Policycontent);
                PolicyResponse.EnsureSuccessStatusCode();
                Thread.Sleep(2000);
            }

            for (var k = 0; k < apiName.Count; k++)
            {
                var clientkey = HttpClientFactory.Create();
                Url = ApplicationConstants.TYK_BASE_URL + apiName[k].ToString() + "/WeatherForecast";
                var responseclientkey = await clientkey.GetAsync(Url);
                responseclientkey.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.Unauthorized);//403 forbidden
            }

            //delete Api
            foreach (var obj in responseModel.Data.APIs)
            {
                var deleteResponse = await DeleteApi(obj.ApiId);
                deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            }

        }

        private async Task<HttpResponseMessage> DeleteApi(Guid id)
        {
            //var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/v1/ApplicationGateway/" + id);
            return response;
        }


    }
}
