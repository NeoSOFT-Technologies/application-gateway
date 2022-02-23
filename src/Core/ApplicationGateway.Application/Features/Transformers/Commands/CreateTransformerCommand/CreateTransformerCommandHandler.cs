using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Transformers.Commands.CreateTransformerCommand
{
    public class CreateTransformerCommandHandler : IRequestHandler<CreateTransformerCommand, Response<CreateTransformerDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTransformerCommandHandler> _logger;
        private readonly ITransformerRepository _transRepository;

        public CreateTransformerCommandHandler(IMapper mapper, ILogger<CreateTransformerCommandHandler> logger, ITransformerRepository transRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _transRepository = transRepository;
        }

        public async Task<Response<CreateTransformerDto>> Handle(CreateTransformerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@CreateTransformerCommand}", request);
            Transformer result = _mapper.Map<Transformer>(request);
            result = await _transRepository.AddAsync(result);
            CreateTransformerDto transformer = _mapper.Map<CreateTransformerDto>(result);
            _logger.LogInformation("Hanlde Completed");
            Response<CreateTransformerDto> response = new Response<CreateTransformerDto>(transformer, "success");
            return response;
        }
    }
}
