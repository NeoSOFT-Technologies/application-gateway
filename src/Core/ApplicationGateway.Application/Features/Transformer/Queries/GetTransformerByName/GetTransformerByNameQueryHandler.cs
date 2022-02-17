using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
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

namespace ApplicationGateway.Application.Features.Transformer.Queries.GetTransformerByName
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
            string transformerName = request.TemplateName.Trim();
            var transformer = await _transRepository.GetTransformerByName(transformerName);
            if (transformer == null)
            {
                throw new NotFoundException(nameof(Transformers), request.TemplateName);
            }
            var result = _mapper.Map<GetTransformerByNameDto>(transformer);

            var response = new Response<GetTransformerByNameDto>(result);
            _logger.LogInformation("Handler completed");
            return response;
        }
    }
}
