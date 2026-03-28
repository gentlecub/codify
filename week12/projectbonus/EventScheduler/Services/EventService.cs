using EventScheduler.Models;

namespace EventScheduler.Services;

public class EventService
{
    private readonly List<Event> _events = new();

    public void AddEvent(Event e)
    {
        if (e.Id == Guid.Empty)
        {
            e.Id = Guid.NewGuid();
        }
        _events.Add(e);
    }

    public List<Event> GetAllEvents()
    {
        return _events.OrderBy(e => e.StartTime).ToList();
    }

    public Event? GetEventById(Guid id)
    {
        return _events.FirstOrDefault(e => e.Id == id);
    }

    public bool DeleteEvent(Guid id)
    {
        var eventToRemove = _events.FirstOrDefault(e => e.Id == id);
        if (eventToRemove != null)
        {
            _events.Remove(eventToRemove);
            return true;
        }
        return false;
    }

    public bool UpdateEvent(Event updatedEvent)
    {
        var existingEvent = _events.FirstOrDefault(e => e.Id == updatedEvent.Id);
        if (existingEvent != null)
        {
            existingEvent.Title = updatedEvent.Title;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.StartTime = updatedEvent.StartTime;
            existingEvent.Duration = updatedEvent.Duration;
            return true;
        }
        return false;
    }

    public List<Event> GetConflictingEvents(Event newEvent)
    {
        return _events
            .Where(e => e.Id != newEvent.Id && Overlaps(newEvent, e))
            .OrderBy(e => e.StartTime)
            .ToList();
    }

    private static bool Overlaps(Event a, Event b)
    {
        return a.StartTime < b.EndTime && a.EndTime > b.StartTime;
    }

    public List<Event> GetTodayEvents()
    {
        var today = DateTime.Today;
        return _events
            .Where(e => e.StartTime.Date == today)
            .OrderBy(e => e.StartTime)
            .ToList();
    }

    public List<Event> GetThisWeekEvents()
    {
        var today = DateTime.Today;
        var dayOfWeek = today.DayOfWeek;
        var daysFromMonday = dayOfWeek == DayOfWeek.Sunday ? 6 : (int)dayOfWeek - 1;
        var startOfWeek = today.AddDays(-daysFromMonday);
        var endOfWeek = startOfWeek.AddDays(7);

        return _events
            .Where(e => e.StartTime.Date >= startOfWeek && e.StartTime.Date < endOfWeek)
            .OrderBy(e => e.StartTime)
            .ToList();
    }

    public List<Event> GetNext7DaysEvents()
    {
        var now = DateTime.Now;
        var endDate = now.AddDays(7);

        return _events
            .Where(e => e.StartTime >= now && e.StartTime <= endDate)
            .OrderBy(e => e.StartTime)
            .ToList();
    }

    public List<Event> SearchByName(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new List<Event>();
        }

        return _events
            .Where(e => e.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
            .OrderBy(e => e.StartTime)
            .ToList();
    }

    public Event? GetNextUpcomingEvent()
    {
        var now = DateTime.Now;
        return _events
            .Where(e => e.StartTime > now)
            .OrderBy(e => e.StartTime)
            .FirstOrDefault();
    }

    public int RemovePastEvents()
    {
        var today = DateTime.Today;
        var pastEvents = _events.Where(e => e.EndTime < today).ToList();
        foreach (var e in pastEvents)
        {
            _events.Remove(e);
        }
        return pastEvents.Count;
    }
}
