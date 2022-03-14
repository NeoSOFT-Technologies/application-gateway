using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class SnapshotRepository : BaseRepository<Snapshot>, ISnapshotRepository
    {
        private readonly ILogger _logger;
        public SnapshotRepository(ApplicationDbContext dbContext, ILogger<Snapshot> logger) : base(dbContext, logger)
        {
            _logger = logger;
        }

        public async Task<Snapshot> TakeSnapshot(Snapshot @snapshot, Enums.Operation operation)
        {
            _logger.LogInformation("TakeSnapShot method triggered");
#nullable enable 
            Snapshot? _snapshot;
#nullable disable
            _dbContext.BeginTransaction();
            switch (operation)
            {
                case Enums.Operation.Created:
                        await _dbContext.Set<Snapshot>().AddAsync(@snapshot);
                    break;
                
                case Enums.Operation.Updated:
                        _snapshot = _dbContext.Set<Snapshot>().Where(a => a.IsActive == true &&
                        a.ObjectName == @snapshot.ObjectName &&
                        a.ObjectKey == @snapshot.ObjectKey).FirstOrDefault();
                        
                        //Update only if there is any change
                        if (_snapshot != null && _snapshot.JsonData != @snapshot.JsonData)
                        {
                            _snapshot.IsActive = false;
                            _dbContext.Update<Snapshot>(_snapshot);
                            await _dbContext.Set<Snapshot>().AddAsync(@snapshot);
                        }
                    break;
                
                case Enums.Operation.Deleted:
                        _snapshot = _dbContext.Set<Snapshot>().Where(a => a.IsActive == true &&
                        a.ObjectName == @snapshot.ObjectName &&
                        a.ObjectKey == @snapshot.ObjectKey).FirstOrDefault();

                        if (_snapshot != null)
                        {
                            _snapshot.IsActive = false;
                            _dbContext.Update<Snapshot>(_snapshot);
                        }
                    break;
            }
            await _dbContext.SaveChangesAsync();
            _dbContext.Commit();
            return @snapshot;
        }
    }
}
