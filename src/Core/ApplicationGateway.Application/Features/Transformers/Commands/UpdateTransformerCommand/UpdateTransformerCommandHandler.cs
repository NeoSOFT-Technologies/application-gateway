using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Transformers.Commands.UpdateTransformerCommand
{
    public class UpdateTransformerCommandHandler : IRequestHandler<UpdateTransformerCommand, Response<UpdateTransformerDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTransformerCommandHandler> _logger;
        private readonly IAsyncRepository<Transformer> _transRepository;

        public UpdateTransformerCommandHandler(IMapper mapper, ILogger<UpdateTransformerCommandHandler> logger, IAsyncRepository<Transformer> transRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _transRepository = transRepository;
        }
        public async Task<Response<UpdateTransformerDto>> Handle(UpdateTransformerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@UpdateTransformerCommand}", request);
            Transformer transformerToUpdate = await _transRepository.GetByIdAsync(request.TransformerId);
            if(transformerToUpdate == null)
            {
                throw new NotFoundException(nameof(Transformers), request.TransformerId);
            }
            _mapper.Map(request, transformerToUpdate, typeof(UpdateTransformerCommand), typeof(Transformer));

            await _transRepository.UpdateAsync(transformerToUpdate);
            _logger.LogInformation("Hanlde Completed");
            return new Response<UpdateTransformerDto>("Update Successfully");
        }
    }
}
