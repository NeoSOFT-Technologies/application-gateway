﻿using ApplicationGateway.API.IntegrationTests.Base;
using ApplicationGateway.API.IntegrationTests.Helper;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.GatewayCommon;
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

namespace ApplicationGateway.API.IntegrationTests.Controller.PolicyTest.ControlAndLimit
{
    [Collection("Database")]
    public partial class PolicyRateLimitTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient client = null;
        public PolicyRateLimitTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            client = _factory.CreateClient();
        }

        [Fact]
        public async Task Add_policy_with_RateLimit_returnSuccess()
        {
            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/createApiData.json");
            CreateApiCommand requestModel1 = JsonConvert.DeserializeObject<CreateApiCommand>(myJsonString);
            requestModel1.Name = newid.ToString();
            requestModel1.ListenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateApiDto>>(jsonString.Result);
            var id = result.Data.ApiId;
            Thread.Sleep(5000);

            //Update standard authentication
            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/ControlandLimit/AddAuthentication.json");
            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;

            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/ApplicationGateway", content1);
            response1.EnsureSuccessStatusCode();
            

            //create policy
            var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/ControlandLimit/CreatePolicy-rateLimit.json");
            JObject keyValues = JObject.Parse(mypolicyJsonString);
            keyValues["name"] = Guid.NewGuid().ToString();
            foreach (var obj in keyValues["apIs"])
            {
                obj["id"] = id;
                obj["name"] = newid.ToString();

            }
            HttpContent Policycontent = new StringContent(keyValues.ToString(), Encoding.UTF8, "application/json");
            var PolicyResponse = await client.PostAsync("/api/v1/Policy", Policycontent);
            PolicyResponse.EnsureSuccessStatusCode();
            var PolicyjsonString = PolicyResponse.Content.ReadAsStringAsync();
            var Policyresult = JsonConvert.DeserializeObject<Response<CreatePolicyDto>>(PolicyjsonString.Result);

            var policyId = Policyresult.Data.PolicyId;
            Thread.Sleep(5000);

            //create key for policy
            var myKeyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/CreatePolicyKey.json");
            CreateKeyCommand keyrequestmodel = JsonConvert.DeserializeObject<CreateKeyCommand>(myKeyJsonString);

            //set policyId
            keyrequestmodel.Policies = new List<string>() { policyId.ToString() };

            //create key
            var keyRequestJson = JsonConvert.SerializeObject(keyrequestmodel);
            HttpContent keycontent = new StringContent(keyRequestJson, Encoding.UTF8, "application/json");
            var responsekey = await client.PostAsync("/api/v1/Key/CreateKey", keycontent);
            responsekey.EnsureSuccessStatusCode();
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            var keyresult = JsonConvert.DeserializeObject<Response<Key>>(jsonStringkey);
            var keyId = keyresult.Data.KeyId;
            Thread.Sleep(5000);

            //downstream api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("gateway-authorization", keyId);

            Thread.Sleep(5000);
            for (int i = 1; i < 4; i++)
            {

                var responseclientkey = await clientkey.GetAsync(Url);
                responseclientkey.EnsureSuccessStatusCode();

            }

            //CHECK RATE LIMITING 
            var responseclientkey1 = await clientkey.GetAsync(Url);
            responseclientkey1.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.TooManyRequests);

            //delete Api,policy,key
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletePolicyResponse = await DeletePolicy(policyId);
            deletePolicyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletekeyResponse = await DeleteKey(keyId);
            deletekeyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Add_policy_with_Api_RateLimit_returnSuccess()
        {
            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/createApiData.json");
            CreateApiCommand requestModel1 = JsonConvert.DeserializeObject<CreateApiCommand>(myJsonString);
            requestModel1.Name = newid.ToString();
            requestModel1.ListenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateApiDto>>(jsonString.Result);
            var id = result.Data.ApiId;
            Thread.Sleep(5000);

            //Update standard authentication
            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/ControlandLimit/AddAuthentication.json");
            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;

            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/ApplicationGateway", content1);
            response1.EnsureSuccessStatusCode();


            //create policy
            var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/ControlandLimit/CreatePolicy-Api_RateLimit.json");
            JObject keyValues = JObject.Parse(mypolicyJsonString);
            keyValues["name"] = Guid.NewGuid().ToString();
            foreach (var obj in keyValues["apIs"])
            {
                obj["id"] = id;
                obj["name"] = newid.ToString();

            }
            HttpContent Policycontent = new StringContent(keyValues.ToString(), Encoding.UTF8, "application/json");
            var PolicyResponse = await client.PostAsync("/api/v1/Policy", Policycontent);
            PolicyResponse.EnsureSuccessStatusCode();
            var PolicyjsonString = PolicyResponse.Content.ReadAsStringAsync();
            var Policyresult = JsonConvert.DeserializeObject<Response<CreatePolicyDto>>(PolicyjsonString.Result);

            var policyId = Policyresult.Data.PolicyId;
            Thread.Sleep(5000);

            //create key for policy
            var myKeyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/CreatePolicyKey.json");
            CreateKeyCommand keyrequestmodel = JsonConvert.DeserializeObject<CreateKeyCommand>(myKeyJsonString);

            //set policyId
            keyrequestmodel.Policies = new List<string>() { policyId.ToString() };

            //create key
            var keyRequestJson = JsonConvert.SerializeObject(keyrequestmodel);
            HttpContent keycontent = new StringContent(keyRequestJson, Encoding.UTF8, "application/json");
            var responsekey = await client.PostAsync("/api/v1/Key/CreateKey", keycontent);
            responsekey.EnsureSuccessStatusCode();
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            var keyresult = JsonConvert.DeserializeObject<Response<Key>>(jsonStringkey);
            var keyId = keyresult.Data.KeyId;
            Thread.Sleep(5000);


            //downstream api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("gateway-authorization", keyId);

            Thread.Sleep(5000);
            for (int i = 0; i < 5; i++)
            {

                var responseclientkey = await clientkey.GetAsync(Url);
                responseclientkey.EnsureSuccessStatusCode();

            }

            //CHECK RATE LIMITING 
            var responseclientkey1 = await clientkey.GetAsync(Url);
            responseclientkey1.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.TooManyRequests);

            //delete Api,policy,key
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletePolicyResponse = await DeletePolicy(policyId);
            deletePolicyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletekeyResponse = await DeleteKey(keyId);
            deletekeyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);

        }



        private async Task<HttpResponseMessage> DeleteApi(Guid id)
        {
            //var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/v1/ApplicationGateway/" + id);
            // await HotReload();
            return response;
        }


        private async Task<HttpResponseMessage> DeletePolicy(Guid id)
        {
            //var client = _factory.CreateClient();
            var response = await client.DeleteAsync("api/v1/Policy/" + id);
            //   await HotReload();
            return response;
        }


        private async Task<HttpResponseMessage> DeleteKey(string id)
        {
            //var client = _factory.CreateClient();
            var response = await client.DeleteAsync("api/v1/Key/DeleteKey?keyId=" + id);
            // await HotReload();
            return response;
        }


    }

}
