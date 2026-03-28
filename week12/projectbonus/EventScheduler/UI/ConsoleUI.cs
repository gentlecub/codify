using EventScheduler.Models;
using EventScheduler.Services;

namespace EventScheduler.UI;

public class ConsoleUI
{
    private readonly EventService _eventService;

    public ConsoleUI(EventService eventService)
    {
        _eventService = eventService;
    }

    public void Run()
    {
        while (true)
        {
            DisplayMenu();
            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    AddNewEvent();
                    break;
                case "2":
                    ViewAllEvents();
                    break;
                case "3":
                    ViewTodayEvents();
                    break;
                case "4":
                    ViewThisWeekEvents();
                    break;
                case "5":
                    ViewNext7DaysEvents();
                    break;
                case "6":
                    SearchEvents();
                    break;
                case "7":
                    EditEvent();
                    break;
                case "8":
                    DeleteEvent();
                    break;
                case "9":
                    ShowCountdown();
                    break;
                case "10":
                    CleanPastEvents();
                    break;
                case "11":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("         EVENT SCHEDULER");
        Console.WriteLine("========================================");
        Console.WriteLine("1.  Add New Event");
        Console.WriteLine("2.  View All Events");
        Console.WriteLine("3.  View Today's Events");
        Console.WriteLine("4.  View This Week's Events");
        Console.WriteLine("5.  View Next 7 Days");
        Console.WriteLine("6.  Search Events");
        Console.WriteLine("7.  Edit Event");
        Console.WriteLine("8.  Delete Event");
        Console.WriteLine("9.  Show Countdown to Next Event");
        Console.WriteLine("10. Clean Past Events");
        Console.WriteLine("11. Exit");
        Console.WriteLine("========================================");
        Console.Write("Enter your choice: ");
    }

    private void AddNewEvent()
    {
        Console.WriteLine("\n--- Add New Event ---");

        Console.Write("Title: ");
        var title = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrEmpty(title))
        {
            Console.WriteLine("Title cannot be empty.");
            return;
        }

        Console.Write("Description: ");
        var description = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Start Date and Time (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var startTime))
        {
            Console.WriteLine("Invalid date/time format.");
            return;
        }

