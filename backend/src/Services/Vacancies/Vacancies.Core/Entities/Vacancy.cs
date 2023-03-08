using Shared.Abstractions.Attributes;
using Shared.Abstractions.Primitives.Mongo;

namespace Vacancies.Core.Entities;

[BsonCollection("Vacancy")]
public class Vacancy : BaseDocument
{
    public string Name { get; set; }
    public List<string> Skills { get; set; }
    public string Location { get; set; }
    public string Salary { get; set; }
    public string RequestUri { get; set; }
}