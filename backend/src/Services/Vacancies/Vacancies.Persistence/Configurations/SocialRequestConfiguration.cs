using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vacancies.Core.Entities;

namespace Vacancies.Persistence.Configurations;

internal sealed class SocialRequestConfiguration : IEntityTypeConfiguration<SocialRequest>
{
    public void Configure(EntityTypeBuilder<SocialRequest> builder)
    {
        builder.HasKey(x => x.Gid);
        builder.Property(x => x.Uri).IsRequired();
    }
}