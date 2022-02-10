using ApplicationGateway.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace ApplicationGateway.API.IntegrationTests.Base
{
    public class DbFixture : IDisposable
    {
        private readonly IdentityDbContext _identityDbContext;
        public readonly string IdentityDbName = $"Identity-{Guid.NewGuid()}";
        public readonly string HealthCheckDbName = $"HealthCheck";
        public readonly string HealthCheckConnString;
        public readonly string IdentityConnString;

        private bool _disposed;

        public DbFixture()
        {
            var identityBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
                    IdentityConnString = $"Server=localhost;Port=5432;Database={IdentityDbName};User Id=root;Password=root;CommandTimeout = 300;";

                    HealthCheckConnString = $"Server=localhost;Port=5432;Database={HealthCheckDbName};User Id=root;Password=root;CommandTimeout = 300;";
                    identityBuilder.UseNpgsql(IdentityConnString);

            _identityDbContext = new IdentityDbContext(identityBuilder.Options);
            _identityDbContext.Database.EnsureCreated();

            SeedIdentity seed = new SeedIdentity(_identityDbContext);
            seed.SeedUsers();
            seed.SeedUserRoles();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // remove the temp db from the server once all tests are done
                    _identityDbContext.Database.EnsureDeleted();
                }
                _disposed = true;
            }
        }
    }

    [CollectionDefinition("Database")]
    public class DatabaseCollection : ICollectionFixture<DbFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
