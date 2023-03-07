using Shared.Abstractions.Primitives;

namespace Vacancies.Core.Entities;

public class SocialRequest : Entity
{
    private readonly List<Vacancy> _vacancies = new();

    private SocialRequest(string uri) : base()
    {
        Uri = uri;
    }

    public string Uri { get; private set; }

    public ICollection<Vacancy>? Vacancies => _vacancies;

    public static SocialRequest Init(
        string uri)
    {
        return new SocialRequest(uri);
    }
}
