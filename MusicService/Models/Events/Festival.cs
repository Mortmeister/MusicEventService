using MusicService.Enums;
namespace MusicService.Models.Events;

// <summary>
/// Represents a festival event with a lineup of acts and a duration in days.
/// </summary>
public class Festival : Event
{

    public int DurationInDays { get; private set; }
    public List<string> LineUp { get; private set; }

    public override string GetEventTypeName() => "Festival";
    
    public override string GetSummary()
    {
        string lineUp = string.Join(", ", LineUp);
        return $"{base.GetSummary()} || {DurationInDays} days || {lineUp} ";
    }

    public override string GetPerformers()
    {
        string lineUp = string.Join(", ", LineUp);
        return lineUp;
    }

    /// <summary>
    /// Initializes a new Festival with the specified details.
    /// </summary>
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
    /// <summary>
    /// Updates festival-specific fields.
    /// </summary>
    public void UpdateFestivalDetails(List<string> lineUp, int durationInDays)
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