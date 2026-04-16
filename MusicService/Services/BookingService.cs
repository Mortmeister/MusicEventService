using MusicService.Data;
using MusicService.Models;
using MusicService.Models.Events;
using MusicService.Enums;

namespace MusicService.Services;

public class BookingService
{
    private DataStorage _storage;

    public BookingService(DataStorage storage)
    {
        _storage = storage;
    }

    public Booking CreateBooking(User user, Event evt, TicketType ticket)
    {
        // sjekk at brukeren ikke booker sitt eget event
        if (evt.Organiser == user)
            throw new InvalidOperationException("You cannot book your own event");

        if (evt.Status != EventStatus.Upcoming)
            throw new InvalidOperationException("Can only book upcoming events");

        if (!ticket.IsAvailable)
            throw new InvalidOperationException("This ticket type is sold out");

        ticket.Reserve();

        var booking = new Booking(evt, ticket, user);
        _storage.Bookings.Add(booking);
        return booking;
    }

    public void CancelBooking(Booking booking, User user)
    {
        if (booking.User != user)
            throw new InvalidOperationException("You can only cancel your own bookings");

        booking.Cancel();
        booking.TicketType.Release();
    }

    public List<Booking> GetBookingsForUser(User user)
    {
        return _storage.Bookings.Where(b => b.User == user).ToList();
    }
}
