using MusicService.Data;
using MusicService.Enums;
using MusicService.Models;
using MusicService.Models.Events;
using MusicService.Services;

namespace MusicService.UI.Menus;

public class MainMenu
{
    private readonly EventService _eventService;
    private readonly BookingService _bookingService;
    
    private readonly ReviewService _reviewService;
    
    private readonly User _currentUser;


    public MainMenu(EventService eventService, BookingService bookingService,
        ReviewService reviewService, User currentUser)
    {
        _eventService = eventService;
        _bookingService = bookingService;
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
                case 5: SeeMyBookings(); break;
                case 6: SeeMyReviews(); break;
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

        
        _eventService.CreateConcert(title, description, ConsoleHelper.SelectCategory(),date, venue, _currentUser, AddTicketTypes(),performers, genre);
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
    
    _eventService.CreateFestival(title, description, ConsoleHelper.SelectCategory(),date, venue, _currentUser, AddTicketTypes(),lineUp, durationInDays);
    }

    public void SeeMyEvents()
    {
        var _myEventsMenu = new MyEventsMenu(_eventService, _currentUser);
        _myEventsMenu.ShowMyEventsMenu();
    }
    public void SearchForEvents()
    {
        //
    }
    public void SeeMyReviews()
    {
        Console.Clear(); 
        Console.WriteLine("\n--- Your Reviews ---");

        var myReviews = _reviewService.GetReviewsByUser(_currentUser);

        if (!myReviews.Any())
        {
            Console.WriteLine("You haven't written any reviews yet!");
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey(); 
            Console.Clear();
            return;
        }

        foreach (var review in myReviews)
        {
            
            Console.WriteLine($"Event: {review.Event.Title}"); 
            Console.WriteLine($"Rating: {review.Rating}/5 Stars");
            Console.WriteLine($"Comment: {review.Comment}");
            Console.WriteLine("-----------------------------");
        }

        Console.WriteLine("\nPress any key to return to the menu...");
        Console.ReadKey(); 
        Console.Clear();
        
    }
    public void SeeMyBookings()
    {
        var myBookingsMenu = new MyBookingsMenu(_bookingService, _currentUser);
        myBookingsMenu.ShowMyBookingsMenu();
    }
      public void LeaveReview(Event selectedEvent)
    {
        string? errorMessage = "";


        if (!_reviewService.IsEligible(_currentUser, selectedEvent))
        {
            Console.Clear();
            Console.WriteLine("\n[!] Error: You cannot review this event.");
            Console.WriteLine("Requirements: Event must be finished, you must have a booking, and you cannot be the organizer.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

  
        int rating = 0;
        while (rating < 1 || rating > 5)
        {
            Console.Clear();
            Console.WriteLine($"--- Leaving a Review for: {selectedEvent.Title} ---");
            
            if (!string.IsNullOrEmpty(errorMessage)) 
            {
                Console.WriteLine($"\n[ATTENTION] {errorMessage}");
            }

            Console.Write("\nRate this event (1-5): ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out rating) || rating < 1 || rating > 5)
            {
                errorMessage = "Invalid entry. Please enter a whole number between 1 and 5.";
            }
        }

 
        errorMessage = ""; 
        string? choice = "";
        string? comment = null;

        while (choice != "1" && choice != "2")
        {
            Console.Clear();
            Console.WriteLine($"--- Reviewing: {selectedEvent.Title} ---");
            Console.WriteLine($"Current Rating: {rating}/5 Stars");

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.WriteLine($"\n[ATTENTION] {errorMessage}");
            }

            Console.WriteLine("\nWould you like to leave a comment?");
            Console.WriteLine("1. Yes, write a comment");
            Console.WriteLine("2. No, back to menu");
            
            Console.Write("\nChoice: ");
            choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter your comment: ");
                comment = Console.ReadLine();
            }
            else if (choice != "2")
            {
                errorMessage = "Please choose either 1 or 2.";
            }
        }

        
        bool success = _reviewService.CreateReview(_currentUser, selectedEvent, rating, comment);

        Console.Clear();
        if (success)
        {
            Console.WriteLine("\n[SUCCESS] Your review has been saved. Thank you!");
        }
        else
        {
            Console.WriteLine("\n[ERROR] Something went wrong while saving your review.");
        }

        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
    }
}