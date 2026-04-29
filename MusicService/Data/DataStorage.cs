using MusicService.Models;
using MusicService.Models.Events;

namespace MusicService.Data;

public class DataStorage
{
    public List<Event> Events { get; } = new();
    public List<User> Users { get; } = new();
    public List<Booking> Bookings { get; } = new();
    public List<Review> Reviews { get; } = new();
}