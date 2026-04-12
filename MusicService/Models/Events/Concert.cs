using MusicService.Enums;

namespace MusicService.Models.Events;

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