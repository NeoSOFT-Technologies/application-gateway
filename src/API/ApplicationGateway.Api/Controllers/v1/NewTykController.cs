using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ApplicationGateway.Domain.TykData;
using Newtonsoft.Json;
using JUST;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace ApplicationGateway.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NewTykController : ControllerBase
    {
        [HttpPost("createApi")]
        public async Task<ActionResult> CreateApi(CreateRequest[] request)
        {    
            //Check for repeated listen path in request
            if (request.DistinctBy(p => p.listenPath.Trim('/')).Count() != request.Count())
            {
                return BadRequest("Listen path should be unique in array");
            }

            //Check for repeated listen path in existing APIs
            JArray allApi = JArray.Parse(await GetApi());
            foreach (var obj in request)
            {
                foreach (var api in allApi)
                {
                    string listen_path = api["proxy"]["listen_path"].ToString();
                    if (obj.listenPath.Trim('/') == listen_path.Trim('/'))
                    {
                        return BadRequest("listen path already exists");
                    }
                }
            }


            string transformer = System.IO.File.ReadAllText(@"E:\Projects\Tyk Repository\ApplicationGateway\ApplicationGateway\docs\CreateTransformer.json");

            foreach (var obj in request)
            {
                string requestJson = JsonConvert.SerializeObject(obj);
                string transformed = new JsonTransformer().Transform(transformer, requestJson);
                JObject finalJson = JObject.Parse(transformed);
                finalJson.Add("api_id", Guid.NewGuid());
                using (HttpClient httpClient = new HttpClient())
                {
                    StringContent stringContent = new StringContent(finalJson.ToString(), System.Text.Encoding.UTF8, "text/plain");
                    httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                    HttpResponseMessage httpResponse = httpClient.PostAsync("http://localhost:8080/tyk/apis", stringContent).Result;
                    HotReload();
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }
                }

            }
            return Ok("APIs from API array created successfully");
        }

        [HttpPut("updateapi")]
        public ActionResult updateApi(UpdateRequest request)
        {
            string requestJson = JsonConvert.SerializeObject(request);
            string transformer = System.IO.File.ReadAllText(@"E:\Projects\Tyk Repository\ApplicationGateway\ApplicationGateway\docs\UpdateTransformer.json");
            string transformed = new JsonTransformer().Transform(transformer, requestJson);

            JObject inputObject = JObject.Parse(requestJson);
            JObject transformedObject = JObject.Parse(transformed);
            if (inputObject["versions"].Count() != 0)
            {
                transformedObject["version_data"]["versions"] = new JObject();
                foreach (var version in inputObject["versions"])
                {
                    (version as JObject).Add("use_extended_paths", true);
                    (transformedObject["version_data"]["versions"] as JObject).Add($"{version["name"]}", version);
                }
            }

            using (HttpClient httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(transformedObject.ToString(), System.Text.Encoding.UTF8, "text/plain");
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                string Url = $"http://localhost:8080/tyk/apis/{request.id}";
                HttpResponseMessage httpResponse = httpClient.PutAsync(Url, stringContent).Result;
                HotReload();
            }
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpDelete("deleteApi")]
        public ActionResult DeleteApi(string apiId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                string url = "http://localhost:8080/tyk/apis/" + apiId;
                HttpResponseMessage httpResponse = httpClient.DeleteAsync(url).Result;
                HotReload();
                //    HttpResponseMessage httpResponse1 = httpClient.GetAsync("http://localhost:8080/tyk/reload/group").Result;
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return NotFound();
                }

            }
            return NoContent();
        }

        //[HttpPut]
        //public async Task<dynamic> CircuitBreaker(CircuitBreakerRequest cb_request)
        //{
        //    var updateObj = await GetApiById(cb_request.apiId);
            
        //        string cb_requestJson = JsonConvert.SerializeObject(cb_request);
        //    string cb_transformer = System.IO.File.ReadAllText(@"E:\Projects\Tyk Repository\ApplicationGateway\ApplicationGateway\docs\CircuitBreakerTransformer.json");

        //    string transformed_cb_String = new JsonTransformer().Transform(cb_transformer, cb_requestJson);
        //    JObject transformed_cb_Json = JObject.Parse(transformed_cb_String);
        //    if (updateObj["version_data"]["versions"].Count() != 0)
        //    {
        //        updateObj["version_data"]["versions"][cb_request.version]["use_extended_paths"] = true;
        //        updateObj["version_data"]["versions"][cb_request.version]["extended_paths"].Parent.Remove();
        //        (updateObj["version_data"]["versions"][cb_request.version] as JObject).Add("extended_paths", transformed_cb_Json);
        //    }
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        StringContent stringContent = new StringContent(updateObj.ToString(), System.Text.Encoding.UTF8, "text/plain");
        //        httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
        //        string Url = $"http://localhost:8080/tyk/apis/{cb_request.apiId}";
        //        HttpResponseMessage httpResponse = await httpClient.PutAsync(Url, stringContent);
        //        HotReload();
        //    }
        //    return Ok();
        //}

        [HttpGet("HotReload")]
        public ActionResult HotReload()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse = httpClient.GetAsync("http://localhost:8080/tyk/reload/group").Result;
               

            }
            return Ok();
        }

        [HttpGet(Name = "getApi")]
        public async Task<string> GetApi()
        {
            string result;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse = await httpClient.GetAsync("http://localhost:8080/tyk/apis");
                result = await httpResponse.Content.ReadAsStringAsync();
            }
            return result;
        }
        [HttpGet]
        public async Task<dynamic> GetApiById(string api_id)
        {
            JObject obj = new JObject();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                var httpResponse =await httpClient.GetAsync($"http://localhost:8080/tyk/apis/{api_id}");
                var content =await httpResponse.Content.ReadAsStringAsync();
                obj = JObject.Parse(content);
            }
            return obj.ToString();
        }

    }
}
