using Shared.Abstractions.Primitives;

namespace Vacancies.Core.Entities;

public class Vacancy : Entity
{
    private Vacancy(
        string name,
        string description,
        string location,
        string salary) : base()
    {
        Name = name;
        Description = description;
        Location = location;
        Salary = salary;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Location { get; private set; }
    public string Salary { get; private set; }

    public Guid SocialRequestGid { get; set; }
    public SocialRequest SocialRequest { get; set; }

    public static Vacancy Init(
        string name,
        string description,
        string location,
        string salary)
    {
        return new Vacancy(name, description, location, salary);
    }
}