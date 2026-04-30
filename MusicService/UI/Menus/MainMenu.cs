using MusicService.Data;
using MusicService.Enums;
using MusicService.Models;
using MusicService.Models.Events;
using MusicService.Services;

namespace MusicService.UI.Menus;

/// <summary>
/// The main navigation menu shown after login. Routes the user to all major features.
/// </summary>
public class MainMenu
{
    private readonly EventService _eventService;
    private readonly BookingService _bookingService;

    private readonly ReviewService _reviewService;

    private readonly User _currentUser;
    private readonly BookingMenu _bookingMenu;
    private readonly ReviewMenu _reviewMenu;


    public MainMenu(EventService eventService, BookingService bookingService,
        ReviewService reviewService, User currentUser, BookingMenu bookingMenu,  ReviewMenu reviewMenu)
    {
        _eventService = eventService;
        _bookingService = bookingService;
        _reviewService = reviewService;
        _currentUser = currentUser;
        _bookingMenu = bookingMenu;
        _reviewMenu = reviewMenu;
    }
    /// <summary>
    /// Displays the main menu in a loop until the user logs out.
    /// </summary>
    public void ShowMainMenu()
    {
        Console.WriteLine("Welcome to Music Service");

        while(true){

            Console.WriteLine("Where are you doing today?");
            Console.WriteLine("1. Browse Events");
            Console.WriteLine("2. Search for Events");
            Console.WriteLine("3. Create Event");
            Console.WriteLine("4. My Events");
            Console.WriteLine("5. My Bookings");
            Console.WriteLine("6. My reviews");
            Console.WriteLine("7. Logout");

            int choice = ConsoleHelper.GetValidChoice(1,7);
            switch (choice)
            {
                case 1: ShowAvailableEvents(); break;
                case 2: var search = new SearchMenu(_eventService, _bookingMenu, _currentUser);
                    search.ShowSearchMenu();
                    break;
                case 3: CreateEvent(); break;
                case 4: SeeMyEvents(); break;
                case 5: SeeMyBookings(); break;
                case 6: _reviewMenu.SeeMyReviews(); break;
                case 7: return;
            }
        }
    }
    
    /// <summary>Lists all upcoming events and allows the user to view details and book.</summary>
    public void ShowAvailableEvents()
    {
        var upcomingEvents = _eventService.GetUpcomingEvents();

        if (upcomingEvents.Count == 0)
        {
            Console.WriteLine("No events are currently available");
            return;
        }

        for (int i = 0; i < upcomingEvents.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {upcomingEvents[i].GetSummary()}");
        }
        Console.WriteLine($"{upcomingEvents.Count + 1}. Back");

        int choice = ConsoleHelper.GetValidChoice(1, upcomingEvents.Count + 1);
        if (choice == upcomingEvents.Count + 1) return;

        ShowEventDetails(upcomingEvents[choice - 1]);
    }
    
    /// <summary>Displays all available event categories.</summary>
    public void ShowCategories(){
        var categories = ConsoleHelper.HandleEnumToList<EventCategory>();

        int counter = 1;
        foreach (EventCategory category in categories)
        {
            Console.WriteLine($"{counter}: {category}");
            counter++;
        }
    }
    
    /// <summary>Prompts the user to choose between creating a Concert or Festival.</summary>
    public void CreateEvent()
    {
           while(true)
           {
                  Console.WriteLine("What type of event would you like to add?");
                  Console.WriteLine("1. Concert");
                  Console.WriteLine("2. Festival");
                  Console.WriteLine("3. Exit");

                  int choice = ConsoleHelper.GetValidChoice(1,3);
                  switch (choice)
                  {
                      case 1: CreateConcert(); break;
                      case 2: CreateFestival(); break;
                      case 3: return;
                  }
           }
    }
    
