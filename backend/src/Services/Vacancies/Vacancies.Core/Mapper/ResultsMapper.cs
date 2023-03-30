using Mapster;
using Vacancies.Core.Entities;
using Vacancies.Core.Responses;

namespace Vacancies.Core.Mapper
{
    public class ResultsMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<VacancyResponse, Vacancy>()
                .RequireDestinationMemberSource(true); 
        }
    }
}
