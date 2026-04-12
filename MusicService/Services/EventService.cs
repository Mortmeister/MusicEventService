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
    
}