using MusicService.Data;
using MusicService.Enums;
using MusicService.Models;
using MusicService.Services;
using MusicService.Models.Events;
using MusicService.UI.Menus;

namespace MusicService;

class Program
{
    
    static void Main(string[] args)
    {
        
        var dataStorage = new DataStorage();
        var eventService = new EventService(dataStorage);
        var bookingService = new BookingService(dataStorage);
        var reviewService = new ReviewService(dataStorage);
        var registerService = new RegisterService(dataStorage);
        var loginService = new LoginService(dataStorage);
        var guestMenu = new GuestMenu(registerService, loginService);

        registerService.Register("test", "test");
        var testUser = dataStorage.Users.FirstOrDefault(u => u.Username == "test");
        
        
        var tickets = new List<TicketType>
        {
            new TicketType("Early Bird", 199, 50),
            new TicketType("Standard", 350, 100),
            new TicketType("VIP", 699, 20)
        };

        eventService.CreateConcert("Nordic Nights", "A great show", EventCategory.Pop,
            DateTime.Now.AddDays(30), "Spektrum, Oslo", testUser, tickets, 
            new List<string> { "Aurora", "Sigrid" }, "Indie Pop");

        eventService.CreateConcert("Iron Maiden Live", "Live from Oslo", EventCategory.Rock,
            DateTime.Now.AddDays(15), "Telenor Arena, Oslo", testUser, tickets,
            new List<string> { "Iron Maiden" }, "Rock");

        eventService.CreateConcert("Jazz Night", "Smooth jazz evening", EventCategory.Jazz,
            DateTime.Now.AddDays(60), "Blå, Oslo", testUser, tickets,
            new List<string> { "Nils Petter Molvær" }, "Jazz");

        eventService.CreateFestival("Øyafestivalen", "The greatest festival of all time", EventCategory.Pop,
            DateTime.Now.AddDays(45), "Tøyenparken, Oslo", testUser, tickets,
            new List<string> { "Metallica", "Aurora", "Sigrid" }, 3);

        eventService.CreateFestival("Norwegian Wood", "Classic rock festival", EventCategory.Rock,
            DateTime.Now.AddDays(20), "Frognerparken, Oslo", testUser, tickets,
            new List<string> { "Ole Ivars", "Hobnobs" }, 2);

        // seed a buyer with a few bookings so My Bookings has something to show
        registerService.Register("buyer", "buyer");
        var buyer = dataStorage.Users.FirstOrDefault(u => u.Username == "buyer");

        var ironMaiden = dataStorage.Events.First(e => e.Title == "Iron Maiden Live");
        var nordicNights = dataStorage.Events.First(e => e.Title == "Nordic Nights");
        var oyafestivalen = dataStorage.Events.First(e => e.Title == "Øyafestivalen");

        bookingService.CreateBooking(buyer, ironMaiden, ironMaiden.TicketTypes[1]);
        bookingService.CreateBooking(buyer, nordicNights, nordicNights.TicketTypes[2]);
        var toCancel = bookingService.CreateBooking(buyer, oyafestivalen, oyafestivalen.TicketTypes[0]);
        bookingService.CancelBooking(toCancel, buyer);


        while (true)
        {
            User? loggedIn = guestMenu.ShowGuestMenu();
            if (loggedIn != null)
            {
                var mainMenu = new MainMenu(eventService, bookingService,reviewService,loggedIn);
                mainMenu.ShowMainMenu();
            }
        }
    }
}