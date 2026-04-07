using MusicService.Enums;
namespace MusicService.Models.Events;

public class Festival : Event
{

    public int DurationInDays { get; }
    public List<string> LineUp { get; }

    public override string GetEventTypeName() => "Festival";
    
    public override string GetSummary()
    {
        string lineUp = string.Join(", ", LineUp);
        return $"{base.GetSummary()} || {DurationInDays} days || {lineUp} ";
    }

    public Festival(string title, string description, EventCategory category, DateTime date, string venue,
        User organiser, List<TicketType>? ticketTypes, List <string> lineUp, int durationInDays) : base(title, description,
        category, date, venue, organiser, ticketTypes)
    {
        if (durationInDays <= 0)
        {
            throw new ArgumentException("Festival duration must be greater than zero");
        }

        if (lineUp == null || lineUp.Count == 0)
        {
            throw new ArgumentException("Festival line up cannot be null or empty");
        }
        
        LineUp = lineUp;
        DurationInDays = durationInDays;
    }
}