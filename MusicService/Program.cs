using MusicService.Data;
using MusicService.Enums;
using MusicService.Models;
using MusicService.Services;

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

        var performers = new List<string> { "Aurora", "Sigrid" };
        var lineUp = new List<string> { "Ole ivars", "Metalliva", "Hobnobs"};

        eventService.CreateConcert("Nordic Nights", "A great show", EventCategory.Pop,
            DateTime.Now.AddDays(30), "Spektrum, Oslo", testUser, tickets, performers, "Indie Pop");
        eventService.CreateFestival("Findings", "The greatest festival of all time", EventCategory.Classical, DateTime.Now.AddDays(30), "Spektrum", testUser, tickets, lineUp, 3 );
        
        
        // Testing that creating festival and concert is working. 
        Console.WriteLine(dataStorage.Events.First().GetSummary());
        Console.WriteLine(dataStorage.Events.Last().GetSummary());
        
    }
}