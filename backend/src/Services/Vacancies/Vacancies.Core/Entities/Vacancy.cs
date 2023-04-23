using Shared.Abstractions.Primitives;

namespace Vacancies.Core.Entities;

public class Vacancy : Entity
{
    public Vacancy(string name, List<string> skills, string location, string salary, DateTime creationDate) : base()
    {
        Name = name;
        Skills = skills;
        Location = location;
        Salary = salary;
        CreationDate = creationDate;
    }
    public string Name { get; set; }
    public List<string> Skills { get; set; }
    public string Location { get; set; }
    public string? Salary { get; set; }
    public DateTime CreationDate { get; set; }
}