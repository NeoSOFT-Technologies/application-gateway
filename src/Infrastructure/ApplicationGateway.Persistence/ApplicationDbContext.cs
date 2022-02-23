using ApplicationGateway.Application.Contracts;
using ApplicationGateway.Domain.Common;
using ApplicationGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using ApplicationGateway.Application.Helper;

namespace ApplicationGateway.Persistence
{
    [ExcludeFromCodeCoverage]
    public partial class ApplicationDbContext : DbContext
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggedInUserService loggedInUserService)
            : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }

        public DbSet<Transformer> Transformers { get; set; }
        public virtual DbSet<Snapshot> Snapshots { get; set; } = null!;
        private IDbContextTransaction _transaction;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            
            modelBuilder.Entity<Transformer>().HasData(new Transformer
            {
                TransformerId = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}"),
                TemplateName = TemplateHelper.CREATEAPI_TEMPLATE,
                TransformerTemplate = File.ReadAllText(@$"JsonTransformers/Tyk/{TemplateHelper.CREATEAPI_TEMPLATE}.json"),
                Gateway = Gateway.Tyk,
                CreatedDate = DateTime.UtcNow
            });
            modelBuilder.Entity<Snapshot>(entity =>
            {
                entity.ToTable("Snapshot");

                entity.Property(e => e.Id).HasIdentityOptions();

                entity.Property(e => e.Comment)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Gateway)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectName)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });


        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _loggedInUserService.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = _loggedInUserService.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                SaveChangesAsync();
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }
    }
}
