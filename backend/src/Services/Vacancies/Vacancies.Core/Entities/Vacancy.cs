using Shared.Abstractions.Primitives;

namespace Vacancies.Core.Entities
{
    public class Vacancy : Entity
    {
        public Vacancy() : base() { }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string? Salary { get; set; }

        public Guid SocialRequestGid { get; set; }
        public SocialRequest SocialRequest { get; set; }
    }
}
