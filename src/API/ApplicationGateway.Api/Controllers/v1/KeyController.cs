using ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand;
using ApplicationGateway.Application.Features.Key.Commands.DeleteKeyCommand;
using ApplicationGateway.Application.Features.Key.Commands.UpdateKeyCommand;
using ApplicationGateway.Application.Features.Key.Queries.GetKey;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using JUST;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Api.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class KeyController : ControllerBase
    {
        readonly ILogger<KeyController> _logger;
        readonly IMediator _mediator;

        public KeyController(ILogger<KeyController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Response<Key>>> GetKey(string keyId)
        {
            _logger.LogInformation($"GetKey initiated in controller for {keyId}");
            var response = await _mediator.Send(new GetKeyQuery() { keyId = keyId });
            _logger.LogInformation($"GetKey completed in controller for {keyId}");
            return response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateKey(CreateKeyCommand createKeyCommand)
        {
            _logger.LogInformation($"CreateKey initiated in controller for {createKeyCommand}");
            Response<Key> response = await _mediator.Send(createKeyCommand);
            _logger.LogInformation($"CreateKey completed for {createKeyCommand}");
            return Ok(response);
        }


        [HttpPut]
        public async Task<ActionResult> UpdateKey(UpdateKeyCommand updateKeyCommand)
        {
            _logger.LogInformation($"UpdateKey initiated in controller for {updateKeyCommand}");
            Response<UpdateKeyCommandDto> response = await _mediator.Send(updateKeyCommand);
            _logger.LogInformation($"UpdateKey completed for {updateKeyCommand}");
            return Ok(response);
        }

    [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteKey(string keyId)
        {
            _logger.LogInformation($"DeleteKey initiated in controller for {keyId}");
            var response = await _mediator.Send(new DeleteKeyCommand() {KeyId=keyId });
            _logger.LogInformation($"DeleteKey completed in controller for {keyId}");
            return NoContent();
        }

    }
}
