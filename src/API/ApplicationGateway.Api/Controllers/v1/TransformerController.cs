using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Transformer.Commands.CreateTransformerCommand;
using ApplicationGateway.Application.Features.Transformer.Commands.DeleteTransformerCommand;
using ApplicationGateway.Application.Features.Transformer.Commands.UpdateTransformerCommand;
using ApplicationGateway.Application.Features.Transformer.Queries.GetTransformer;
using ApplicationGateway.Application.Features.Transformer.Queries.GetTransformerById;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.TykData;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationGateway.Api.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TransformerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransformerController> _logger;
        

        public TransformerController(IMediator mediator, ILogger<TransformerController> logger )
        {
            _logger = logger;
            _mediator = mediator;
            
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllTransformer()
        {
            _logger.LogInformation("GetAllTransformer Initiated");
            var dtos = await _mediator.Send(new GetTransformerQuery());
            _logger.LogInformation("GetAllTransformer Completed");
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTransformerById(Guid id)
        {
            _logger.LogInformation("GetTransformer Initiated with {@Id}", id);
            var getTransformer = new GetTransformerByIdQuery() { Id = id };
            _logger.LogInformation("GetTransformer Completed");
            return Ok(await _mediator.Send(getTransformer));
            
        }

        [HttpPost("CreateTransformer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateTransformer(CreateTransformerCommand transCommand)
        {
            _logger.LogInformation("CreateTransformer Initiated with {@CreateTransformerCommand}", transCommand);
             Response<CreateTransformerDto> response = await _mediator.Send(transCommand);
            _logger.LogInformation("CreateTransformer Completed");
            return Ok(response);

        }

        [HttpPut("UpdateTransformer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateTransformer(UpdateTransformerCommand updateTransformerCommand)
        {
            _logger.LogInformation("UpdateTransformer Initiated with {@UpdateTransformerCommand}", updateTransformerCommand);
            Response<UpdateTransformerDto> response = await _mediator.Send(updateTransformerCommand);
            _logger.LogInformation("UpdateTransformer Completed");
            return Ok(response);
        }

        [HttpDelete("{transId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTransformer(Guid transId)
        {
            _logger.LogInformation("DeleteTransformer Initiated with {@Id}", transId);
            await _mediator.Send(new DeleteTransformerCommand() { Id = transId });
            _logger.LogInformation("DeleteTransformer Completed");
            return NoContent();
        }


    }
}
