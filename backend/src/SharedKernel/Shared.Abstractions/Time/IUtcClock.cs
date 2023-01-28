namespace Shared.Abstractions.Time;

public interface IUtcClock
{
    DateTime GetCurrentUtcTime();
}

public class UtcClock : IUtcClock
{
    public DateTime GetCurrentUtcTime() => DateTime.UtcNow;
}
