using MusicService.Enums;
using MusicService.Models.Events;

namespace MusicService.Models;

/// <summary>
/// Represents a ticket booking made by a user for an event.
/// </summary>
public class Booking
{
    public string BookingReference { get; } = $"BK-{Guid.NewGuid().ToString()[..5].ToUpper()}";
    public decimal PriceAtBooking { get; }
    public int Quantity { get; }
    public decimal TotalPrice => PriceAtBooking * Quantity;
    public TicketType TicketType { get; }
    public DateTime DateBooked { get; }
    public Event Event { get; }
    public User User { get; }
    public BookingStatus Status { get; private set; }

    /// <summary>
    /// Cancels this booking. Only valid for confirmed bookings.
    /// </summary>
    public void Cancel()
    {
        if (Status != BookingStatus.Confirmed)
        {
            throw new ArgumentException("Only confirmed bookings can be cancelled");
        }
        Status = BookingStatus.Cancelled;
    }
    
    /// <summary>
    /// Initializes a new Booking for the given event, ticket type, user, and quantity.
    /// </summary>
    public Booking(Event e, TicketType ticketType, User user, int quantity){
        Event = e;
        TicketType = ticketType;
        User = user;
        Quantity = quantity;
        PriceAtBooking = ticketType.Price;
        DateBooked = DateTime.Now;
    }
}