using Vacancies.Core.Entities;

namespace Vacancies.Core.Repositories
{
    public interface IVacancyRepository
    {
        public void AddOneVacancy(Vacancy vacancy);
        public void AddManyVacancies(List<Vacancy> vacancies);
    }
}
