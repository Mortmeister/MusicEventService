using MusicService.Models;
using MusicService.Services;
using MusicService.Enums;
using MusicService.Models.Events;

namespace MusicService.UI.Menus;

public class SearchMenu
{
    private readonly EventService _eventService;

    public SearchMenu(EventService eventService)
    {
        _eventService = eventService;
    }

    public void ShowSearchMenu()
    {

        List<Event> currentResults = _eventService.GetAllEvents();
        string activeFilter = "All Events";

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"--- Browse & Search Events ({activeFilter}) ---");

            if (!currentResults.Any())
            {
                Console.WriteLine("No events found matching your criteria.");
            }
            else
            {
     
                for (int i = 0; i < currentResults.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {currentResults[i].GetSummary()}");
                }
            }

            Console.WriteLine("\n[K] Search Keyword | [G] Genre/Category | [T] Type (Concert/Festival) | [C] Clear Filters | [B] Back");
            Console.Write("\nSelect an event number to view details or choose a filter: ");
            
            string input = Console.ReadLine()?.ToUpper() ?? "";

   
            if (input == "B") break;

     
            if (input == "C")
            {
                currentResults = _eventService.GetAllEvents();
                activeFilter = "All Events";
                continue;
            }


            if (input == "K")
            {
                Console.Write("Enter keyword (Title/Venue): ");
                string keyword = Console.ReadLine() ?? "";
                currentResults = _eventService.FilterEventByKeyword(keyword);
                activeFilter = $"Keyword: {keyword}";
            }

 
            else if (input == "G")
            {
                Console.WriteLine("Select Category: 1. Pop | 2. Rock | 3. Jazz | 4. Metal");
                string catChoice = Console.ReadLine() ?? "";
                
       
                EventCategory? selectedCat = catChoice switch
                {
                    "1" => EventCategory.Pop,
                    "2" => EventCategory.Rock,
                    "3" => EventCategory.Jazz,
                    "4" => EventCategory.Metal,
                    _ => null
                };

                if (selectedCat.HasValue)
                {
                    currentResults = _eventService.FilterEventByCategory(selectedCat.Value);
                    activeFilter = $"Genre: {selectedCat}";
                }
            }


            else if (input == "T")
            {
                Console.WriteLine("Select Type: 1. Concerts | 2. Festivals");
                string typeChoice = Console.ReadLine() ?? "";

                if (typeChoice == "1")
                {
                    currentResults = _eventService.FilterEventByType<Concert>();
                    activeFilter = "Only Concerts";
                }
                else if (typeChoice == "2")
                {
                    currentResults = _eventService.FilterEventByType<Festival>();
                    activeFilter = "Only Festivals";
                }
            }

       
            else if (int.TryParse(input, out int index) && index > 0 && index <= currentResults.Count)
            {
                ShowEventDetails(currentResults[index - 1]);
            }
        }
    }

    private void ShowEventDetails(Event ev)
    {
        Console.Clear();
        Console.WriteLine("=== EVENT DETAILS ===");
        Console.WriteLine($"Title:       {ev.Title}");
        Console.WriteLine($"Category:    {ev.Category}");
        Console.WriteLine($"Date:        {ev.Date:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Venue:       {ev.Venue}");
        Console.WriteLine($"Description: {ev.Description}");
        
 
        if (ev is Concert concert)
        {
            Console.WriteLine($"Artist(s):   {string.Join(", ", concert.Performers)}");
        }
        else if (ev is Festival festival)
        {
            Console.WriteLine($"Duration:    {festival.DurationInDays} days");
        }

        Console.WriteLine("\nPress any key to return to the list...");
        Console.ReadKey();
    }
}