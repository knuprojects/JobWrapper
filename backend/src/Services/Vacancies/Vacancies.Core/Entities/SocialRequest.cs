using Shared.Abstractions.Primitives;

namespace Vacancies.Core.Entities
{
    public class SocialRequest : Entity
    {
        private readonly List<Vacancy> _vacancies = new();
        public SocialRequest() : base() { }

        public string Uri { get; set; }

        public ICollection<Vacancy>? Vacancies => _vacancies;
    }
}
