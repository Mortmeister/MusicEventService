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
        var registerService = new RegisterService(dataStorage);
        var reviewService = new ReviewService(dataStorage);
        var loginService = new LoginService(dataStorage);
        var guestMenu = new GuestMenu(registerService, loginService);
        var bookingMenu = new BookingMenu(bookingService);


        registerService.Register("test", "test");
        registerService.Register("test2", "test2");
        registerService.Register("test3", "test3");
        var testUser = dataStorage.Users.FirstOrDefault(u => u.Username == "test");
        var testUser2 = dataStorage.Users.FirstOrDefault(u => u.Username == "test2");
        var testUser3 = dataStorage.Users.FirstOrDefault(u => u.Username == "test3");


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
            DateTime.Now.AddDays(15), "Telenor Arena, Oslo", testUser2, tickets,
            new List<string> { "Iron Maiden" }, "Rock");

        eventService.CreateConcert("Jazz Night", "Smooth jazz evening", EventCategory.Jazz,
            DateTime.Now.AddDays(60), "Blå, Oslo", testUser3, tickets,
            new List<string> { "Nils Petter Molvær" }, "Jazz");

        eventService.CreateFestival("Øyafestivalen", "The greatest festival of all time", EventCategory.Pop,
            DateTime.Now.AddDays(45), "Tøyenparken, Oslo", testUser, tickets,
            new List<string> { "Metallica", "Aurora", "Sigrid" }, 3);

        eventService.CreateFestival("Norwegian Wood", "Classic rock festival", EventCategory.Rock,
            DateTime.Now.AddDays(20), "Frognerparken, Oslo", testUser, tickets,
            new List<string> { "Ole Ivars", "Hobnobs" }, 2);

        // Past event for testing reviews
        var pastTickets = new List<TicketType>
        {
            new TicketType("Standard", 400, 200)
        };
        eventService.CreateConcert("Last Year's Show", "A show from earlier", EventCategory.Rock,
            DateTime.Now.AddDays(7), "Sentrum Scene, Oslo", testUser3, pastTickets,
            new List<string> { "Kvelertak" }, "Rock");


        registerService.Register("buyer", "buyer");
        var buyer = dataStorage.Users.FirstOrDefault(u => u.Username == "buyer");

        var ironMaiden = dataStorage.Events.First(e => e.Title == "Iron Maiden Live");
        var nordicNights = dataStorage.Events.First(e => e.Title == "Nordic Nights");
        var oyafestivalen = dataStorage.Events.First(e => e.Title == "Øyafestivalen");
        var pastShow = dataStorage.Events.First(e => e.Title == "Last Year's Show");

        bookingService.CreateBooking(buyer, ironMaiden, ironMaiden.TicketTypes[1], 2);
        bookingService.CreateBooking(buyer, nordicNights, nordicNights.TicketTypes[2], 1);
        var toCancel = bookingService.CreateBooking(buyer, oyafestivalen, oyafestivalen.TicketTypes[0], 3);
        bookingService.CancelBooking(toCancel, buyer);

        // backdate so buyer has something to review
        bookingService.CreateBooking(buyer, pastShow, pastShow.TicketTypes[0], 1);
        pastShow.SetPastDate(DateTime.Now.AddDays(-7));
        pastShow.Complete();


        while (true)
        {
            User? loggedIn = guestMenu.ShowGuestMenu();
            if (loggedIn != null)
            {
                var reviewMenu = new ReviewMenu(reviewService, loggedIn);
                var mainMenu = new MainMenu(eventService,bookingService,reviewService,loggedIn,bookingMenu, reviewMenu);
                mainMenu.ShowMainMenu();
            }
        }
    }
}
