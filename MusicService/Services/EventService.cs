using MusicService.Data;
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
}