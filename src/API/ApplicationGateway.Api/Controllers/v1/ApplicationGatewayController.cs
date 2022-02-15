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
using MediatR;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Responses;
using Microsoft.Extensions.Options;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
using ApplicationGateway.Application.Features.Api.Commands.DeleteApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery;

namespace ApplicationGateway.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApplicationGatewayController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationGatewayController> _logger;

        public ApplicationGatewayController(IMediator mediator, ILogger<ApplicationGatewayController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllApis()
        {
            _logger.LogInformation("GetAllApis Initiated");
            Response<GetAllApisDto> response = await _mediator.Send(new GetAllApisQuery());
            _logger.LogInformation("GetAllApis Completed");
            return Ok(response);
        }

        [HttpGet("{apiId}")]
        public async Task<dynamic> GetApiById(string api_id)
        {
            JObject obj = new JObject();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse = await httpClient.GetAsync($"http://localhost:8080/tyk/apis/{api_id}");
                string content = await httpResponse.Content.ReadAsStringAsync();
                obj = JObject.Parse(content);
            }
            return obj.ToString();
        }

        [HttpPost("CreateApi")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateApi(CreateApiCommand createApiCommand)
        {
            _logger.LogInformation("CreateApi Initiated with {@CreateApiCommand}", createApiCommand);
            Response<CreateApiDto> response = await _mediator.Send(createApiCommand);
            _logger.LogInformation("CreateApi Completed");
            return Ok(response);

        }

        [HttpPost("CreateMultipleApis")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateMultipleApis(CreateMultipleApisCommand createMultipleApisCommand)
        {
            _logger.LogInformation("CreateMultipleApis Initiated with {@CreateMultipleApisCommand}", createMultipleApisCommand);
            Response<CreateMultipleApisDto> response = await _mediator.Send(createMultipleApisCommand);
            _logger.LogInformation("CreateMultipleApis Completed");
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateApi(UpdateApiCommand updateApiCommand)
        {
            _logger.LogInformation("UpdateApi Initiated with {@updateApiCommand}", updateApiCommand);
            Response<UpdateApiDto> response = await _mediator.Send(updateApiCommand);
            _logger.LogInformation("UpdateApi Completed");
            return Ok(response);
        }

        [HttpDelete("{apiId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteApi(Guid apiId)
        {
            _logger.LogInformation("DeleteApi Initiated with {@ApiId}", apiId);
            await _mediator.Send(new DeleteApiCommand() { ApiId = apiId });
            _logger.LogInformation("DeleteApi Completed");
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
    }
}
