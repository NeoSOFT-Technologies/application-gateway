using ApplicationGateway.API.IntegrationTests.Base;
using ApplicationGateway.API.IntegrationTests.Helper;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
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

namespace ApplicationGateway.API.IntegrationTests.Controller
{
    public class test : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient client = null;
        public test(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            client = _factory.CreateClient();
        }


        [Fact]
        public async Task Blacklisting()
        {
            ////var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";
            string OriginUrl = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/AccessRestrictionTest/createApiData.json");
            CreateApiCommand requestModel1 = JsonConvert.DeserializeObject<CreateApiCommand>(myJsonString);
            requestModel1.Name = newid.ToString();
            requestModel1.ListenPath = $"/{newid.ToString()}/";
            requestModel1.TargetUrl = ApplicationConstants.ORIGIN_IP_URL;

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateApiDto>>(jsonString.Result);
            var id = result.Data.ApiId;
            Thread.Sleep(2000);

            //getorigin
            var ipaddress = await getsOrigin(OriginUrl);
            var ip = ipaddress.origin;
            string[] values = ip.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
            }
            var ipvalue = values[0];
            //read update json file
            var myupdateJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/AccessRestrictionTest/updateApiData.json");
            UpdateApiCommand updaterequestModel1 = JsonConvert.DeserializeObject<UpdateApiCommand>(myupdateJsonString);
            updaterequestModel1.Name = newid.ToString();
            updaterequestModel1.ListenPath = $"/{newid.ToString()}/";
            updaterequestModel1.ApiId = new Guid(id.ToString());
            updaterequestModel1.Blacklist = new List<string>
            {
                ipvalue
            };

            //updateappi
            var updateRequestJson = JsonConvert.SerializeObject(updaterequestModel1);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            Thread.Sleep(2000);


