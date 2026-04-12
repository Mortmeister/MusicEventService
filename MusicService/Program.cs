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

        
    
        string concertId = dataStorage.Events
            .OfType<Concert>()
            .First()
            .Id;

        Console.WriteLine($"\n=== Before Edit ===");
        Console.WriteLine(dataStorage.Events.First(e => e.Id == concertId).GetSummary());

        eventService.EditConcert(
            concertId,
            "Nordic Nights EDITED",
            "An even greater show",
            EventCategory.Classical,
            DateTime.Now.AddDays(40),
            "Oslo Spektrum",
            new List<string> { "Aurora", "Sigrid", "Kygo" },
            "Indie Electronic",
            testUser
        );

        Console.WriteLine($"\n=== After Edit ===");
        Console.WriteLine(dataStorage.Events.First(e => e.Id == concertId).GetSummary());       
    }
}