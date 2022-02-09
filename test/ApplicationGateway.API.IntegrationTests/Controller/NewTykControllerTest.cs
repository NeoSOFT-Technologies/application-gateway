using ApplicationGateway.API.IntegrationTests.Base;
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
    public class NewTykControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        public NewTykControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }



        [Fact]
        public async Task Post_CreateApi_ReturnsSuccessResult()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText("../../../JsonData/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/NewTyk/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);

            var id = result.key;
            await HotReload();
            Thread.Sleep(4000);

            //downstream
            var responseN = await DownStream(Url);
            //   responseN.EnsureSuccessStatusCode();

            //delete Api
            var deleteResponse = await DeleteApi(id);  // await client.DeleteAsync("/api/NewTyk/deleteApi?apiId=" + requestModel1.api_id);//await DeleteApi(Request.api_id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();

        }



        [Fact]
        public async Task Post_CreateMultipleApis_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();
            Guid newid;
            IList<string> path = new List<string>();
            string Url = "";
            IList<CreateRequest> requestModel1 = new List<CreateRequest>();
            var myJsonString = File.ReadAllText("../../../JsonData/createMultipleApiData.json");
            requestModel1 = JsonConvert.DeserializeObject<List<CreateRequest>>(myJsonString);

            foreach (CreateRequest obj in requestModel1)
            {
                newid = Guid.NewGuid();
                obj.name = newid.ToString();
                obj.listenPath = $"/{newid.ToString()}/";
                path.Add(newid.ToString());
            }

            //create Apis
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/NewTyk/CreateMultipleApi/createMultipleApi", content);

            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            IList<ResponseModel> responseModel = new List<ResponseModel>();
            responseModel = JsonConvert.DeserializeObject<List<ResponseModel>>(jsonString.Result);

            await HotReload();
            Thread.Sleep(7000);


            foreach (var item in path)
            {
                //downstream
                Url = $"http://localhost:8080/" + item + "/WeatherForecast";
                /* var clientH = HttpClientFactory.Create();
                 var responseN = await clientH.GetAsync(Url);*/
                var responseN = await DownStream(Url);
                //  responseN.EnsureSuccessStatusCode();
            }

            var id = "";
            foreach (ResponseModel obj in responseModel)
            {
                //delete Api
                id = obj.key;
                var deleteResponse = await DeleteApi(id);
                deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
                await HotReload();
            }



        }


        [Fact]
        public async Task Update_Versioning_byHeader()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText("../../../JsonData/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/NewTyk/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);

            var id = result.key;
            await HotReload();
            // Thread.Sleep(4000);

            //Read Json
            var myJsonString1 = File.ReadAllText("../../../JsonData/masterVersion.json");

            UpdateRequest data =
                JsonConvert.DeserializeObject<UpdateRequest>(myJsonString1);
            data.name = newid.ToString();
            data.listenPath = $"/{newid.ToString()}/";
            data.id = Guid.Parse(id);

            data.defaultVersion = "V1";

            data.versioningInfo.location = "header";
            data.versioningInfo.key = "x-tyk-api-version";
            data.versioningInfo.strip_path = false;
            // Update_Api
            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/NewTyk/UpdateApi/updateapi", content1);
            response1.EnsureSuccessStatusCode();
            await HotReload();
            Thread.Sleep(5000);

            //  for Version1
            /* var clientV = HttpClientFactory.Create();
             clientV.DefaultRequestHeaders.Add(data.versioningInfo.key, data.defaultVersion);
             var responseV = await clientV.GetAsync(Url);
             responseV.EnsureSuccessStatusCode();*/

            foreach (VersionModel obj in data.versions)
            {
                var clientV = HttpClientFactory.Create();
                clientV.DefaultRequestHeaders.Add(data.versioningInfo.key, obj.name);
                var responseV = await clientV.GetAsync(Url);
                responseV.EnsureSuccessStatusCode();
            }

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();

        }

        [Fact]
        public async Task Update_Versioning_byQueryParam()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = "";

            //read json file 
            var myJsonString = File.ReadAllText("../../../JsonData/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/NewTyk/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);

            var id = result.key;
            await HotReload();
            // Thread.Sleep(4000);

            //Read Json
            var myJsonString1 = File.ReadAllText("../../../JsonData/masterVersion.json");

            UpdateRequest data =
                JsonConvert.DeserializeObject<UpdateRequest>(myJsonString1);
            data.name = newid.ToString();
            data.listenPath = $"/{newid.ToString()}/";
            data.id = Guid.Parse(id);

            data.defaultVersion = "V1";

            data.versioningInfo.location = "url-param";
            data.versioningInfo.key = "x-tyk-api-version";
            data.versioningInfo.strip_path = false;
            // Update_Api
            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/NewTyk/UpdateApi/updateapi", content1);
            response1.EnsureSuccessStatusCode();
            await HotReload();
            Thread.Sleep(5000);


            //downstream
            foreach (VersionModel obj in data.versions)
            {
                Url = $"http://localhost:8080/" + newid.ToString() + $"/WeatherForecast?{data.versioningInfo.key}={obj.name}";
                var responseV = await DownStream(Url);
                responseV.EnsureSuccessStatusCode();
            }

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();

        }

        [Fact]
        public async Task Update_Versioning_byUrl()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = "";

            //read json file 
            var myJsonString = File.ReadAllText("../../../JsonData/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/NewTyk/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);

            var id = result.key;
            await HotReload();
            // Thread.Sleep(4000);

            //Read Json
            var myJsonString1 = File.ReadAllText("../../../JsonData/masterVersion.json");

            UpdateRequest data =
                JsonConvert.DeserializeObject<UpdateRequest>(myJsonString1);
            data.name = newid.ToString();
            data.listenPath = $"/{newid.ToString()}/";
            data.id = Guid.Parse(id);

            data.defaultVersion = "V1";

            data.versioningInfo.location = "url";
            data.versioningInfo.key = "";
            data.versioningInfo.strip_path = true;
            // Update_Api
            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/NewTyk/UpdateApi/updateapi", content1);
            response1.EnsureSuccessStatusCode();
            await HotReload();
            Thread.Sleep(5000);


            //downstream
            foreach (VersionModel version in data.versions)
            {
                Url = $"http://localhost:8080/" + newid.ToString() + "/" + version.name + "/WeatherForecast";
                var responseV = await DownStream(Url);
                responseV.EnsureSuccessStatusCode();
                Thread.Sleep(2000);
            }

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();

        }

        [Fact]
        public async Task Update_loadBalancing()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText("../../../JsonData/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/NewTyk/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);

            var id = result.key;
            await HotReload();
            // Thread.Sleep(4000);

            //Read Json
            var myJsonString1 = File.ReadAllText("../../../JsonData/masterJson.json");

            UpdateRequest data =
                JsonConvert.DeserializeObject<UpdateRequest>(myJsonString1);
            data.name = newid.ToString();
            data.listenPath = $"/{newid.ToString()}/";
            data.id = Guid.Parse(id);
            data.targetUrl = "http://host.docker.internal:5000";
            data.loadBalancingTargets = new List<string>() {
                "http://host.docker.internal:5001",
                "http://host.docker.internal:5000" };


            // Update_Api
            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/NewTyk/UpdateApi/updateapi", content1);
            response1.EnsureSuccessStatusCode();
            await HotReload();
            Thread.Sleep(5000);


            // Thread.Sleep(5000);

            // downstream
            for (int i = 1; i < 8; i++)
            {
                /* var clientN = HttpClientFactory.Create();
                 var responseN = await clientN.GetAsync(Url);*/
                var responseN = await DownStream(Url);
                responseN.EnsureSuccessStatusCode();
            }

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();

        }


        [Fact]
        public async Task Update_rateLimiting()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText("../../../JsonData/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/NewTyk/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);

            var id = result.key;
            await HotReload();
            // Thread.Sleep(4000);

            //Read Json
            var myJsonString1 = File.ReadAllText("../../../JsonData/masterJson.json");

            UpdateRequest data =
                JsonConvert.DeserializeObject<UpdateRequest>(myJsonString1);
            data.name = newid.ToString();
            data.listenPath = $"/{newid.ToString()}/";
            data.id = Guid.Parse(id);
            data.targetUrl = "http://host.docker.internal:5000";
            data.rateLimit.rate = 8;
            data.rateLimit.per = 10;


            // Update_Api
            var RequestJson1 = JsonConvert.SerializeObject(data);
            HttpContent content1 = new StringContent(RequestJson1, Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("/api/v1/NewTyk/UpdateApi/updateapi", content1);
            response1.EnsureSuccessStatusCode();
            await HotReload();
            Thread.Sleep(5000);

            // downstream
            for (int i = 0; i < 8; i++)
            {
                var responseh = await DownStream(Url);
                responseh.EnsureSuccessStatusCode();
            }

            //check Rate Limit Exceed
            response = await DownStream(Url);
            response.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.TooManyRequests);

            //Rate Limiting Reset
            Thread.Sleep(10000);
            client = HttpClientFactory.Create();
            response = await client.GetAsync(Url);
            response.EnsureSuccessStatusCode();


            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();

        }


        public async Task<HttpResponseMessage> DownStream(string path)
        {
            var client = HttpClientFactory.Create();
            var response = await client.GetAsync(path);
            return response;
        }


        private async Task HotReload()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/v1/NewTyk/HotReload/HotReload");
            response.EnsureSuccessStatusCode();
        }



        private async Task<HttpResponseMessage> DeleteApi(string id)
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/v1/NewTyk/DeleteApi/deleteApi?apiId=" + id);
            // await HotReload();
            return response;
        }

    }
}
