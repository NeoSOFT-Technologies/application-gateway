using ApplicationGateway.Application.Contracts.Persistence;
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

namespace ApplicationGateway.Application.Features.Transformers.Queries.GetAllTransformer
{
    public class GetAllTransformerQueryHandler : IRequestHandler<GetAllTransformerQuery, Response<IEnumerable<GetAllTransformerDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTransformerQueryHandler> _logger;
        private readonly IAsyncRepository<Transformer> _transRepository;

        public GetAllTransformerQueryHandler(IMapper mapper, ILogger<GetAllTransformerQueryHandler> logger, IAsyncRepository<Transformer> transRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _transRepository = transRepository;
        }

        public async Task<Response<IEnumerable<GetAllTransformerDto>>> Handle(GetAllTransformerQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handle Initiated with {@GetTransformerQuery}", request);
            var allTransformer = await _transRepository.ListAllAsync();
            var transformer = _mapper.Map<IEnumerable<GetAllTransformerDto>>(allTransformer);
            _logger.LogInformation("Hanlde Completed");
            return new Response<IEnumerable<GetAllTransformerDto>>(transformer, "success");
        }
    }
}
