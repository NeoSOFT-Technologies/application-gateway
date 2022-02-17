using ApplicationGateway.API.IntegrationTests.Base;
using ApplicationGateway.API.IntegrationTests.Helper;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Application.Responses;
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
    public class PolicyQuotasTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public PolicyQuotasTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }




        [Fact]
        public async Task Add_policy_with_Quotas_returnSuccess()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);
            var id = result.key;
            await HotReload();

            //Update standard authentication
            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/ControlandLimit/AddAuthentication.json");
            UpdateRequest data = JsonConvert.DeserializeObject<UpdateRequest>(myJsonString1);
            data.name = newid.ToString();
            data.listenPath = $"/{newid.ToString()}/";
            data.id = Guid.Parse(id);

            // data.authType = "standard";
            // Update_Api
            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/ApplicationGateway/UpdateApi/updateapi", content1);
            response1.EnsureSuccessStatusCode();
            await HotReload();

            //create policy
            var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/ControlandLimit/CreatePolicy-Quotas.json");
            JObject keyValues = JObject.Parse(mypolicyJsonString);
            keyValues["name"] = Guid.NewGuid().ToString();
            foreach (var obj in keyValues["apIs"])
            {
                obj["id"] = id;
                obj["name"] = newid.ToString();

            }
            //create Api

            HttpContent Policycontent = new StringContent(keyValues.ToString(), Encoding.UTF8, "application/json");
            var PolicyResponse = await client.PostAsync("/api/v1/Policy", Policycontent);
            PolicyResponse.EnsureSuccessStatusCode();
            var PolicyjsonString = PolicyResponse.Content.ReadAsStringAsync();
            var Policyresult = JsonConvert.DeserializeObject<Response<CreatePolicyDto>>(PolicyjsonString.Result);

            var policyId = Policyresult.Data.PolicyId;
            await HotReload();
            Thread.Sleep(2000);

            //create key for policy
            var myKeyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/CreatePolicyKey.json");
            CreateKeyRequest keyrequestmodel = JsonConvert.DeserializeObject<CreateKeyRequest>(myKeyJsonString);

            //set policyId
            keyrequestmodel.policyId = new List<string>() { policyId.ToString() };

            //create key
            var keyRequestJson = JsonConvert.SerializeObject(keyrequestmodel);
            HttpContent keycontent = new StringContent(keyRequestJson, Encoding.UTF8, "application/json");
            var responsekey = await client.PostAsync("/api/Key/CreateKey", keycontent);
            responsekey.EnsureSuccessStatusCode();
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            JObject key = JObject.Parse(jsonStringkey);

            var keyid = key["key"];

            //downstream api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("Authorization", keyid.ToString());

            Thread.Sleep(5000);
            for (int i = 0; i < 5; i++)
            {

                var responseclientkey = await clientkey.GetAsync(Url);
                responseclientkey.EnsureSuccessStatusCode();

            }

            var responseclientkey1 = await clientkey.GetAsync(Url);
            responseclientkey1.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.OK);

            //delete Api,policy,key
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();
            var deletePolicyResponse = await DeletePolicy(policyId);
            deletePolicyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();
            var deletekeyResponse = await DeleteKey(keyid.ToString());
            deletekeyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.OK);
            await HotReload();

        }

        [Fact]
        public async Task Add_policy_with_Api_Quotas_returnSuccess()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);
            var id = result.key;
            await HotReload();

            //Update standard authentication
            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/AddAuthentication.json");
            UpdateRequest data = JsonConvert.DeserializeObject<UpdateRequest>(myJsonString1);
            data.name = newid.ToString();
            data.listenPath = $"/{newid.ToString()}/";
            data.id = Guid.Parse(id);

            // data.authType = "standard";
            // Update_Api
            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/ApplicationGateway/UpdateApi/updateapi", content1);
            response1.EnsureSuccessStatusCode();
            await HotReload();

            //create policy
            var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/CreatePolicy-Api_Quotas.json");
            JObject keyValues = JObject.Parse(mypolicyJsonString);
            keyValues["name"] = Guid.NewGuid().ToString();
            foreach (var obj in keyValues["apIs"])
            {
                obj["id"] = id;
                obj["name"] = newid.ToString();

            }
            //create Api

            HttpContent Policycontent = new StringContent(keyValues.ToString(), Encoding.UTF8, "application/json");
            var PolicyResponse = await client.PostAsync("/api/v1/Policy", Policycontent);
            PolicyResponse.EnsureSuccessStatusCode();
            var PolicyjsonString = PolicyResponse.Content.ReadAsStringAsync();
            var Policyresult = JsonConvert.DeserializeObject<Response<CreatePolicyDto>>(PolicyjsonString.Result);

            var policyId = Policyresult.Data.PolicyId;
            await HotReload();
            Thread.Sleep(2000);

            //create key for policy
            var myKeyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/CreatePolicyKey.json");
            CreateKeyRequest keyrequestmodel = JsonConvert.DeserializeObject<CreateKeyRequest>(myKeyJsonString);

            //set policyId
            keyrequestmodel.policyId = new List<string>() { policyId.ToString() };

            //create key
            var keyRequestJson = JsonConvert.SerializeObject(keyrequestmodel);
            HttpContent keycontent = new StringContent(keyRequestJson, Encoding.UTF8, "application/json");
            var responsekey = await client.PostAsync("/api/Key/CreateKey", keycontent);
            responsekey.EnsureSuccessStatusCode();
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            JObject key = JObject.Parse(jsonStringkey);

            var keyid = key["key"];

            //downstream api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("Authorization", keyid.ToString());

            Thread.Sleep(5000);
            for (int i = 0; i < 11; i++)
            {

                var responseclientkey = await clientkey.GetAsync(Url);
                responseclientkey.EnsureSuccessStatusCode();

            }

            var responseclientkey1 = await clientkey.GetAsync(Url);
            responseclientkey1.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.Forbidden);

            //delete Api,policy,key
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();
            //var deletePolicyResponse = await DeletePolicy(policyId);
            //deletePolicyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();
            var deletekeyResponse = await DeleteKey(keyid.ToString());
            deletekeyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.OK);
            await HotReload();

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


        private async Task<HttpResponseMessage> DeletePolicy(Guid id)
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("api/v1/Policy/" + id);
            await HotReload();
            return response;
        }


        private async Task<HttpResponseMessage> DeleteKey(string id)
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("api/Key/DeleteKey?keyId=" + id);
            await HotReload();
            return response;
        }


    }

}
