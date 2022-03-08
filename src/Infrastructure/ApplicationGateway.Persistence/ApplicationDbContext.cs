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
        public DbSet<ApiDto> ApiDtos { get; set; } = null!;
        public DbSet<KeyDto> KeyDtos { get; set; } = null!;
        public DbSet<PolicyDto> PolicyDtos { get; set; } = null!;

        private IDbContextTransaction _transaction;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            
            modelBuilder.Entity<Transformer>().HasData(new Transformer
            {
                TransformerId = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}"),
                TemplateName = TemplateHelper.GETAPI_TEMPLATE,
                TransformerTemplate = File.ReadAllText(@$"JsonTransformers/Tyk/{TemplateHelper.GETAPI_TEMPLATE}.json"),
                Gateway = Gateway.Tyk,
                CreatedDate = DateTime.UtcNow
            });
            modelBuilder.Entity<Transformer>().HasData(new Transformer
            {
                TransformerId = Guid.Parse("{31EA7C6D-D731-47C4-AF4A-155BAF2E2ED4}"),
                TemplateName = TemplateHelper.CREATEAPI_TEMPLATE,
                TransformerTemplate = File.ReadAllText(@$"JsonTransformers/Tyk/{TemplateHelper.CREATEAPI_TEMPLATE}.json"),
                Gateway = Gateway.Tyk,
                CreatedDate = DateTime.UtcNow
            });
            modelBuilder.Entity<Transformer>().HasData(new Transformer
            {
                TransformerId = Guid.Parse("{3F243DD1-644E-410F-93D0-E7979BE9D629}"),
                TemplateName = TemplateHelper.UPDATEAPI_TEMPLATE,
                TransformerTemplate = File.ReadAllText(@$"JsonTransformers/Tyk/{TemplateHelper.UPDATEAPI_TEMPLATE}.json"),
                Gateway = Gateway.Tyk,
                CreatedDate = DateTime.UtcNow
            });
            modelBuilder.Entity<Transformer>().HasData(new Transformer
            {
                TransformerId = Guid.Parse("{C8A540F9-0601-4DFB-B4E6-4ADAC1D52123}"),
                TemplateName = TemplateHelper.GETPOLICY_TEMPLATE,
                TransformerTemplate = File.ReadAllText(@$"JsonTransformers/Tyk/{TemplateHelper.GETPOLICY_TEMPLATE}.json"),
                Gateway = Gateway.Tyk,
                CreatedDate = DateTime.UtcNow
            });
            modelBuilder.Entity<Transformer>().HasData(new Transformer
            {
                TransformerId = Guid.Parse("{79AB4897-947C-4638-8D38-526AC28C5BFD}"),
                TemplateName = TemplateHelper.POLICY_TEMPLATE,
                TransformerTemplate = File.ReadAllText(@$"JsonTransformers/Tyk/{TemplateHelper.POLICY_TEMPLATE}.json"),
                Gateway = Gateway.Tyk,
                CreatedDate = DateTime.UtcNow
            });
            modelBuilder.Entity<Transformer>().HasData(new Transformer
            {
                TransformerId = Guid.Parse("{176D16BE-6A5E-4914-8939-58CAC1F7E0F0}"),
                TemplateName = TemplateHelper.GETKEY_TEMPLATE,
                TransformerTemplate = File.ReadAllText(@$"JsonTransformers/Tyk/{TemplateHelper.GETKEY_TEMPLATE}.json"),
                Gateway = Gateway.Tyk,
                CreatedDate = DateTime.UtcNow
            });
            modelBuilder.Entity<Transformer>().HasData(new Transformer
            {
                TransformerId = Guid.Parse("{63EFDD05-A2B8-44F8-9589-86380A7052A1}"),
                TemplateName = TemplateHelper.CREATEKEY_TEMPLATE,
                TransformerTemplate = File.ReadAllText(@$"JsonTransformers/Tyk/{TemplateHelper.CREATEKEY_TEMPLATE}.json"),
                Gateway = Gateway.Tyk,
                CreatedDate = DateTime.UtcNow
            });
            modelBuilder.Entity<Transformer>().HasData(new Transformer
            {
                TransformerId = Guid.Parse("{D37832B5-8400-4A80-90B0-51B07DFAAF4A}"),
                TemplateName = TemplateHelper.UPDATEKEY_TEMPLATE,
                TransformerTemplate = File.ReadAllText(@$"JsonTransformers/Tyk/{TemplateHelper.UPDATEKEY_TEMPLATE}.json"),
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
