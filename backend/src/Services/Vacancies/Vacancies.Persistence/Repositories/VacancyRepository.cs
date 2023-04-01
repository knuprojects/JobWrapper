using Vacancies.Core.Entities;
using Vacancies.Core.Repositories;

namespace Vacancies.Persistence.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly VacancyContext _dbContext;

        public VacancyRepository(VacancyContext dbContext)
            => _dbContext = dbContext;

        public void AddManyVacancies(List<Vacancy> vacancies) => _dbContext.AddRange(vacancies);

        public void AddOneVacancy(Vacancy vacancy) => _dbContext.Add(vacancy);
    }
}
