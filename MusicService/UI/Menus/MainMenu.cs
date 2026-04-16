using MusicService.Data;
using MusicService.Enums;
using MusicService.Models;
using MusicService.Models.Events;
using MusicService.Services;

namespace MusicService.UI.Menus;

public class MainMenu
{
    private readonly EventService _eventService;
    /*private readonly BookingService _bookingService;
    private readonly ReviewService _reviewService;*/
    /*private readonly User _currentUser;*/

    public MainMenu(EventService eventService /*BookingService bookingService, 
        ReviewService reviewService*/ /*User currentUser*/)
    {
        _eventService = eventService;
        /*_bookingService = bookingService;
        _reviewService = reviewService;*/
        /*_currentUser = currentUser;*/
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

    public void CreateConcert()
    {
        //
    }
    public void CreateFestival()
    {
        //
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
}