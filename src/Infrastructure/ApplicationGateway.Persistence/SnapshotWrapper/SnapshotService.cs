﻿using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApplicationGateway.Persistence.SnapshotWrapper
{
    public class SnapshotService : ISnapshotService
    {
        private readonly ILogger<SnapshotService> _logger;
        private readonly ISnapshotRepository _snapshotRepository;
        public SnapshotService(ILogger<SnapshotService> logger, ISnapshotRepository snapshotRepository)
        {
            _logger = logger;
            _snapshotRepository = snapshotRepository;
        }

#nullable enable
        public Task<Snapshot> CreateSnapshot(Enums.Gateway gateway, Enums.Type snapshotType, Enums.Operation operation, string key, dynamic? jsonData)
#nullable disable
        {
            _logger.LogInformation("CreateSnapshot method triggered");
            Snapshot snapshot = new Snapshot()
            {
                Gateway = gateway.ToString(),
                ObjectName = snapshotType.ToString(),
                ObjectKey = key,
                JsonData = operation!= Enums.Operation.Deleted ? JsonConvert.SerializeObject(jsonData) : ""
            };
            return _snapshotRepository.TakeSnapshot(snapshot, operation);
        }
    }
}
