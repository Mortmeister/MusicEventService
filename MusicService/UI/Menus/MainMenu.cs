using MusicService.Data;
using MusicService.Enums;
using MusicService.Models;
using MusicService.Models.Events;
using MusicService.Services;

namespace MusicService.UI.Menus;

public class MainMenu
{
    private readonly EventService _eventService;
   
    /*private readonly BookingService _bookingService;*/
    private readonly ReviewService _reviewService;
    private readonly User _currentUser;

    public MainMenu(EventService eventService, /*BookingService bookingService,*/ 
        ReviewService reviewService, User currentUser)
    {
        _eventService = eventService;
        /*_bookingService = bookingService;*/
        _reviewService = reviewService;
        _currentUser = currentUser;
    }
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
                case 2: SearchForEvents(); break;
                case 3: CreateEvent(); break;
                case 4: SeeMyEvents(); break;
                case 5: SeeMyReviews(); break;
                case 6: SeeMyBookings(); break;
                case 7: return;
            }
        }
    }
    
    public void ShowAvailableEvents()
    {
        var upcomingEvents = _eventService.GetUpcomingEvents();

        if (upcomingEvents.Count == 0)
        {
            Console.WriteLine("No events are currently available");
            return;
        }
        
        foreach (var upcomingEvent in upcomingEvents)
        {
            Console.WriteLine(upcomingEvent.GetSummary());
        }
    }

    public void ShowCategories(){
        var categories = ConsoleHelper.HandleEnumToList<EventCategory>();
   
        int counter = 1;
        foreach (EventCategory category in categories)
        {
            Console.WriteLine($"{counter}: {category}");
            counter++;
        }
    }
    
    public EventCategory SelectCategory(){
        Console.WriteLine("Enter Category: ");
        ShowCategories();
        var categories = ConsoleHelper.HandleEnumToList<EventCategory>();
        return categories[ConsoleHelper.GetValidChoice(1, categories.Count)-1];
    }
    
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
    
    public void CreateConcert()
    {
        string title = ConsoleHelper.GetValidString("Enter a title: ");
        string description = ConsoleHelper.GetValidString("Enter a description: "); 
             
        DateTime date = ConsoleHelper.GetDate("Enter a date: ");
        string venue = ConsoleHelper.GetValidString("Enter a venue: "); 
   
        string genre = ConsoleHelper.GetValidString("Enter a genre: ");
        string performerInput = ConsoleHelper.GetValidString("Enter performers separated by commas: ");
        List<string> performers = performerInput.Split(',').Select(p => p.Trim()).ToList();

        
        _eventService.CreateConcert(title, description, SelectCategory(),date, venue, _currentUser, AddTicketTypes(),performers, genre);
    }
    public void CreateFestival()
    {
    string title = ConsoleHelper.GetValidString("Enter a title: ");
    string description = ConsoleHelper.GetValidString("Enter a description: "); 
    DateTime date = ConsoleHelper.GetDate("Enter a date: ");
    string venue = ConsoleHelper.GetValidString("Enter a venue: ");
    int durationInDays = ConsoleHelper.GetValidInt("Enter duration in days: ");
    string lineUpInput = ConsoleHelper.GetValidString("Enter lineup separated by commas: ");
    List<string> lineUp = lineUpInput.Split(',').Select(p => p.Trim()).ToList();
    
    _eventService.CreateFestival(title, description, SelectCategory(),date, venue, _currentUser, AddTicketTypes(),lineUp, durationInDays);
    }

    public void SeeMyEvents()
    {
        //
    }
    public void SearchForEvents()
    {
        //
    }
    public void SeeMyReviews()
    {
        //
    }
    public void SeeMyBookings()
    {
        //
    }
    
    public void LeaveReview(Event selectedEvent)
    {
        
        if (!_reviewService.IsEligible(_currentUser, selectedEvent))
        {
            Console.WriteLine("Error: You cannot review this event (You might be the organizer or it hasn't happened yet).");
            return; 
        }

        
        int rating = 0;
        while (rating < 1 || rating > 5)
        {
            Console.Write("Rate this event (1-5): ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out rating) || rating < 1 || rating > 5)
            {
                Console.WriteLine("Please enter a valid number between 1 and 5.");
            }
        }

        
        Console.Write("Leave a comment (Optional - press Enter to skip): ");
        string comment = Console.ReadLine();

        
        _reviewService.CreateReview(_currentUser, selectedEvent, rating, comment);
    
        Console.WriteLine("Success! Your review has been saved.");
        
    }
}