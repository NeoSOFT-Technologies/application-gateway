using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.TykData;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Transformer.Commands.CreateTransformerCommand
{
    public class CreateTransformerCommandHandler : IRequestHandler<CreateTransformerCommand, Response<CreateTransformerDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTransformerCommandHandler> _logger;
        private readonly IAsyncRepository<Transformers> _transRepository;

        public CreateTransformerCommandHandler(IMapper mapper, ILogger<CreateTransformerCommandHandler> logger, IAsyncRepository<Transformers> transRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _transRepository = transRepository;
        }

        public async Task<Response<CreateTransformerDto>> Handle(CreateTransformerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@CreateTransformerCommand}", request);
            var result = _mapper.Map<Transformers>(request);
            result = await _transRepository.AddAsync(result);
            var transformer = _mapper.Map<CreateTransformerDto>(result);
            _logger.LogInformation("Hanlde Completed");
            Response<CreateTransformerDto> response = new Response<CreateTransformerDto>(transformer, "success");
            return response;
        }
    }
}
