namespace EventScheduler.Models;

public class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime EndTime => StartTime + Duration;

    public override string ToString()
    {
        return $"{Title} ({StartTime:g} - {EndTime:t})";
    }
}
