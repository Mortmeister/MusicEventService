using MusicService.Enums;
using MusicService.Models.Events;

namespace MusicService.Models;

public class Booking
{
    public string BookingReference { get; } = $"BK-{Guid.NewGuid().ToString()[..5].ToUpper()}";
    public decimal PriceAtBooking { get; }
    public TicketType TicketType { get; }
    public DateTime DateBooked { get; }
    public Event Event { get; }
    public User User { get; }
    public BookingStatus Status { get; private set; }

    public void Cancel()
    {
        if (Status != BookingStatus.Confirmed)
        {
            throw new ArgumentException("Only confirmed bookings can be cancelled");
        }
        Status = BookingStatus.Cancelled;
    }
    public Booking(Event e, TicketType ticketType, User user){
        Event = e;
        TicketType = ticketType;
        User = user;
        PriceAtBooking = ticketType.Price;
        DateBooked = DateTime.Now;
    }
}