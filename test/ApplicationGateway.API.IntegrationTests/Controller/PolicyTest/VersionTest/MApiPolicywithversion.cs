using ApplicationGateway.API.IntegrationTests.Base;
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


namespace ApplicationGateway.API.IntegrationTests.Controller.PolicyTest.VersionTest
{
    public class MApiPolicywithversion : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient client = null;
        public MApiPolicywithversion(CustomWebApplicationFactory factory)
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

            
            List<UpdateApiCommand> Apidata =  new List<UpdateApiCommand>();
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
            var versionPol = new List<string>();
            var versioncheck = new List<string>();
            for (var i = 0; i < apiName.Count; i++)
            {
                var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/Versions/CreatePolicy-MApi_versions.json");
                JObject keyValues = JObject.Parse(mypolicyJsonString);
                keyValues["name"] = Guid.NewGuid().ToString();
                versionPol = new List<string>()
                {
                    "V1","V2"
                };
                JArray version = new JArray();
                version.Add(versionPol[i]);
                foreach (var obj in keyValues["apIs"])
                {
                    obj["id"] = responseModel.Data.APIs[i].ApiId;
                    obj["name"] = responseModel.Data.APIs[i].Name;
                    obj["versions"] = version;
                    versioncheck.Add((version[0]).ToString());
                }
                
                HttpContent Policycontent = new StringContent(keyValues.ToString(), Encoding.UTF8, "application/json");
                var PolicyResponse = await client.PostAsync("/api/v1/Policy", Policycontent);
                PolicyResponse.EnsureSuccessStatusCode();
                var PolicyjsonString = PolicyResponse.Content.ReadAsStringAsync();
                var Policyresult = JsonConvert.DeserializeObject<Response<CreatePolicyDto>>(PolicyjsonString.Result);

                policyIds.Add(Policyresult.Data.PolicyId);
                Thread.Sleep(2000);
                version.RemoveAll();
            }

            //create key for policy's
            var myKeyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/CreatePolicyKey.json");
            CreateKeyCommand keyrequestmodel = JsonConvert.DeserializeObject<CreateKeyCommand>(myKeyJsonString);

            //set policyId
            for (var i = 0; i < apiName.Count; i++)
            {
                keyrequestmodel.Policies.Add(policyIds[i].ToString());

            }

            //create key
            var keyRequestJson = JsonConvert.SerializeObject(keyrequestmodel);
            HttpContent keycontent = new StringContent(keyRequestJson, Encoding.UTF8, "application/json");
            var responsekey = await client.PostAsync("/api/v1/Key/CreateKey", keycontent);
            responsekey.EnsureSuccessStatusCode();
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            var keyresult = JsonConvert.DeserializeObject<Response<Key>>(jsonStringkey);
            var keyId = keyresult.Data.KeyId;
            Thread.Sleep(4000);

            //downstream api
            
            for (var k = 0; k < apiName.Count; k++)
            {
                for (var j = 0; j < versionPol.Count; j++)
                {
                    var clientkey = HttpClientFactory.Create();
                    clientkey.DefaultRequestHeaders.Add("Authorization", keyId);
                    Url = ApplicationConstants.TYK_BASE_URL + apiName[k].ToString() + "/WeatherForecast";
                    clientkey.DefaultRequestHeaders.Add(Apidata[j].VersioningInfo.Key, versionPol[j]);
                    var responseV = await clientkey.GetAsync(Url);
                    if (versioncheck[k] == versionPol[j])
                        responseV.EnsureSuccessStatusCode();
                    else
                        responseV.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.Forbidden);
                   
                }
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


        private async Task<HttpResponseMessage> DeletePolicy(Guid id)
        {
            //var client = _factory.CreateClient();
            var response = await client.DeleteAsync("api/v1/Policy/" + id);
            return response;
        }


        private async Task<HttpResponseMessage> DeleteKey(string id)
        {
            //var client = _factory.CreateClient();
            var response = await client.DeleteAsync("api/v1/Key/DeleteKey?keyId=" + id);
            return response;
        }
    }
}
