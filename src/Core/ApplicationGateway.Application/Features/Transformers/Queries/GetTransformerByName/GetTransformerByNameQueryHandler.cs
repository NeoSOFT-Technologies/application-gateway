using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerByName
{
    public class GetTransformerByNameQueryHandler : IRequestHandler<GetTransformerByNameQuery, Response<GetTransformerByNameDto>>

    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetTransformerByNameQueryHandler> _logger;
        private readonly ITransformerRepository _transRepository;

        public GetTransformerByNameQueryHandler(IMapper mapper, ILogger<GetTransformerByNameQueryHandler> logger, ITransformerRepository transRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _transRepository = transRepository;
        }

        public async Task<Response<GetTransformerByNameDto>> Handle(GetTransformerByNameQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler initiated with {@GetTransformerByNameQuery}", request);
            Transformer transformer = await _transRepository.GetTransformerByNameAndGateway(request.TemplateName, request.Gateway);
            if (transformer == null)
            {
                throw new NotFoundException(nameof(Transformers), request.TemplateName);
            }
            GetTransformerByNameDto result = _mapper.Map<GetTransformerByNameDto>(transformer);

            Response<GetTransformerByNameDto> response = new Response<GetTransformerByNameDto>(result);
            _logger.LogInformation("Handler completed");
            return response;
        }
    }
}
