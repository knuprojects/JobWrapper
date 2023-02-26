using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacancies.Core.Entities;

namespace Vacancies.Persistence
{
    public class VacanciesDbContext : DbContext
    {
        public DbSet<Vacancy>? Vacancies { get; set; }
        public DbSet<SocialRequest>? SocialRequests { get; set; }

        public VacanciesDbContext(DbContextOptions<VacanciesDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
            => builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