            //downstream
            var downstreamResponse = await DownStream(Url);
            downstreamResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.Forbidden);

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Whitelisting()
        {
            ////var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";
            string OriginUrl = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/";


            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/AccessRestrictionTest/createApiData.json");
            CreateApiCommand requestModel1 = JsonConvert.DeserializeObject<CreateApiCommand>(myJsonString);
            requestModel1.Name = newid.ToString();
            requestModel1.ListenPath = $"/{newid.ToString()}/";
            requestModel1.TargetUrl = ApplicationConstants.ORIGIN_IP_URL;

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateApiDto>>(jsonString.Result);
            var id = result.Data.ApiId;
            Thread.Sleep(2000);

            //getorigin
            var ipaddress = await getsOrigin(OriginUrl);
            var ip = ipaddress.origin;
            string[] values = ip.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
            }
            var ipvalue = values[0];
            //read update json file
            var myupdateJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/AccessRestrictionTest/updateApiData.json");
            UpdateApiCommand updaterequestModel1 = JsonConvert.DeserializeObject<UpdateApiCommand>(myupdateJsonString);
            updaterequestModel1.Name = newid.ToString();
            updaterequestModel1.ListenPath = $"/{newid.ToString()}/";
            updaterequestModel1.ApiId = new Guid(id.ToString());
            updaterequestModel1.Whitelist = new List<string>
            {
                ipvalue
            };

            //updateappi
            var updateRequestJson = JsonConvert.SerializeObject(updaterequestModel1);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            Thread.Sleep(2000);


            //downstream
            var downstreamResponse = await DownStream(Url);
            downstreamResponse.EnsureSuccessStatusCode();

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RateLimiting()
        {

            ////var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/ControlandLimit/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateApiDto>>(jsonString.Result);
            var id = result.Data.ApiId;
            Thread.Sleep(3000);

            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/ControlandLimit/rateLimitData.json");
            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid}/";
            data.ApiId = id;
            data.TargetUrl = ApplicationConstants.TARGET_URL;

            // Update_Api
            var updateRequestJson = JsonConvert.SerializeObject(data);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            Thread.Sleep(5000);

            // downstream
            for (int i = 0; i < 8; i++)
            {
                var responseh = await DownStream(Url);
                responseh.EnsureSuccessStatusCode();
            }

            //check Rate Limit Exceed
            var checkresponse = await DownStream(Url);
            checkresponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.TooManyRequests);

            //Rate Limiting Reset
            //Thread.Sleep(10000);
            //client = HttpClientFactory.Create();
            //response = await client.GetAsync(Url);
            //response.EnsureSuccessStatusCode();


            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);

        }

        [Fact]
        public async Task CreateApi_ReturnsSuccessResult()
        {
            Console.WriteLine("test started");
            //var client = _factory.CreateClient();

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/CreateApiTest/createApiData.json");
            CreateApiCommand requestModel1 = JsonConvert.DeserializeObject<CreateApiCommand>(myJsonString);

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateApiDto>>(jsonString.Result);
            var id = result.Data.ApiId;
            Thread.Sleep(3000);

            //downstream
            var listenpath = requestModel1.ListenPath.Trim(new char[] { '/' });
            string Url = ApplicationConstants.TYK_BASE_URL + listenpath + "/WeatherForecast";
            var responseN = await DownStream(Url);
            responseN.EnsureSuccessStatusCode();

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);

        }

        [Fact]
        public async Task CreateMultipleApis_ReturnsSuccessResult()
        {
            //var client = _factory.CreateClient();
            string Url;
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/CreateApiTest/createMultipleApiData.json");

            CreateMultipleApisCommand requestModel1 = JsonConvert.DeserializeObject<CreateMultipleApisCommand>(myJsonString);


            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateMultipleApis", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateMultipleApisDto>>(jsonString.Result);
            var ApisList = result.Data.APIs;
            Thread.Sleep(3000);
            //downstream
            foreach (var item in requestModel1.APIs)
            {
                var path1 = item.ListenPath.Trim(new char[] { '/' });
                Url = ApplicationConstants.TYK_BASE_URL + path1 + "/WeatherForecast";
                var responseN = await DownStream(Url);
                responseN.EnsureSuccessStatusCode();
            }

            //delete Api
            foreach (MultipleApiModelDto obj in ApisList)
            {
                var deleteResponse = await DeleteApi(obj.ApiId);
                deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task ExpireTest()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createApiData.json");
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

            Thread.Sleep(4000);

            //read update json file
            var myupdateJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/KeyTest/updateApiData.json");
            UpdateApiCommand updaterequestModel1 = JsonConvert.DeserializeObject<UpdateApiCommand>(myupdateJsonString);
            updaterequestModel1.Name = newid.ToString();
            updaterequestModel1.ListenPath = $"/{newid}/";
            updaterequestModel1.ApiId = id;

            //update appi
            var updateRequestJson = JsonConvert.SerializeObject(updaterequestModel1);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();

            Thread.Sleep(2000);

            // read createkey json file
            var myJsonStringKey = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createKeyData.json");
            //Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(DateTime.Now.AddSeconds(60))).TotalSeconds;
            DateTime foo = DateTime.Now.AddSeconds(30);
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
            JObject keyrequestmodel = JObject.Parse(myJsonStringKey);
            foreach (var item in keyrequestmodel["AccessRights"])
            {
                item["ApiId"] = id.ToString();
                item["ApiName"] = newid.ToString();
            }
            keyrequestmodel["Expires"] = unixTime;
            StringContent stringContent = new StringContent(keyrequestmodel.ToString(), System.Text.Encoding.UTF8, "application/json");

            //create key
            var responsekey = await client.PostAsync("/api/v1/Key/CreateKey", stringContent);
            responsekey.EnsureSuccessStatusCode();
            var jsonStringkey = await responsekey.Content.ReadAsStringAsync();
            JObject key = JObject.Parse(jsonStringkey);
            var keyid = key["data"]["keyId"];

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



        }

        [Fact]
        public async Task KeyRateLimiting()
        {
            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createApiData.json");
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

            Thread.Sleep(4000);

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
            Thread.Sleep(2000);

            //read json file 
            var myJsonStringKey = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createKeyData.json");
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
        }

        [Fact]
        public async Task Should_create_key()
        {
            //var client = _factory.CreateClient();
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
            Thread.Sleep(3000);
            var jsonString = response.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<Response<CreateMultipleApisDto>>(jsonString.Result);

            //read craatekey json file 
            var myJsonStringKey = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createKeyData.json");
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
            for (var i = 0; i < responseModel.Data.APIs.Count; i++)
            {
                accessRight.Add(new JObject(
                new JProperty("ApiId", responseModel.Data.APIs[i].ApiId),
                new JProperty("ApiName", apiName[i]),
                new JProperty("Versions", jarrayObj),
                new JProperty("AllowedUrls", jarrayObj1)));
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
                var clientkey = HttpClientFactory.Create();
                clientkey.DefaultRequestHeaders.Add("Authorization", keyid.ToString());
                Url = ApplicationConstants.TYK_BASE_URL + item.ToString() + "/WeatherForecast";
                var responseclientkey = await clientkey.GetAsync(Url);
                var check = responseclientkey.EnsureSuccessStatusCode();
            }


            //delete Api
            foreach (var item in accessRight)
            {
                var deleteResponse = await DeleteApi(((new Guid((item["ApiId"].ToString())))));
                deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);

            }
        }
        [Fact]
        public async Task quota_with_key()
        {
            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";
            string versioncheck = "";
            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createApiData.json");
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
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/KeyTest/updateApiData.json");
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
            var myJsonStringKey = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createKeyData.json");
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

            for (var i = 0; i < 3; i++)
            {
                var clientV = HttpClientFactory.Create();
                clientV.DefaultRequestHeaders.Add("Authorization", keyid.ToString());
                var responseV = await clientV.GetAsync(Url);
                responseV.EnsureSuccessStatusCode();
            }

            var client1 = HttpClientFactory.Create();
            client1.DefaultRequestHeaders.Add("Authorization", keyid.ToString());
            var responseT = await client1.GetAsync(Url);
            responseT.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.TooManyRequests);

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);

        }

        [Fact]
        public async Task Key_Versioning_byHeader()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";
            string versioncheck = "";
            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createApiData.json");
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
            var myJsonStringKey = File.ReadAllText(ApplicationConstants.BASE_PATH + "/keyTest/createKeyData.json");
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
            var keyid = key["data"]["keyId"];

            foreach (UpdateVersionModel obj in data.Versions)
            {
                var clientV = HttpClientFactory.Create();
                clientV.DefaultRequestHeaders.Add("Authorization", keyid.ToString());
                clientV.DefaultRequestHeaders.Add(data.VersioningInfo.Key, obj.Name);
                if (versioncheck == obj.Name)
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

        [Fact]
        public async Task loadBalancing()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/LoadBalancingTest/createApiData.json");
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
            Thread.Sleep(3000);

            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/LoadBalancingTest/loadBalancingData.json");

            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;
            data.TargetUrl = ApplicationConstants.TARGET_URL;

            // Update_Api
            var updateRequestJson = JsonConvert.SerializeObject(data);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            Thread.Sleep(5000);


            // downstream
            for (int i = 1; i < 8; i++)
            {
                var responseN = await DownStream(Url);
                responseN.EnsureSuccessStatusCode();
            }

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Add_policy_with_Api_AllowedUrls()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

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
            Thread.Sleep(2000);

            //Update standard authentication
            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/AccessControls/AddAuthentication.json");
            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;

            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/ApplicationGateway", content1);
            response1.EnsureSuccessStatusCode();

            //create policy
            var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/AccessControls/CreatePolicy-AllowedUrls.json");
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
            Thread.Sleep(2000);

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
            Thread.Sleep(2000);

            // JObject key = JObject.Parse(jsonStringkey);
            // var key_Id = (key["data"]["keyId"]).ToString();

            //downstream api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("Authorization", keyId);

            Thread.Sleep(5000);
            var responseclientkey = await clientkey.GetAsync(Url);
            responseclientkey.EnsureSuccessStatusCode();//403 forbidden


            //delete Api,policy,key
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletePolicyResponse = await DeletePolicy(policyId);
            deletePolicyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletekeyResponse = await DeleteKey(keyId);
            deletekeyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);


        }

        [Fact]
        public async Task Add_policy_with_Quotas_returnSuccess()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

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
            Thread.Sleep(2000);

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
            var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/ControlandLimit/CreatePolicy-Quotas.json");
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
            Thread.Sleep(2000);

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
            Thread.Sleep(2000);

            //downstream api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("Authorization", keyId);

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
            var deletePolicyResponse = await DeletePolicy(policyId);
            deletePolicyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletekeyResponse = await DeleteKey(keyId);
            deletekeyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);

        }

        [Fact]
        public async Task Add_policy_with_Api_Quotas_returnSuccess()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

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
            Thread.Sleep(2000);

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
            var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/ControlandLimit/CreatePolicy-Api_Quotas.json");
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
            Thread.Sleep(2000);

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
            Thread.Sleep(2000);

            //downstream api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("Authorization", keyId);

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
            var deletePolicyResponse = await DeletePolicy(policyId);
            deletePolicyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletekeyResponse = await DeleteKey(keyId);
            deletekeyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);

        }

        [Fact]
        public async Task Add_policy_with_RateLimit_returnSuccess()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

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
            Thread.Sleep(2000);

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
            ;

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
            Thread.Sleep(2000);

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
            Thread.Sleep(2000);

            //downstream api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("Authorization", keyId);

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
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

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
            Thread.Sleep(2000);

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
            Thread.Sleep(2000);

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
            Thread.Sleep(2000);


            //downstream api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("Authorization", keyId);

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

        [Fact]
        public async Task Add_policy_with_Throttling_returnSuccess()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

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
            Thread.Sleep(2000);

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
            var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/ControlandLimit/CreatePolicy-Throttling.json");
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
            Thread.Sleep(2000);

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
            Thread.Sleep(2000);
            //downstream api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("Authorization", keyId);

            Thread.Sleep(5000);
            for (int i = 1; i < 4; i++)
            {

                var responseclientkey = await clientkey.GetAsync(Url);
                responseclientkey.EnsureSuccessStatusCode();

            }

            var responseclientkey1 = await clientkey.GetAsync(Url);
            responseclientkey1.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.OK);

            //delete Api,policy,key
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletePolicyResponse = await DeletePolicy(policyId);
            deletePolicyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletekeyResponse = await DeleteKey(keyId);
            deletekeyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);

        }

        [Fact]
        public async Task Add_policy_with_Api_Throttling_returnSuccess()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

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
            Thread.Sleep(2000);

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
            var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/ControlandLimit/CreatePolicy-Api_Throttling.json");
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
            Thread.Sleep(2000);

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
            Thread.Sleep(2000);

            //downstream api
            var clientkey = HttpClientFactory.Create();
            clientkey.DefaultRequestHeaders.Add("Authorization", keyId);

            Thread.Sleep(5000);
            for (int i = 1; i < 4; i++)
            {

                var responseclientkey = await clientkey.GetAsync(Url);
                responseclientkey.EnsureSuccessStatusCode();

            }

            var responseclientkey1 = await clientkey.GetAsync(Url);
            responseclientkey1.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.OK);

            //delete Api,policy,key
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletePolicyResponse = await DeletePolicy(policyId);
            deletePolicyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletekeyResponse = await DeleteKey(keyId);
            deletekeyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);

        }


        [Fact]
        public async Task Add_policy_with_Api_VersionAccess_returnSuccess()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

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
            Thread.Sleep(2000);

            //Update standard authentication and version info.
            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/Versions/UpdateVersioning.json");
            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;

            // Update_Api
            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/ApplicationGateway", content1);
            response1.EnsureSuccessStatusCode();

            //create policy
            var mypolicyJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/PolicyData/Versions/CreatePolicy-Api_versions.json");
            JObject keyValues = JObject.Parse(mypolicyJsonString);
            keyValues["name"] = Guid.NewGuid().ToString();
            var versionPol = "";
            foreach (var obj in keyValues["apIs"])
            {
                obj["id"] = id;
                obj["name"] = newid.ToString();
                versionPol = (obj["versions"][0]).ToString();
            }

            HttpContent Policycontent = new StringContent(keyValues.ToString(), Encoding.UTF8, "application/json");
            var PolicyResponse = await client.PostAsync("/api/v1/Policy", Policycontent);
            PolicyResponse.EnsureSuccessStatusCode();
            var PolicyjsonString = PolicyResponse.Content.ReadAsStringAsync();
            var Policyresult = JsonConvert.DeserializeObject<Response<CreatePolicyDto>>(PolicyjsonString.Result);

            var policyId = Policyresult.Data.PolicyId;
            Thread.Sleep(2000);

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
            Thread.Sleep(2000);


            //downstream api
            /* var clientkey = HttpClientFactory.Create();
             clientkey.DefaultRequestHeaders.Add("Authorization", keyId.ToString());
 */
            Thread.Sleep(5000);

            foreach (UpdateVersionModel obj in data.Versions)
            {

                var clientV = HttpClientFactory.Create();
                clientV.DefaultRequestHeaders.Add("Authorization", keyId.ToString());
                clientV.DefaultRequestHeaders.Add(data.VersioningInfo.Key, obj.Name);
                var responseV = await clientV.GetAsync(Url);
                if (obj.Name == versionPol)
                    responseV.EnsureSuccessStatusCode();
                else
                    responseV.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.Forbidden);
            }


            //delete Api,policy,key
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletePolicyResponse = await DeletePolicy(policyId);
            deletePolicyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            var deletekeyResponse = await DeleteKey(keyId);
            deletekeyResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);

        }

        [Fact]
        public async Task Versioning_byHeader()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/Versioning/createApiData.json");
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
            Thread.Sleep(3000);

            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/Versioning/Header_Version.json");
            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;

            // Update_Api
            var updateRequestJson = JsonConvert.SerializeObject(data);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            Thread.Sleep(5000);

            foreach (UpdateVersionModel obj in data.Versions)
            {
                var clientV = HttpClientFactory.Create();
                clientV.DefaultRequestHeaders.Add(data.VersioningInfo.Key, obj.Name);
                var responseV = await clientV.GetAsync(Url);
                responseV.EnsureSuccessStatusCode();
            }

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Versioning_byQueryParam()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url;

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/Versioning/createApiData.json");
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
            Thread.Sleep(3000);

            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/Versioning/QueryParam_Version.json");

            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;

            // Update_Api
            var updateRequestJson = JsonConvert.SerializeObject(data);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            Thread.Sleep(5000);


            //downstream
            foreach (UpdateVersionModel obj in data.Versions)
            {
                Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + $"/WeatherForecast?{data.VersioningInfo.Key}={obj.Name}";
                var responseV = await DownStream(Url);
                responseV.EnsureSuccessStatusCode();
            }

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Versioning_byUrl()
        {

            //var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = "";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH + "/Versioning/createApiData.json");
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
            Thread.Sleep(3000);

            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH + "/Versioning/Url_Version.json");
            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;

            // Update_Api
            var updateRequestJson = JsonConvert.SerializeObject(data);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            Thread.Sleep(5000);

            //downstream
            foreach (UpdateVersionModel version in data.Versions)
            {
                Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/" + version.Name + "/WeatherForecast";
                var responseV = await DownStream(Url);
                responseV.EnsureSuccessStatusCode();
                Thread.Sleep(2000);
            }

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
        public async Task<Docker> getsOrigin(string path)
        {
            var client = HttpClientFactory.Create();
            var response = await client.GetAsync(path);
            var jsonString = response.Content.ReadAsStringAsync();
            Docker result = JsonConvert.DeserializeObject<Docker>(jsonString.Result);
            return result;
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
