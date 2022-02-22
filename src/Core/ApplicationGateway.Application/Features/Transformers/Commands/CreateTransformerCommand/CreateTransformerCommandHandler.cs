using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var gateway = Enums.Gateway.Tyk;
            var result = _mapper.Map<Transformer>(request);
            result = await _transRepository.CreateTransformer(result.TemplateName,request.TransformerTemplate,gateway);
            var transformer = _mapper.Map<CreateTransformerDto>(result);
            _logger.LogInformation("Hanlde Completed");
            Response<CreateTransformerDto> response = new Response<CreateTransformerDto>(transformer, "success");
            return response;
        }
    }
}
