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

namespace ApplicationGateway.Application.Features.Transformer.Commands.UpdateTransformerCommand
{
    public class UpdateTransformerCommandHandler : IRequestHandler<UpdateTransformerCommand, Response<UpdateTransformerDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTransformerCommandHandler> _logger;
        private readonly IAsyncRepository<Transformers> _transRepository;

        public UpdateTransformerCommandHandler(IMapper mapper, ILogger<UpdateTransformerCommandHandler> logger, IAsyncRepository<Transformers> transRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _transRepository = transRepository;
        }
        public async Task<Response<UpdateTransformerDto>> Handle(UpdateTransformerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@UpdateTransformerCommand}", request);
            var transformerToUpdate = await _transRepository.GetByIdAsync(request.Id);
            if(transformerToUpdate == null)
            {
                throw new NotFoundException(nameof(Transformers), request.Id);
            }
            _mapper.Map(request, transformerToUpdate, typeof(UpdateTransformerCommand), typeof(Transformers));

            await _transRepository.UpdateAsync(transformerToUpdate);
            _logger.LogInformation("Hanlde Completed");
            return new Response<UpdateTransformerDto>("Update Successfully");
        }
    }
}
