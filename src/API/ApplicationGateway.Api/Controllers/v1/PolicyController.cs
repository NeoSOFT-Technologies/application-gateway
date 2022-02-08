using Microsoft.AspNetCore.Mvc;
using ApplicationGateway.Domain.TykData;
using Newtonsoft.Json;
using JUST;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllPolicies()
        {
            string folderPath = @"C:\Projects\tyk\tyk-gateway-docker\policies";
            if (!Directory.Exists(folderPath) || !System.IO.File.Exists(folderPath + @"\policies.json"))
            {
                return NotFound("Policies not found");
            }

            string policiesJson = System.IO.File.ReadAllText(folderPath + @"\policies.json");
            JObject policiesObject = JObject.Parse(policiesJson);

            return Ok(policiesObject.ToString());
        }

        [HttpGet("{policyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetPolicyByid(Guid policyId)
        {
            string folderPath = @"C:\Projects\tyk\tyk-gateway-docker\policies";
            if (!Directory.Exists(folderPath) || !System.IO.File.Exists(folderPath + @"\policies.json"))
            {
                return NotFound("Policies not found");
            }

            string policiesJson = System.IO.File.ReadAllText(folderPath + @"\policies.json");
            JObject policiesObject = JObject.Parse(policiesJson);
            if (!policiesObject.ContainsKey(policyId.ToString()))
            {
                return NotFound($"Policy with id: {policyId} was not found");
            }

            string policy = (policiesObject[policyId.ToString()] as JObject).ToString();

            return Ok(policy);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreatePolicy(Policy request)
        {
            string requestJson = JsonConvert.SerializeObject(request);
            string path = Directory.GetCurrentDirectory();
            string transformer = System.IO.File.ReadAllText(path + @"\JsonTransformers\PolicyTransformer.json");
            string transformed = new JsonTransformer().Transform(transformer, requestJson);

            JObject inputObject = JObject.Parse(requestJson);
            JObject transformedObject = JObject.Parse(transformed);
            if (inputObject["APIs"].Count() != 0)
            {
                transformedObject["access_rights"] = new JObject();
                foreach (var api in inputObject["APIs"])
                {
                    var apiObject = new JObject()
                    {
                        { "api_id", api["Id"] },
                        { "api_name", api["Name"] },
                        { "versions", api["Versions"] },
                        { "allowed_urls", api["AllowedUrls"] },
                        { "limit", api["Limit"] }
                    };
                    (transformedObject["access_rights"] as JObject).Add($"{api["Id"]}", apiObject);
                }
            }
            string folderPath = @"C:\Projects\tyk\tyk-gateway-docker\policies";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if(!System.IO.File.Exists(folderPath + @"\policies.json"))
            {
                var sw = System.IO.File.CreateText(folderPath + @"\policies.json");
                sw.WriteLine("{}");
                sw.Dispose();
            }
            string policiesJson = System.IO.File.ReadAllText(folderPath + @"\policies.json");
            JObject policiesObject = JObject.Parse(policiesJson);
            string policyId = Guid.NewGuid().ToString();
            policiesObject.Add(policyId, transformedObject);

            System.IO.File.WriteAllText(@"C:\Projects\tyk\tyk-gateway-docker\policies\policies.json", policiesObject.ToString());
            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse = httpClient.GetAsync("http://localhost:8080/tyk/reload/group").Result;
            }

            return Ok($"Policy created successfully with PolicyId: {policyId}");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdatePolicy(Policy request)
        {
            string requestJson = JsonConvert.SerializeObject(request);
            string path = Directory.GetCurrentDirectory();
            string transformer = System.IO.File.ReadAllText(path + @"\JsonTransformers\PolicyTransformer.json");
            string transformed = new JsonTransformer().Transform(transformer, requestJson);

            JObject inputObject = JObject.Parse(requestJson);
            JObject transformedObject = JObject.Parse(transformed);
            if (inputObject["APIs"].Count() != 0)
            {
                transformedObject["access_rights"] = new JObject();
                foreach (var api in inputObject["APIs"])
                {
                    var apiObject = new JObject()
                    {
                        { "api_id", api["Id"] },
                        { "api_name", api["Name"] },
                        { "versions", api["Versions"] },
                        { "allowed_urls", api["AllowedUrls"] },
                        { "limit", api["Limit"] }
                    };
                    (transformedObject["access_rights"] as JObject).Add($"{api["Id"]}", apiObject);
                }
            }

            string folderPath = @"C:\Projects\tyk\tyk-gateway-docker\policies";
            if (!Directory.Exists(folderPath) || !System.IO.File.Exists(folderPath + @"\policies.json"))
            {
                return NotFound("Policies not found");
            }

            string policiesJson = System.IO.File.ReadAllText(folderPath + @"\policies.json");
            JObject policiesObject = JObject.Parse(policiesJson);
            string policyId = request.PolicyId.ToString();
            if (!policiesObject.ContainsKey(policyId))
            {
                return NotFound($"Policy with id: {policyId} was not found");
            }

            policiesObject.Remove(policyId);
            policiesObject.Add(policyId, transformedObject);

            System.IO.File.WriteAllText(@"C:\Projects\tyk\tyk-gateway-docker\policies\policies.json", policiesObject.ToString());

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse = httpClient.GetAsync("http://localhost:8080/tyk/reload/group").Result;
            }

            return Ok($"Policy with PolicyId: {policyId} updated successfully");
        }

        [HttpDelete("{policyId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePolicy(Guid policyId)
        {
            string folderPath = @"C:\Projects\tyk\tyk-gateway-docker\policies";
            if (!Directory.Exists(folderPath) || !System.IO.File.Exists(folderPath + @"\policies.json"))
            {
                return NotFound("Policies not found");
            }

            string policiesJson = System.IO.File.ReadAllText(folderPath + @"\policies.json");
            JObject policiesObject = JObject.Parse(policiesJson);
            if (!policiesObject.ContainsKey(policyId.ToString()))
            {
                return NotFound($"Policy with id: {policyId} was not found");
            }

            policiesObject.Remove(policyId.ToString());

            System.IO.File.WriteAllText(@"C:\Projects\tyk\tyk-gateway-docker\policies\policies.json", policiesObject.ToString());

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse = httpClient.GetAsync("http://localhost:8080/tyk/reload/group").Result;
            }
            return NoContent();
        }
    }
}
