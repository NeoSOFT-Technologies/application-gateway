using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Domain.TykData;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Transformer.Commands.DeleteTransformerCommand
{
    public class DeleteTransformerCommandHandler : IRequestHandler<DeleteTransformerCommand>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteTransformerCommandHandler> _logger;
        private readonly IAsyncRepository<Transformers> _transRepository;

        public DeleteTransformerCommandHandler(IMapper mapper, ILogger<DeleteTransformerCommandHandler> logger, IAsyncRepository<Transformers> transRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _transRepository = transRepository;
        }

        public async Task<Unit> Handle(DeleteTransformerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@DeleteTransformerCommand}", request);
            var transformerToDelete = await _transRepository.GetByIdAsync(request.Id);

            if (transformerToDelete == null)
            {
                throw new NotFoundException(nameof(Transformer), request.Id);
            }

            await _transRepository.DeleteAsync(transformerToDelete);
            _logger.LogInformation("Hanlde Completed");
            return Unit.Value;
        }
    }
}
