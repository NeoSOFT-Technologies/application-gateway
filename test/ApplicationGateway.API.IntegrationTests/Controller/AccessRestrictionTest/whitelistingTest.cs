using ApplicationGateway.API.IntegrationTests.Base;
using ApplicationGateway.API.IntegrationTests.Helper;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
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

namespace ApplicationGateway.API.IntegrationTests.Controller.AccessRestrictionTest
{
    public partial class WhitelistingTest : IClassFixture<CustomWebApplicationFactory>
    {

        private readonly CustomWebApplicationFactory _factory;
        private HttpClient client = null;
        public WhitelistingTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            client = _factory.CreateClient();
        }

        [Fact]
        public async Task Whitelisting()
        {
            //var client = _factory.CreateClient();
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
    }
}