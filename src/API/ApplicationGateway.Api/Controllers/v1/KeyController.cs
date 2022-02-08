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
        [HttpPost]
        public async Task<ActionResult<string>> CreateKey(CreateKeyRequest request)
        {
            string path = Directory.GetCurrentDirectory();
            string transformer = System.IO.File.ReadAllText(path + @"\JsonTransformers\CreateKeyTransformer.json");

            //string transformer = System.IO.File.ReadAllText(@"E:\Projects\Tyk Repository\ApplicationGateway\ApplicationGateway\docs\CreateKeyTransformer.json");
            //jsonObj.Add("apply_policies", new JArray() {policyId});orm(transformer, requestJson);
            string requestString = JsonConvert.SerializeObject(request);
            JObject inputObject = JObject.Parse(requestString);

            string transformedObj = new JsonTransformer().Transform(transformer, requestString);
            JObject jsonObj = JObject.Parse(transformedObj);
            if (request.accessRights != null)
            {
                jsonObj["access_rights"] = new JObject();
                //foreach(var api in inputObject["access_rights"])
                //{
                //    JObject obj = new JObject();
                //    obj.Add("api_id", api["apiId"]);
                //    obj.Add("apiName", api["apiName"]);
                //    (jsonObj["access_rights"] as JObject).Add(api["apiId"], );
                //}
                if (request.policyId == "")
                {
                    jsonObj.Remove("apply_policies");
                }
                foreach(var api in request.accessRights)
                {
                    string jsonString = JsonConvert.SerializeObject(api);
                    JObject obj = JObject.Parse(jsonString);
                    JObject accObj = new JObject();
                    accObj.Add("api_id", obj["apiId"]);
                    accObj.Add("api_name", obj["apiName"]);
                    //(jsonObj["access_rights"] as JObject).Add(api.apiId,obj);
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
