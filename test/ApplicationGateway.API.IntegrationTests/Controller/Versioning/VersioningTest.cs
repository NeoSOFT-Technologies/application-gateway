﻿using ApplicationGateway.API.IntegrationTests.Base;
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
using ApplicationGateway.API.IntegrationTests.Helper;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;

namespace ApplicationGateway.API.IntegrationTests.Controller
{
    public class VersioningTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public VersioningTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Versioning_byHeader()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = ApplicationConstants.TYK_BASE_URL + newid.ToString() + "/WeatherForecast";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH+"/Versioning/createApiData.json");
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
            await HotReload();
            Thread.Sleep(3000);

            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH+ "/Versioning/Header_Version.json");
            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;

            // Update_Api
            var updateRequestJson = JsonConvert.SerializeObject(data);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            await HotReload();
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
            await HotReload();

        }


        [Fact]
        public async Task Versioning_byQueryParam()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url;

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH+"/Versioning/createApiData.json");
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
            await HotReload();
            Thread.Sleep(3000);
            // Thread.Sleep(4000);

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
            await HotReload();
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
            await HotReload();

        }


        [Fact]
        public async Task Versioning_byUrl()
        {

            var client = _factory.CreateClient();
            Guid newid = Guid.NewGuid();
            string Url = "";

            //read json file 
            var myJsonString = File.ReadAllText(ApplicationConstants.BASE_PATH+"/Versioning/createApiData.json");
            CreateRequest requestModel1 = JsonConvert.DeserializeObject<CreateRequest>(myJsonString);
            requestModel1.name = newid.ToString();
            requestModel1.listenPath = $"/{newid}/";

            //create Api
            var RequestJson = JsonConvert.SerializeObject(requestModel1);
            HttpContent content = new StringContent(RequestJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/ApplicationGateway/CreateApi", content);
            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<CreateApiDto>>(jsonString.Result);
            var id = result.Data.ApiId;
            await HotReload();
            Thread.Sleep(3000);

            //Read Json
            var myJsonString1 = File.ReadAllText(ApplicationConstants.BASE_PATH+ "/Versioning/Url_Version.json");
            UpdateApiCommand data = JsonConvert.DeserializeObject<UpdateApiCommand>(myJsonString1);
            data.Name = newid.ToString();
            data.ListenPath = $"/{newid.ToString()}/";
            data.ApiId = id;

            // Update_Api
            var updateRequestJson = JsonConvert.SerializeObject(data);
            HttpContent updatecontent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateresponse = await client.PutAsync("/api/v1/ApplicationGateway", updatecontent);
            updateresponse.EnsureSuccessStatusCode();
            await HotReload();
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


        private async Task HotReload()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/v1/ApplicationGateway/HotReload");
            response.EnsureSuccessStatusCode();
        }

        private async Task<HttpResponseMessage> DeleteApi(Guid id)
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/v1/ApplicationGateway/" + id);
            return response;
        }
    }
}
