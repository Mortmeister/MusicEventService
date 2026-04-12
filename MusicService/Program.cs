using MusicService.Data;
using MusicService.Enums;
using MusicService.Models;
using MusicService.Services;
using MusicService.Models.Events;

namespace MusicService;

class Program
{
    
    static void Main(string[] args)
    {
        
        
        var dataStorage = new DataStorage();
        var eventService = new EventService(dataStorage);
        
        /// Generated dummy variables for testing
        var testUser = new User("Arvid","Arvid");
        
        var tickets = new List<TicketType>
        {
            new TicketType("Early Bird", 199, 50),
            new TicketType("Standard", 350, 100),
            new TicketType("VIP", 699, 20)
        };
        
        
        // TESTS STARTS HERE: 

        
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

        
        void PrintTestTitle(string title) =>
            Console.WriteLine($"\n=== {title} ===");

        void PrintEvents(List<Event> events)
        {
            if (!events.Any())
            {
                Console.WriteLine("No results.");
                return;
            }
            foreach (var e in events)
                Console.WriteLine(e.GetSummary());
        }

        // --- Test GetUpcomingEvents (sorted by date) ---
        PrintTestTitle("All Upcoming Events (sorted by date)");
        PrintEvents(eventService.GetUpcomingEvents());
        // Expected: Iron Maiden (15d), Norwegian Wood (20d), Nordic Nights (30d), Øya (45d), Jazz Night (60d)

        // --- Test FilterByCategory ---
        PrintTestTitle("Filter by Rock");
        PrintEvents(eventService.FilterEventByCategory(EventCategory.Rock));
        // Expected: Iron Maiden, Norwegian Wood

        PrintTestTitle("Filter by Pop");
        PrintEvents(eventService.FilterEventByCategory(EventCategory.Pop));
        // Expected: Nordic Nights, Øyafestivalen

        // --- Test FilterByType ---
        PrintTestTitle("Filter by Concert");
        PrintEvents(eventService.FilterEventByType<Concert>());
        // Expected: Nordic Nights, Iron Maiden, Jazz Night

        PrintTestTitle("Filter by Festival");
        PrintEvents(eventService.FilterEventByType<Festival>());
        // Expected: Øyafestivalen, Norwegian Wood

        // --- Test FilterByKeyword ---
        PrintTestTitle("Search: 'oslo'");
        PrintEvents(eventService.FilterEventByKeyword("oslo"));
        // Expected: Nordic Nights, Iron Maiden, Jazz Night (all have Oslo in venue)

        PrintTestTitle("Search: 'iron maiden'");
        PrintEvents(eventService.FilterEventByKeyword("iron maiden"));
        // Expected: Iron Maiden Live (title match)

        PrintTestTitle("Search: 'greatest'");
        PrintEvents(eventService.FilterEventByKeyword("greatest"));
        // Expected: Øyafestivalen (description match)

        PrintTestTitle("Search: 'frogner'");
        PrintEvents(eventService.FilterEventByKeyword("frogner"));
        // Expected: Norwegian Wood (venue match)

        PrintTestTitle("Search: 'zzznomatch'");
        PrintEvents(eventService.FilterEventByKeyword("zzznomatch"));
        // Expected: No results.

        
    }
}