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

namespace ApplicationGateway.Application.Features.Transformer.Queries.GetTransformer
{
    public class GetTransformerQueryHandler : IRequestHandler<GetTransformerQuery, Response<IEnumerable<GetTransformerDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetTransformerQueryHandler> _logger;
        private readonly IAsyncRepository<Transformers> _transRepository;

        public GetTransformerQueryHandler(IMapper mapper, ILogger<GetTransformerQueryHandler> logger, IAsyncRepository<Transformers> transRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _transRepository = transRepository;
        }

        public async Task<Response<IEnumerable<GetTransformerDto>>> Handle(GetTransformerQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handle Initiated with {@GetTransformerQuery}", request);
            var allTransformer = await _transRepository.ListAllAsync();
            var transformer = _mapper.Map<IEnumerable<GetTransformerDto>>(allTransformer);
            _logger.LogInformation("Hanlde Completed");
            return new Response<IEnumerable<GetTransformerDto>>(transformer, "success");
        }
    }
}
