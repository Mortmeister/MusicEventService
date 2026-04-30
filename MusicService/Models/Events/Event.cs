using MusicService.Enums;

namespace MusicService.Models.Events;
/// <summary>
/// Abstract base class for all event types. Contains shared fields, status management, and validation.
/// </summary>
public abstract class Event
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public  string Title { get; private set; }
    public  string Description { get; private set; }
    public EventCategory Category { get; private set; }
    public EventStatus Status { get; private set; } = EventStatus.Upcoming;
    public  DateTime Date { get; private set; }
    public  string Venue { get; private set; }
    public User Organiser { get; }
    public List <TicketType> TicketTypes { get; }

    
    /// <summary>
    /// Cancels the event. Only valid for upcoming events.
    /// </summary>
    public void Cancel()
    {
        if (Status != EventStatus.Upcoming)
        {
            throw new InvalidOperationException("Only upcoming events can be cancelled");
        }
        Status = EventStatus.Cancelled;
    } /// <summary>
    /// Completes the event. Only valid for upcoming events.
    /// </summary>

    public void Complete()
    {
        if (Status != EventStatus.Upcoming)
        {
            throw new InvalidOperationException("Only upcoming events can be completed");
        }
        Status = EventStatus.Completed;
    }

    // only used for seeding past events
    public void SetPastDate(DateTime pastDate)
    {
        if (pastDate >= DateTime.Now)
        {
            throw new ArgumentException("Date must be in the past");
        }
        Date = pastDate;
    }
    
    /// <summary>Returns the event type name, e.g. "Concert" or "Festival".</summary>
    public abstract string GetEventTypeName();

    /// <summary>Returns a formatted one-line summary for use in list views.</summary>
    public virtual string GetSummary()
    {
        string availability = TicketTypes.Any(t=>t.IsAvailable) ? "Available" : "Sold out";
        return $"{Title} || {availability} || {Date:dd/MM/yyyy} || {GetEventTypeName()}";
    }

    /// <summary>Returns a formatted string of performers or lineup acts.</summary>
    public abstract string GetPerformers();

    /// <summary>
    /// Initializes a new Event with the specified details.
    /// </summary>
    public Event( string title, string description, EventCategory category, DateTime date, string venue, User organiser, List <TicketType> ticketTypes)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException("Title cannot be empty");
        }

        if (string.IsNullOrEmpty(description))
        {
            throw new ArgumentException("Description cannot be empty");
        }

        if (string.IsNullOrEmpty(venue))
        {
            throw new ArgumentException("Venue cannot be empty");
        }

        if (date <= DateTime.Now)
        {
            throw new ArgumentException("Event date must be in the future", nameof(date));
        }

        if (ticketTypes == null || ticketTypes.Count == 0)
        {
            throw new ArgumentException("Ticket types cannot be empty", nameof(ticketTypes));
        }
        
        Title = title;
        Description = description;
        Category = category;
        Date = date;
        Venue = venue;
        Organiser = organiser ?? throw new ArgumentNullException(nameof(organiser), "Organiser cannot be null");
        TicketTypes = ticketTypes;
    }
    
    
    /// <summary>
    /// Updates shared event fields. Only valid for upcoming events.
    /// </summary>
    public void Update(string title, string description, EventCategory category, DateTime date, string venue)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException("Title cannot be empty");
        }

        if (string.IsNullOrEmpty(description))
        {
            throw new ArgumentException("Description cannot be empty");
        }

        if (string.IsNullOrEmpty(venue))
        {
            throw new ArgumentException("Venue cannot be empty");
        }

        if (date <= DateTime.Now)
        {
            throw new ArgumentException("Event date must be in the future", nameof(date));
        }
        
        Title = title;
        Description = description;
        Category = category;
        Date = date;
        Venue = venue;
    }
}