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
using System.Text;

namespace ApplicationGateway.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
     [ApiController]
    public class NewTykController : ControllerBase
    {

        [HttpPost("createApi")]
        public async Task<ActionResult> CreateApi(CreateRequest request)
        {
           // string path = Directory.GetCurrentDirectory();
            // string transformer = System.IO.File.ReadAllText(path + @"\JsonTransformers\CreateApiTransformer.json");
            string transformer = System.IO.File.ReadAllText(@"JsonTransformers/CreateApiTransformer.json");

            string requestJson = JsonConvert.SerializeObject(request);
            string transformed = new JsonTransformer().Transform(transformer, requestJson);
            JObject finalJson = JObject.Parse(transformed);
            finalJson.Add("api_id", Guid.NewGuid());
            ResponseModel result;
            using (HttpClient httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(finalJson.ToString(), System.Text.Encoding.UTF8, "text/plain");
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse =await httpClient.PostAsync("http://localhost:8080/tyk/apis", stringContent);
                HotReload();

                //read response
                var jsonString = httpResponse.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return NotFound();
                }
            }
            return Ok(result);  //"Api created successfully"

        }




        [HttpPost("createMultipleApi")]
        public async Task<ActionResult> CreateMultipleApi(List<CreateRequest> request)
        {    
            //Check for repeated listen path in request
            if (request.DistinctBy(p => p.listenPath.Trim('/')).Count() != request.Count())
            {
                return BadRequest("Listen path should be unique in array");
            }

            //Check for repeated listen path in existing APIs
            JArray allApi = JArray.Parse(await GetApi());
            foreach (CreateRequest obj in request)
            {
                foreach (JToken api in allApi)
                {
                    string listen_path = api["proxy"]["listen_path"].ToString();
                    if (obj.listenPath.Trim('/') == listen_path.Trim('/'))
                    {
                        return BadRequest("listen path already exists");
                    }
                }
            }

            /*string path = Directory.GetCurrentDirectory();
            string transformer = System.IO.File.ReadAllText(path + @"\JsonTransformers\CreateApiTransformer.json");*/
            string transformer = System.IO.File.ReadAllText(@"JsonTransformers/CreateApiTransformer.json");
            List<ResponseModel> resultList = new List<ResponseModel>();

            foreach (CreateRequest obj in request)
            {
                string requestJson = JsonConvert.SerializeObject(obj);
                string transformed = new JsonTransformer().Transform(transformer, requestJson);
                JObject finalJson = JObject.Parse(transformed);
                finalJson.Add("api_id", Guid.NewGuid());
                using (HttpClient httpClient = new HttpClient())
                {
                    StringContent stringContent = new StringContent(finalJson.ToString(), System.Text.Encoding.UTF8, "text/plain");
                    httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                    HttpResponseMessage httpResponse = await httpClient.PostAsync("http://localhost:8080/tyk/apis", stringContent);
                    HotReload();

                    //read response
                    var jsonString = httpResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResponseModel>(jsonString.Result);
                    resultList.Add(result);

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }
                }
            }
            return Ok(resultList);    //"APIs from API array created successfully"
        }



        [HttpPut("updateapi")]
        public ActionResult UpdateApi(UpdateRequest request)
        {
            string requestJson = JsonConvert.SerializeObject(request);
            //    string path = Directory.GetCurrentDirectory();
            //   string transformer = System.IO.File.ReadAllText(path + @"\JsonTransformers\UpdateApiTransformer.json");
            string transformer = System.IO.File.ReadAllText( @"JsonTransformers/UpdateApiTransformer.json");
            string transformed = new JsonTransformer().Transform(transformer, requestJson);

            JObject inputObject = JObject.Parse(requestJson);
            JObject transformedObject = JObject.Parse(transformed);
            if (inputObject["versions"].Count() != 0)
            {
                transformedObject["version_data"]["versions"] = new JObject();
                foreach (JToken version in inputObject["versions"])
                {
                    (version as JObject).Add("use_extended_paths", true);
                    JArray removeGlobalHeaders = new JArray();
                    removeGlobalHeaders.Add("Authorization");
                    (version as JObject).Add("global_headers_remove", removeGlobalHeaders);
                    (transformedObject["version_data"]["versions"] as JObject).Add($"{version["name"]}", version);
                    (transformedObject["version_data"]["versions"][$"{version["name"]}"] as JObject).Add("override_target", version["overrideTarget"]);
                }
            }

            if (inputObject["authType"].ToString() == "openid")
            {
                transformedObject["openid_options"]["providers"] = new JArray();
                foreach (JToken provider in inputObject["openidOptions"]["providers"])
                {
                    JObject newProvider = new JObject();
                    newProvider.Add("issuer", provider["issuer"]);
                    JObject newClient = new JObject();
                    foreach (JToken client in provider["client_ids"])
                    {
                        string base64ClientId = Convert.ToBase64String(Encoding.UTF8.GetBytes(client["clientId"].ToString()));
                        newClient.Add(base64ClientId, client["policy"]);
                    }
                    newProvider.Add("client_ids", newClient);
                    (transformedObject["openid_options"]["providers"] as JArray).Add(newProvider);
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
                HttpResponseMessage httpResponse =await httpClient.GetAsync($"http://localhost:8080/tyk/apis/{api_id}");
                string content =await httpResponse.Content.ReadAsStringAsync();
                obj = JObject.Parse(content);
            }
            return obj.ToString();
        }
    }
}
