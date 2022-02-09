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
            Console.WriteLine("test started");
            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/"+newid.ToString()+ "/weatherforecast";

            //read json file 
            var myJsonString = File.ReadAllText("../../../JsonData/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid}/";

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
            responseN.EnsureSuccessStatusCode();

            //delete Api
            var deleteResponse = await DeleteApi(id);  // await client.DeleteAsync("/api/NewTyk/deleteApi?apiId=" + requestModel1.api_id);//await DeleteApi(Request.api_id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();

        }

        [Fact]
        public async Task downstream()
        {
            Console.WriteLine("downstream test started");
            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:5000/weatherforecast";

            //read json file 
           
       


            //downstream
            var responseN = await DownStream(Url);
            responseN.EnsureSuccessStatusCode();
            Console.WriteLine("downstream test finish");

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
                responseN.EnsureSuccessStatusCode();
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
        public async Task Blacklisting()
        {
            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/"+newid.ToString()+"/WeatherForecast";
            string OriginUrl = $"http://localhost:8080/"+newid.ToString()+"/";
            //read json file 
            var myJsonString = File.ReadAllText("../../../JsonData/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid.ToString()}/";
            requestModel1.targetUrl = "http://httpbin.org/get";
            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/NewTyk/CreateApi/createApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);

            var id = result.key;
            await HotReload();
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
            var myupdateJsonString = File.ReadAllText("../../../JsonData/updateApiData.json");
            UpdateRequest updaterequestModel1 = JsonConvert.DeserializeObject<UpdateRequest>(myupdateJsonString);
            updaterequestModel1.name = newid.ToString();
            updaterequestModel1.listenPath = $"/{newid.ToString()}/";
            updaterequestModel1.id = new Guid(id);
            updaterequestModel1.blacklist = new List<string>
            {
                ipvalue
            };
            //updateappi
            var updateRequestJson = JsonConvert.SerializeObject(updaterequestModel1);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/NewTyk/UpdateApi/updateapi", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            await HotReload();
            Thread.Sleep(2000);

        
            //downstream
            var downstreamResponse = await DownStream(Url);
            downstreamResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.Forbidden);

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();
        }

        [Fact]
        public async Task Whitelisting()
        {
            
            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = $"http://localhost:8080/" + newid.ToString() + "/WeatherForecast";
            string OriginUrl = $"http://localhost:8080/" + newid.ToString() + "/";
            //read json file 
            var myJsonString = File.ReadAllText("../../../JsonData/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid}/";
            requestModel1.targetUrl = "http://httpbin.org/get";
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
            var myupdateJsonString = File.ReadAllText("../../../JsonData/updateApiData.json");
            UpdateRequest updaterequestModel1 = JsonConvert.DeserializeObject<UpdateRequest>(myupdateJsonString);
            updaterequestModel1.name = newid.ToString();
            updaterequestModel1.listenPath = $"/{newid}/";
            updaterequestModel1.id = new Guid(id);
            updaterequestModel1.whitelist = new List<string>
            {
                ipvalue
            };
            //updateappi
            var updateRequestJson = JsonConvert.SerializeObject(updaterequestModel1);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/NewTyk/UpdateApi/updateapi", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            await HotReload();
            Thread.Sleep(4000);

            //downstream
            var downstreamResponse = await DownStream(Url);
            downstreamResponse.EnsureSuccessStatusCode();

            //delete Api
            var deleteResponse = await DeleteApi(id);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NoContent);
            await HotReload();
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
