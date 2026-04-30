using MusicService.Enums;

namespace MusicService.Models.Events;
// Concert.cs
/// <summary>
/// Represents a concert event with a set of performers and a genre.
/// </summary>
public class Concert : Event
{
    public List<string> Performers { get; private set; }
    public string Genre { get; private set; }
    public override string GetEventTypeName()=>"Concert";

    public override string GetSummary()
    {
        string performers = string.Join(", ", Performers);
        return $"{base.GetSummary()} || {performers} || {Genre} ";
    }

    /// <summary>Returns a formatted one-line summary including performers and genre.</summary>

    public Concert(string title, string description, EventCategory category, 
        DateTime date, string venue, User organiser, 
        List<TicketType>? ticketTypes, List<string> performers, string genre)
        : base(title, description, category, date, venue, organiser, ticketTypes)
    {
        if (performers == null || performers.Count == 0)
        {
            throw new ArgumentException("Performers cannot be null or empty");
        }
        
        Performers = performers;
        Genre = genre;
    }
    
    /// <summary>Returns a comma-separated string of performer names.</summary>

    public override string GetPerformers()
    {
        string performers = string.Join(", ", Performers);
        return performers;
    }
    /// <summary>
    /// Updates concert-specific fields.
    /// </summary>
    public void UpdateConcertDetails(List<string> performers, string genre)
    {
        
        if (performers == null || performers.Count == 0)
        {
            throw new ArgumentException("Performers cannot be null or empty");
        }
        
        Performers =  performers;
        Genre = genre;
    }
}