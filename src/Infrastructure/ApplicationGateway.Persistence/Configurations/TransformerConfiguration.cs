using ApplicationGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationGateway.Persistence.Configurations
{
    public class TransformerConfiguration : IEntityTypeConfiguration<Transformer>
    {
        public void Configure(EntityTypeBuilder<Transformer> builder)
        {
            builder
                .Property(b => b.Gateway)
                .HasConversion<string>();
        }
    }
}
