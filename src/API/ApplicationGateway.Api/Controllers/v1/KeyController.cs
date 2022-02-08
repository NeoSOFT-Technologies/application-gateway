using ApplicationGateway.Domain.TykData;
using JUST;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Api.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class KeyController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetKey(string keyId)
        {
            dynamic key;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse = await httpClient.GetAsync("http://localhost:8080/tyk/keys/" + keyId);
                if (!httpResponse.IsSuccessStatusCode)
                    return NotFound("Failed to get key");
                key = await httpResponse.Content.ReadAsStringAsync();
                HotReload();
            }
            return Ok(key);
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateKey(CreateKeyRequest request)
        {
            string path = Directory.GetCurrentDirectory();
            string transformer = System.IO.File.ReadAllText(path + @"\JsonTransformers\CreateKeyTransformer.json");
            string requestString = JsonConvert.SerializeObject(request);
            JObject inputObject = JObject.Parse(requestString);
            string transformedObj = new JsonTransformer().Transform(transformer, requestString);
            JObject jsonObj = JObject.Parse(transformedObj);
            if (request.accessRights != null)
            {
                jsonObj["access_rights"] = new JObject();
                if (request.policyId == "")
                {
                    jsonObj.Remove("apply_policies");
                }
                foreach(var api in request.accessRights)
                {
                    string jsonString = JsonConvert.SerializeObject(api);
                    JObject obj = JObject.Parse(jsonString);
                    JArray versions = new JArray();
                    api.versions.ForEach(v => versions.Add(v));
                    JObject accObj = new JObject();
                    accObj.Add("api_id", obj["apiId"]);
                    accObj.Add("api_name", obj["apiName"]);
                    accObj.Add("versions",versions); 
                    (jsonObj["access_rights"] as JObject).Add(obj["apiId"].ToString(),accObj);
                }
            }
            dynamic key;
            using (HttpClient httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(jsonObj.ToString(), System.Text.Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                string url = "http://localhost:8080/tyk/keys";
                HttpResponseMessage httpResponse = await httpClient.PostAsync(url,stringContent);
                HotReload();
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return NotFound();
                }
                key = await httpResponse.Content.ReadAsStringAsync();
            }

            return key;
        }


        [HttpPut]
        public async Task<ActionResult<dynamic>> UpdateKey(UpdateKeyRequest request)
        {
            string path = Directory.GetCurrentDirectory();
            string transformer = System.IO.File.ReadAllText(path + @"\JsonTransformers\UpdateKeyTransformer.json");
            string requestString = JsonConvert.SerializeObject(request);

            string transformedObj = new JsonTransformer().Transform(transformer, requestString);
            JObject jsonObj = JObject.Parse(transformedObj);
            if (request.accessRights != null)
            {
                jsonObj["access_rights"] = new JObject();
                if (request.policyId == "")
                {
                    jsonObj.Remove("apply_policies");
                }
                foreach (var api in request.accessRights)
                {
                    string jsonString = JsonConvert.SerializeObject(api);
                    JObject obj = JObject.Parse(jsonString);
                    JArray versions = new JArray();
                    api.versions.ForEach(v => versions.Add(v));
                    JObject accObj = new JObject();
                    accObj.Add("api_id", obj["apiId"]);
                    accObj.Add("api_name", obj["apiName"]);
                    accObj.Add("versions", versions);
                    (jsonObj["access_rights"] as JObject).Add(obj["apiId"].ToString(), accObj);
                }
            }
            dynamic key;
            using (HttpClient httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(jsonObj.ToString(), System.Text.Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                string url = "http://localhost:8080/tyk/keys/"+request.keyId;
                HttpResponseMessage httpResponse = await httpClient.PutAsync(url, stringContent);
                HotReload();
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return NotFound();
                }
                key = await httpResponse.Content.ReadAsStringAsync();
            }

            return Ok(key);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteKey(string keyId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse = await httpClient.DeleteAsync("http://localhost:8080/tyk/keys/"+keyId);
                if (!httpResponse.IsSuccessStatusCode)
                    return NotFound("Failed to delete key");
                HotReload();
            }
            return Ok("Key deleted succesfully!");
        }

        [HttpGet]
        public ActionResult HotReload()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse = httpClient.GetAsync("http://localhost:8080/tyk/reload/group").Result;
            }
            return Ok();
        }
    }
}
