using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vacancies.Core.Entities;

namespace Vacancies.Persistence.Configurations
{
    public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            builder.HasKey(x => x.Gid);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Location).IsRequired();
            builder.Property(x => x.Salary);

            builder.HasOne(x => x.SocialRequest)
                .WithMany(x => x.Vacancies)
                .HasForeignKey(x => x.SocialRequestGid);
        }
    }
}
