using MusicService.Enums;

namespace MusicService.Models.Events;

public abstract class Event
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public  string Title { get; }
    public  string Description { get; }
    public EventCategory Category { get; }
    public EventStatus Status { get; private set; } = EventStatus.Upcoming;
    public  DateTime Date { get; }
    public  string Venue { get; }
    public User Organiser { get; }
    public List <TicketType> TicketTypes { get; }

    public void Cancel()
    {
        if (Status != EventStatus.Upcoming)
        {
            throw new InvalidOperationException("Only upcoming events can be cancelled");
        }
        Status = EventStatus.Cancelled;
    }

    public void Complete()
    {
        if (Status != EventStatus.Upcoming)
        {
            throw new InvalidOperationException("Only upcoming events can be completed");
        }
        Status = EventStatus.Completed;
    }

    public abstract string GetEventTypeName();

    public virtual string GetSummary()
    {
        string availability = TicketTypes.Any(t=>t.IsAvailable) ? "Available" : "Sold out";
        return $"{Title} || {availability} || {Date:dd/MM/yyyy} || {GetEventTypeName()}";
    }

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

        if (date <= Date)
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
}