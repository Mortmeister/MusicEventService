using MusicService.Enums;
using MusicService.Models;
using MusicService.Services;

namespace MusicService.UI.Menus;

/// <summary>
/// Menu where the user sees their bookings and can cancel or leave a review.
/// </summary>
public class MyBookingsMenu
{
    private readonly BookingService _bookingService;
    private readonly ReviewMenu _reviewMenu;
    private readonly User _currentUser;

    public MyBookingsMenu(BookingService bookingService, ReviewMenu reviewMenu, User currentUser)
    {
        _bookingService = bookingService;
        _reviewMenu = reviewMenu;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Shows the user's bookings split into upcoming, past and cancelled.
    /// </summary>
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

            var upcoming = bookings
                .Where(b => b.Status == BookingStatus.Confirmed && b.Event.Date > DateTime.Now)
                .ToList();

            var past = bookings
                .Where(b => b.Event.Date <= DateTime.Now && b.Status == BookingStatus.Confirmed)
                .ToList();

            var cancelled = bookings
                .Where(b => b.Status == BookingStatus.Cancelled)
                .ToList();


            Console.WriteLine("Upcoming:");
            if (upcoming.Count == 0)
                Console.WriteLine("  None");
            for (int i = 0; i < upcoming.Count; i++)
                Console.WriteLine($"  {i + 1}. {FormatBooking(upcoming[i])}");


            if (past.Count > 0)
            {
                Console.WriteLine("Past attended:");
                for (int i = 0; i < past.Count; i++)
                    Console.WriteLine($"  {i + 1}. {FormatBooking(past[i])}");
            }


            if (cancelled.Count > 0)
            {
                Console.WriteLine("\nCancelled:");
                foreach (var b in cancelled)
                    Console.WriteLine($"  - {FormatBooking(b)}");
            }


            Console.WriteLine();
            int option = 1;
            int cancelOption = -1;
            int reviewOption = -1;
            int backOption;

            if (upcoming.Count > 0)
            {
                Console.WriteLine($"{option}. Cancel an upcoming booking");
                cancelOption = option++;
            }
            if (past.Count > 0)
            {
                Console.WriteLine($"{option}. Leave a review for a past event");
                reviewOption = option++;
            }

            backOption = option;
            Console.WriteLine($"{backOption}. Go back");

            int choice = ConsoleHelper.GetValidChoice(1, backOption);

            if (choice == backOption) return;

            if (choice == cancelOption)
            {
                Console.Write($"Select an upcoming booking to cancel (1-{upcoming.Count}): ");
                int i = ConsoleHelper.GetValidChoice(1, upcoming.Count);
                CancelBooking(upcoming[i - 1]);
            }
            else if (choice == reviewOption)
            {
                Console.Write($"Select a past booking to review (1-{past.Count}): ");
                int i = ConsoleHelper.GetValidChoice(1, past.Count);
                _reviewMenu.LeaveReview(past[i - 1].Event);
            }
        }
    }

    private string FormatBooking(Booking b)
    {
        return $"{b.Event.Title} | {b.TicketType.Name} x{b.Quantity} | {b.TotalPrice} kr " +
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
