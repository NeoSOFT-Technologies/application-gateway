using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerById
{
    public class GetTransformerByIdQueryHandler : IRequestHandler<GetTransformerByIdQuery, Response<GetTransformerByIdDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetTransformerByIdQueryHandler> _logger;
        private readonly IAsyncRepository<Transformer> _transRepository;

        public GetTransformerByIdQueryHandler(IMapper mapper, ILogger<GetTransformerByIdQueryHandler> logger, IAsyncRepository<Transformer> transRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _transRepository = transRepository;
        }
        public async Task<Response<GetTransformerByIdDto>> Handle(GetTransformerByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler initiated with {@GetTransformerByIdQuery}", request);
            Transformer transformer = await _transRepository.GetByIdAsync(request.TransformerId);

            if (transformer == null)
            {
                throw new NotFoundException(nameof(Transformers), request.TransformerId);
            }
            GetTransformerByIdDto result = _mapper.Map<GetTransformerByIdDto>(transformer);

            var response = new Response<GetTransformerByIdDto>(result);
            _logger.LogInformation("Handler completed");
            return response;
        }
    }
}
