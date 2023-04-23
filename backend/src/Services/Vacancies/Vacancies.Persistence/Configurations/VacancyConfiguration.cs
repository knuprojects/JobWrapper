using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vacancies.Core.Entities;

namespace Vacancies.Persistence.Configurations;

internal sealed class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
{
    public void Configure(EntityTypeBuilder<Vacancy> builder)
    {
        builder.HasKey(x => x.Gid);
        builder.Property(x => x.Name);
        builder.Property(x => x.Skills);
        builder.Property(x => x.Location);
        builder.Property(x => x.Salary);
        builder.Property(x => x.CreationDate);
    }
}