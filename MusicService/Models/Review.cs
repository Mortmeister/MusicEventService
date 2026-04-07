using MusicService.Models.Events;

namespace MusicService.Models;

public class Review
{
    public User Author { get; }
    public Event Event { get; }
    public int Rating { get; }
    public string? Comment { get; }
    public DateTime DateReviewed { get; }

    public Review(User author, Event evt, int rating, string? comment)
    {
        if (rating is > 6 or < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 6");
        }
        
        Author = author ?? throw new ArgumentNullException(nameof(author), "Author cannot be null.");
        Event = evt ?? throw new ArgumentNullException(nameof(evt), "Event cannot be null.");
        Rating = rating;
        Comment = comment;
        DateReviewed = DateTime.Now;
    }
}