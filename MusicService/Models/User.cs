using MusicService.Models.Events;

namespace MusicService.Models;


// A user has a username and password, a list of bookings they´ve made, a list of reviews that they made, and reviews they´ve received. 
// They also have a list of events they´ve published. 
public class User
{
    public string _username { get; private set; }
    public string _passwordHash { get; private set; }
    public List<Booking> BookingHistory { get;} = new List<Booking>();
    public List<Review> ReviewHistory { get;} = new List<Review>(); 
    public List<Event> MyEvents { get;} = new List<Event>();
    
    public User(string username, string password)
    {
        _username = username;
        _passwordHash = password;
    }
}