    /// <summary>Collects ticket type details in a loop and returns the list.</summary>
    private List<TicketType> AddTicketTypes()
    {

        var ticketType = new List <TicketType>();
        Console.WriteLine("Enter Ticket Type: ");
        while (true)
        {
            string name = ConsoleHelper.GetValidString("Enter name: ");
            Console.WriteLine("Set price");
            decimal price = ConsoleHelper.SetPrice();
            int quantity = ConsoleHelper.GetValidInt("Enter quantity: ");


            ticketType.Add(new TicketType(name, price, quantity));
            Console.WriteLine("Ticket type added.");

            Console.WriteLine("Add another ticket type?");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");

            int choice = ConsoleHelper.GetValidChoice(1,2);
            if (choice == 2) break;
        }
        return ticketType;
    }
    
    /// <summary>Collects concert details from the user and creates a new Concert via EventService.</summary>
    public void CreateConcert()
    {
        string title = ConsoleHelper.GetValidString("Enter a title: ");
        string description = ConsoleHelper.GetValidString("Enter a description: ");

        DateTime date = ConsoleHelper.GetDate("Enter a date: ");
        string venue = ConsoleHelper.GetValidString("Enter a venue: ");

        string genre = ConsoleHelper.GetValidString("Enter a genre: ");
        string performerInput = ConsoleHelper.GetValidString("Enter performers separated by commas: ");
        List<string> performers = performerInput.Split(',').Select(p => p.Trim()).ToList();


        _eventService.CreateConcert(title, description, ConsoleHelper.SelectCategory(),date, venue, _currentUser, AddTicketTypes(),performers, genre);
    }
    
    /// <summary>Collects festival details from the user and creates a new Festival via EventService.</summary>
    public void CreateFestival()
    {
    string title = ConsoleHelper.GetValidString("Enter a title: ");
    string description = ConsoleHelper.GetValidString("Enter a description: ");
    DateTime date = ConsoleHelper.GetDate("Enter a date: ");
    string venue = ConsoleHelper.GetValidString("Enter a venue: ");
    int durationInDays = ConsoleHelper.GetValidInt("Enter duration in days: ");
    string lineUpInput = ConsoleHelper.GetValidString("Enter lineup separated by commas: ");
    List<string> lineUp = lineUpInput.Split(',').Select(p => p.Trim()).ToList();

    _eventService.CreateFestival(title, description, ConsoleHelper.SelectCategory(),date, venue, _currentUser, AddTicketTypes(),lineUp, durationInDays);
    }
    
    /// <summary>Shows full details for a selected event, with options to book or go back.</summary>
    private void ShowEventDetails(Event evt)
    {
        while (true)
        {
            Console.WriteLine($"{evt.Title}");
            Console.WriteLine($"{evt.GetEventTypeName()}");
            Console.WriteLine($"Description: {evt.Description}");
            Console.WriteLine($"Lineup: {evt.GetPerformers()}");
            Console.WriteLine($"Date: {evt.Date:dd MMM yyyy HH:mm}");
            Console.WriteLine($"Venue: {evt.Venue}");
            Console.WriteLine($"Status: {evt.Status}");
            Console.WriteLine($"Category: {evt.Category}");

            Console.WriteLine("Ticket Types:");

            foreach (var ticket in evt.TicketTypes)
            {
                Console.WriteLine($"{ticket.Name}: {ticket.Price} kr ({ticket.RemainingQuantity} remaining) ");
            }

            Console.WriteLine("1. Book event");
            Console.WriteLine("2. Go back");

            int choice = ConsoleHelper.GetValidChoice(1, 3);
            switch (choice)
            {
                case 1: _bookingMenu.BookTicket(_currentUser, evt); 
                    Console.WriteLine("\nPress any key to continue..."); 
                    Console.ReadKey();                                       
                    break;
                case 2: return;
            }
        }
    }

    /// <summary>Opens the My Events menu for the current user.</summary>
    public void SeeMyEvents()
    {
        var _myEventsMenu = new MyEventsMenu(_eventService, _currentUser);
        _myEventsMenu.ShowMyEventsMenu();
    }

    /// <summary>Opens the My Bookings menu for the current user.</summary>
    public void SeeMyBookings()
    {
        var myBookingsMenu = new MyBookingsMenu(_bookingService,_reviewMenu, _currentUser);
        myBookingsMenu.ShowMyBookingsMenu();
    }
}
