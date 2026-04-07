using MusicService.Enums;

namespace MusicService.Models.Events;

public class Concert : Event
{
    public List<string> Performers { get;}
    public string Genre { get;}
    public override string GetEventTypeName()
    {
        return "Concert";
    }

    public override string GetSummary()
    {
        string performers = string.Join(", ", Performers);
        return $"{base.GetSummary()} || {Performers} || {Genre} ";
    }
    
    public Concert(string title, string description, EventCategory category, 
        DateTime date, string venue, User organiser, 
        List<TicketType>? ticketTypes, List<string> performers, string genre)
        : base(title, description, category, date, venue, organiser, ticketTypes)
    {
        Performers = performers;
        Genre = genre;
    }
}