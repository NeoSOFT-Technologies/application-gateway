using ApplicationGateway.Identity;
using ApplicationGateway.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace ApplicationGateway.API.IntegrationTests.Base
{
    public class DbFixture : IDisposable
    {
        private readonly  ApplicationDbContext _applicationDbContext;
        private readonly IdentityDbContext _identityDbContext;
        public readonly string ApplicationDbName = $"Application-{Guid.NewGuid()}";
        public readonly string IdentityDbName = $"Identity-{Guid.NewGuid()}";
        public readonly string HealthCheckDbName = $"HealthCheck";
        public readonly string HealthCheckConnString;
        public readonly string ApplicationConnString;
        public readonly string IdentityConnString;

        private bool _disposed;

        public DbFixture()
        {
            var applicationBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var identityBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
            ApplicationConnString = $"Server=localhost;Port=5430;Database={ApplicationDbName};User Id=root;Password=root;CommandTimeout = 300;";
                    IdentityConnString = $"Server=localhost;Port=5430;Database={IdentityDbName};User Id=root;Password=root;CommandTimeout = 300;";

                    HealthCheckConnString = $"Server=localhost;Port=5430;Database={HealthCheckDbName};User Id=root;Password=root;CommandTimeout = 300;";
                    applicationBuilder.UseNpgsql(ApplicationConnString);
                    identityBuilder.UseNpgsql(IdentityConnString);
            _applicationDbContext = new ApplicationDbContext(applicationBuilder.Options);
            _applicationDbContext.Database.EnsureCreated();

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
                    _applicationDbContext.Database.EnsureDeleted();
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
