using MusicService.Enums;
using MusicService.Models;
using MusicService.Services;

namespace MusicService.UI.Menus;

public class MyBookingsMenu
{
    private readonly BookingService _bookingService;
    private readonly User _currentUser;

    public MyBookingsMenu(BookingService bookingService, User currentUser)
    {
        _bookingService = bookingService;
        _currentUser = currentUser;
    }

    public void ShowMyBookingsMenu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== My Bookings ===");

            var bookings = _bookingService.GetBookingsForUser(_currentUser);

            if (bookings.Count == 0)
            {
                Console.WriteLine("You haven't booked any events yet.");
                return;
            }

            // upcoming = confirmed bookings for events that haven't happened yet
            var upcoming = bookings
                .Where(b => b.Status == BookingStatus.Confirmed && b.Event.Date > DateTime.Now)
                .ToList();

            var past = bookings
                .Where(b => b.Status == BookingStatus.Cancelled || b.Event.Date <= DateTime.Now)
                .ToList();

            Console.WriteLine("Upcoming:");
            if (upcoming.Count == 0)
            {
                Console.WriteLine("  None");
            }
            for (int i = 0; i < upcoming.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {FormatBooking(upcoming[i])}");
            }

            if (past.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Past:");
                foreach (var b in past)
                {
                    Console.WriteLine($"   - {FormatBooking(b)}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("0. Go back");
            if (upcoming.Count > 0)
                Console.Write($"Select a booking to cancel (1-{upcoming.Count}) or 0 to go back: ");
            else
                Console.Write("Press 0 to go back: ");

            int choice = ConsoleHelper.GetValidChoice(0, upcoming.Count);
            if (choice == 0) return;

            CancelBooking(upcoming[choice - 1]);
        }
    }

    private string FormatBooking(Booking b)
    {
        return $"{b.Event.Title} | {b.TicketType.Name} | {b.PriceAtBooking} kr " +
               $"| booked {b.DateBooked:dd MMM yyyy} | {b.BookingReference} | {b.Status}";
    }

    private void CancelBooking(Booking booking)
    {
        Console.WriteLine();
        Console.WriteLine($"Cancel booking {booking.BookingReference} for {booking.Event.Title}?");
        Console.WriteLine("1. Yes");
        Console.WriteLine("2. No");

        int choice = ConsoleHelper.GetValidChoice(1, 2);
        if (choice != 1) return;

        try
        {
            _bookingService.CancelBooking(booking, _currentUser);
            Console.WriteLine("Booking cancelled successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not cancel booking: {ex.Message}");
        }
    }
}
