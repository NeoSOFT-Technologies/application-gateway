using ApplicationGateway.API.IntegrationTests.Base;
using ApplicationGateway.API.IntegrationTests.Helper;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
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


namespace ApplicationGateway.API.IntegrationTests.Controller.CreateApiTest
{
    public partial class CreateMultipleApisTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient client = null;
        public CreateMultipleApisTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            client = _factory.CreateClient();
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
            return response;
        }

    }
}
