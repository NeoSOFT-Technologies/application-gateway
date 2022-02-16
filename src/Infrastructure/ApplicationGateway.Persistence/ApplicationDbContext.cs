using ApplicationGateway.Application.Contracts;
using ApplicationGateway.Domain.Common;
using ApplicationGateway.Domain.TykData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Persistence
{
    public class ApplicationDbContext : DbContext
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

        public DbSet<Transformers> Transformers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            string transformer = "{\n  \"name\": \"#valueof(Name)\",\n  \"use_keyless\": true,\n  \"active\": true,\n  \"proxy\": {\n    \"listen_path\": \"#valueof(ListenPath)\",\n    \"target_url\": \"#valueof(TargetUrl)\",\n    \"strip_listen_path\": true\n  },\n  \"version_data\": {\n    \"not_versioned\": true,\n " +
                "   \"default_version\": \"Default\",\n    \"versions\": {\n      \"Default\": {\n        \"name\": \"Default\",\n        \"use_extended_paths\": true\n      }\n    }\n  }\n}";
            //seed data, added through migrations
            modelBuilder.Entity<Transformers>().HasData(new Transformers
            {
                Id = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}"),
                TemplateName = "CreateApi",
                TransformerTemplate = transformer,
                Gateway= "TykGateway"
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = _loggedInUserService.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = _loggedInUserService.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
