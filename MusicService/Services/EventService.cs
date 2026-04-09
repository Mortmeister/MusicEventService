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
}