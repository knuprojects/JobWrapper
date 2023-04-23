using Microsoft.EntityFrameworkCore;
using Vacancies.Core.Entities;

namespace Vacancies.Persistence;

public class VacancyContext : DbContext
{
    public DbSet<Vacancy>? Vacancies { get; set; }

    public VacancyContext(DbContextOptions<VacancyContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}
