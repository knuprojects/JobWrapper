namespace Vacancies.Application.Vacancies.Dto
{
    public class VacancyReaponseModel
    {
        public Guid Gid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string? Salary { get; set; }
    }
}
