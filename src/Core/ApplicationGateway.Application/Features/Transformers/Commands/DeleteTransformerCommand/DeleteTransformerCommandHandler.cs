using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Transformers.Commands.DeleteTransformerCommand
{
    public class DeleteTransformerCommandHandler : IRequestHandler<DeleteTransformerCommand>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteTransformerCommandHandler> _logger;
        private readonly IAsyncRepository<Transformer> _transRepository;

        public DeleteTransformerCommandHandler(IMapper mapper, ILogger<DeleteTransformerCommandHandler> logger, IAsyncRepository<Transformer> transRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _transRepository = transRepository;
        }

        public async Task<Unit> Handle(DeleteTransformerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@DeleteTransformerCommand}", request);
            Transformer transformerToDelete = await _transRepository.GetByIdAsync(request.TransformerId);

            if (transformerToDelete == null)
            {
                throw new NotFoundException(nameof(Transformer), request.TransformerId);
            }

            await _transRepository.DeleteAsync(transformerToDelete);
            _logger.LogInformation("Hanlde Completed");
            return Unit.Value;
        }
    }
}
