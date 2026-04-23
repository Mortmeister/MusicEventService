using MusicService.Data;
using MusicService.Enums;
using MusicService.Models;
using MusicService.Models.Events;

namespace MusicService.Services;

public class EventService
{
    private readonly DataStorage _dataStorage;

    public EventService(DataStorage dataStorage)
    {
        _dataStorage = dataStorage;
    }
    
    public Event CreateEvent(Event evt)
    {
        _dataStorage.Events.Add(evt);
        return evt;
    }

    public void CreateConcert(string title, string description, EventCategory category, DateTime date, string venue, User organiser, List <TicketType> ticketTypes, List <string> performers, string genre)
    {
        var concert = new Concert(title, description, category, date, venue, organiser, ticketTypes, performers, genre);
        CreateEvent(concert);
    }

    public void CreateFestival(string title, string description, EventCategory category, DateTime date, string venue, User organiser, List <TicketType> ticketTypes, List <string> lineup, int durationInDays)
    {
        var festival = new Festival(title, description, category, date, venue, organiser, ticketTypes, lineup,
            durationInDays);
        CreateEvent(festival);
    }

    public void EditConcert(string eventId, string title, string description, EventCategory category, DateTime date, string venue, List<string> performers, string genre, User currentUser)
    {
        Concert? existing = GetEventById(eventId) as Concert;
        
        if (existing == null)
        {
            throw new InvalidOperationException($"Event with id {eventId} not found");
        }

        if (existing.Organiser != currentUser)
        {
            throw new InvalidOperationException("You can only edit your own events");
        }

        if (existing.Status != EventStatus.Upcoming)
        {
            throw new InvalidOperationException("You can only edit upcoming events");
        }
        /*public void Update(string title, string description, EventCategory category, DateTime date, string venue)*/
        existing.Update(title, description, category, date, venue);
        existing.UpdateConcertDetails(performers, genre);
    }
    public void EditFestival(string eventId, string title, string description, EventCategory category, DateTime date, string venue, List<string> lineup, int durationInDays, User currentUser)
    {
        Festival? existing = GetEventById(eventId) as Festival;
        
        if (existing == null)
        {
            throw new InvalidOperationException($"Event with id {eventId} not found");
        }

        if (existing.Organiser != currentUser)
        {
            throw new InvalidOperationException("You can only edit your own events");
        }

        if (existing.Status != EventStatus.Upcoming)
        {
            throw new InvalidOperationException("You can only edit upcoming events");
        }
        /*public void Update(string title, string description, EventCategory category, DateTime date, string venue)*/
        existing.Update(title, description, category, date, venue);
        existing.UpdateFestivalDetails(lineup, durationInDays);
    }

    public Event? GetEventById(string eventId)
    {
        Event? eventById = _dataStorage.Events.FirstOrDefault(e=>e.Id == eventId);
        return  eventById;
    }

    public void CancelEvent(string eventId, User currentUser)
    {
        Event? eventById = GetEventById(eventId);

        if (eventById == null)
        {
            throw new InvalidOperationException($"Event with id {eventId} not found");
        }
        
        if(eventById.Organiser.Username != currentUser.Username)
        {
            throw new InvalidOperationException("You can only cancel your own events");
        }
        
        eventById.Cancel();
    }

    public void AddTicketType(TicketType ticketType, Event currentEvent)
    {
        if (ticketType == null)
        {
            throw new ArgumentNullException(nameof(ticketType), "Ticket type cannot be empty");
        }

        if (currentEvent == null)
        {
            throw new ArgumentNullException(nameof(currentEvent), "Event cannot be empty");
        }

        if (currentEvent.Status != EventStatus.Upcoming)
        {
            throw new InvalidOperationException("You can only add ticketTypes to upcoming events");
        }
        
        if (currentEvent.TicketTypes.Any(ticket => ticket.Name.ToLower().Trim() == ticketType.Name.ToLower().Trim()))
        {
            throw new InvalidOperationException("Ticket type already exists");
        }
        
        currentEvent.TicketTypes.Add(ticketType);
    }
    
    public List <Event> GetUpcomingEvents()
    {
        return _dataStorage.Events.Where(e => e.Status == EventStatus.Upcoming).ToList();
    }

    public List<Event> FilterEventByCategory(EventCategory category)
    {
        return _dataStorage.Events.Where(e => e.Category == category).ToList();
    }
    
    public List<Event> FilterEventByKeyword(string keyword)
    {
        string lower = keyword.ToLower();
        return _dataStorage.Events
            .Where(e =>e.Title.ToLower().Contains(lower)
                || e.Description.ToLower().Contains(lower)
                || e.Venue.ToLower().Contains(lower))
            .ToList();
    }
    public List<Event> FilterEventByType<T>() where T : Event
    {
        return _dataStorage.Events
            .OfType<T>()
            .Cast<Event>()
            .ToList();
    }

    public List<Event> GetMyEvents(User currentUser)
    {
        return _dataStorage.Events.Where(e => e.Organiser.Username == currentUser.Username).ToList();
    }
}