using MusicService.Models;
using MusicService.Models.Events;
using MusicService.Services;

namespace MusicService.UI.Menus;

public class BookingMenu
{
    private readonly BookingService _bookingService;

    public BookingMenu(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public void BookTicket(User user, Event evt)
    {
        Console.WriteLine();
        Console.WriteLine($"=== Book a Ticket - {evt.Title} ===");

        if (evt.TicketTypes.Count == 0)
        {
            Console.WriteLine("No ticket types available for this event.");
            return;
        }

        Console.WriteLine("Ticket Types:");
        for (int i = 0; i < evt.TicketTypes.Count; i++)
        {
            var t = evt.TicketTypes[i];
            string availability;
            if (t.IsAvailable)
                availability = $"{t.RemainingQuantity} remaining";
            else
                availability = "Sold out";
            Console.WriteLine($"  {i + 1}. {t.Name} - {t.Price} kr ({availability})");
        }

        
        Console.WriteLine($"  {evt.TicketTypes.Count + 1}. Cancel");
        Console.Write($"Select a ticket type (1-{evt.TicketTypes.Count + 1}): ");

        int choice = ConsoleHelper.GetValidChoice(1, evt.TicketTypes.Count + 1);
        if (choice == evt.TicketTypes.Count + 1) return;

        var selected = evt.TicketTypes[choice - 1];

        if (!selected.IsAvailable)
        {
            Console.WriteLine("Sorry, this ticket type is sold out.");
            return;
        }

        // ticket quanitity
        int max = Math.Min(20, selected.RemainingQuantity);
        Console.Write($"How many tickets? (1-{max}): ");
        int quantity = ConsoleHelper.GetValidChoice(1, max);

        try
        {
            var booking = _bookingService.CreateBooking(user, evt, selected, quantity);

            Console.WriteLine();
            Console.WriteLine("Booking confirmed!");
            Console.WriteLine($"  Event: {booking.Event.Title}");
            Console.WriteLine($"  Ticket: {booking.TicketType.Name} x{booking.Quantity} - {booking.TotalPrice} kr ({booking.PriceAtBooking} kr each)");
            Console.WriteLine($"  Booked: {booking.DateBooked:dd MMMM yyyy}");
            Console.WriteLine($"  Reference: {booking.BookingReference}");
            Console.WriteLine();
            Console.WriteLine("Payment is handled externally. Enjoy the event!");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"Booking failed: {ex.Message}");
        }
    }
}