        Console.Write("Duration in minutes: ");
        if (!int.TryParse(Console.ReadLine(), out var durationMinutes) || durationMinutes <= 0)
        {
            Console.WriteLine("Invalid duration.");
            return;
        }

        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            StartTime = startTime,
            Duration = TimeSpan.FromMinutes(durationMinutes)
        };

        var conflicts = _eventService.GetConflictingEvents(newEvent);
        if (conflicts.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nWarning: This event conflicts with the following events:");
            Console.ResetColor();
            foreach (var conflict in conflicts)
            {
                Console.WriteLine($"  - {conflict}");
            }

            Console.Write("\nDo you still want to add this event? (y/n): ");
            var confirm = Console.ReadLine()?.Trim().ToLower();
            if (confirm != "y")
            {
                Console.WriteLine("Event not added.");
                return;
            }
        }

        _eventService.AddEvent(newEvent);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Event added successfully!");
        Console.ResetColor();
    }

    private void ViewAllEvents()
    {
        Console.WriteLine("\n--- All Events ---");
        var events = _eventService.GetAllEvents();
        DisplayEventList(events);
    }

    private void ViewTodayEvents()
    {
        Console.WriteLine("\n--- Today's Events ---");
        var events = _eventService.GetTodayEvents();
        DisplayEventList(events);
    }

    private void ViewThisWeekEvents()
    {
        Console.WriteLine("\n--- This Week's Events ---");
        var events = _eventService.GetThisWeekEvents();
        DisplayEventList(events);
    }

    private void ViewNext7DaysEvents()
    {
        Console.WriteLine("\n--- Next 7 Days ---");
        var events = _eventService.GetNext7DaysEvents();
        DisplayEventList(events);
    }

    private void SearchEvents()
    {
        Console.WriteLine("\n--- Search Events ---");
        Console.Write("Enter search query: ");
        var query = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrEmpty(query))
        {
            Console.WriteLine("Search query cannot be empty.");
            return;
        }

        var events = _eventService.SearchByName(query);

        if (events.Count == 0)
        {
            Console.WriteLine("No events found matching your search.");
            return;
        }

        Console.WriteLine($"\nFound {events.Count} event(s):\n");

        foreach (var e in events)
        {
            DisplayEventWithHighlight(e, query);
        }
    }

    private void DisplayEventWithHighlight(Event e, string query)
    {
        var title = e.Title;
        var index = title.IndexOf(query, StringComparison.OrdinalIgnoreCase);

        Console.Write("  ");
        if (index >= 0)
        {
            Console.Write(title[..index]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(title.Substring(index, query.Length));
            Console.ResetColor();
            Console.Write(title[(index + query.Length)..]);
        }
        else
        {
            Console.Write(title);
        }

        Console.WriteLine($" ({e.StartTime:g} - {e.EndTime:t})");
        if (!string.IsNullOrEmpty(e.Description))
        {
            Console.WriteLine($"    {e.Description}");
        }
    }

    private void EditEvent()
    {
        Console.WriteLine("\n--- Edit Event ---");
        var events = _eventService.GetAllEvents();

        if (events.Count == 0)
        {
            Console.WriteLine("No events to edit.");
            return;
        }

        DisplayEventListWithIndex(events);

        Console.Write("\nEnter the number of the event to edit: ");
        if (!int.TryParse(Console.ReadLine(), out var index) || index < 1 || index > events.Count)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }

        var eventToEdit = events[index - 1];
        Console.WriteLine($"\nEditing: {eventToEdit}");
        Console.WriteLine("(Press Enter to keep current value)\n");

        Console.Write($"Title [{eventToEdit.Title}]: ");
        var newTitle = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(newTitle))
        {
            eventToEdit.Title = newTitle;
        }

        Console.Write($"Description [{eventToEdit.Description}]: ");
        var newDescription = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(newDescription))
        {
            eventToEdit.Description = newDescription;
        }

        Console.Write($"Start Time [{eventToEdit.StartTime:yyyy-MM-dd HH:mm}]: ");
        var startTimeInput = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(startTimeInput) && DateTime.TryParse(startTimeInput, out var newStartTime))
        {
            eventToEdit.StartTime = newStartTime;
        }

        Console.Write($"Duration in minutes [{eventToEdit.Duration.TotalMinutes}]: ");
        var durationInput = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(durationInput) && int.TryParse(durationInput, out var newDuration) && newDuration > 0)
        {
            eventToEdit.Duration = TimeSpan.FromMinutes(newDuration);
        }

        var conflicts = _eventService.GetConflictingEvents(eventToEdit);
        if (conflicts.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nWarning: This event now conflicts with:");
            Console.ResetColor();
            foreach (var conflict in conflicts)
            {
                Console.WriteLine($"  - {conflict}");
            }
        }

        _eventService.UpdateEvent(eventToEdit);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Event updated successfully!");
        Console.ResetColor();
    }

    private void DeleteEvent()
    {
        Console.WriteLine("\n--- Delete Event ---");
        var events = _eventService.GetAllEvents();

        if (events.Count == 0)
        {
            Console.WriteLine("No events to delete.");
            return;
        }

        DisplayEventListWithIndex(events);

        Console.Write("\nEnter the number of the event to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var index) || index < 1 || index > events.Count)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }

        var eventToDelete = events[index - 1];
        Console.Write($"Are you sure you want to delete '{eventToDelete.Title}'? (y/n): ");
        var confirm = Console.ReadLine()?.Trim().ToLower();

        if (confirm == "y")
        {
            _eventService.DeleteEvent(eventToDelete.Id);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Event deleted successfully!");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }

    private void ShowCountdown()
    {
        Console.WriteLine("\n--- Countdown to Next Event ---");
        var nextEvent = _eventService.GetNextUpcomingEvent();

        if (nextEvent == null)
        {
            Console.WriteLine("No upcoming events.");
            return;
        }

        var timeUntil = nextEvent.StartTime - DateTime.Now;

        Console.WriteLine($"\nNext Event: {nextEvent.Title}");
        Console.WriteLine($"Starts: {nextEvent.StartTime:g}");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Time remaining: {timeUntil.Days} days, {timeUntil.Hours} hours, {timeUntil.Minutes} minutes");
        Console.ResetColor();
    }

    private void CleanPastEvents()
    {
        Console.WriteLine("\n--- Clean Past Events ---");
        Console.Write("This will remove all events that have ended before today. Continue? (y/n): ");
        var confirm = Console.ReadLine()?.Trim().ToLower();

        if (confirm == "y")
        {
            var count = _eventService.RemovePastEvents();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Removed {count} past event(s).");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Operation cancelled.");
        }
    }

    private void DisplayEventList(List<Event> events)
    {
        if (events.Count == 0)
        {
            Console.WriteLine("No events found.");
            return;
        }

        Console.WriteLine();
        foreach (var e in events)
        {
            Console.WriteLine($"  {e.Title}");
            Console.WriteLine($"    {e.StartTime:g} - {e.EndTime:t} ({e.Duration.TotalMinutes} min)");
            if (!string.IsNullOrEmpty(e.Description))
            {
                Console.WriteLine($"    {e.Description}");
            }
            Console.WriteLine();
        }
    }

    private void DisplayEventListWithIndex(List<Event> events)
    {
        Console.WriteLine();
        for (int i = 0; i < events.Count; i++)
        {
            var e = events[i];
            Console.WriteLine($"  {i + 1}. {e.Title}");
            Console.WriteLine($"     {e.StartTime:g} - {e.EndTime:t}");
        }
    }
}
