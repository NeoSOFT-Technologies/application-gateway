using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Transformers.Queries.GetAllTransformer
{
    public class GetAllTransformersQueryHandler : IRequestHandler<GetAllTransformersQuery, Response<IEnumerable<GetAllTransformersDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTransformersQueryHandler> _logger;
        private readonly IAsyncRepository<Transformer> _transRepository;

        public GetAllTransformersQueryHandler(IMapper mapper, ILogger<GetAllTransformersQueryHandler> logger, IAsyncRepository<Transformer> transRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _transRepository = transRepository;
        }

        public async Task<Response<IEnumerable<GetAllTransformersDto>>> Handle(GetAllTransformersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handle Initiated with {@GetTransformerQuery}", request);
            var allTransformer = await _transRepository.ListAllAsync();
            var transformer = _mapper.Map<IEnumerable<GetAllTransformersDto>>(allTransformer);
            _logger.LogInformation("Hanlde Completed");
            return new Response<IEnumerable<GetAllTransformersDto>>(transformer, "success");
        }
    }
}
