using MusicService.Data;
using MusicService.Models;
using MusicService.Models.Events;
/// <summary>
/// Handles the business logic for event reviews, including eligibility checks and storage.
/// </summary>
public class ReviewService
{
    private readonly DataStorage _repo;
    /// <summary>
    /// Initializes a new instance of the <see cref="ReviewService"/> class.
    /// </summary>
    /// <param name="repo">The data storage repository used for bookings and reviews.</param>
    public ReviewService(DataStorage repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Determines if a user is eligible to review a specific event.
    /// To be eligible: the event must be finished (based on VirtualTime), the user must have a booking,
    /// the user cannot be the organiser, and the user must not have reviewed it already.
    /// </summary>
    /// <param name="author">The user attempting to write the review.</param>
    /// <param name="evt">The event to be reviewed.</param>
    /// <returns>True if the user meets all requirements; otherwise, false.</returns>
    public bool IsEligible(User author, Event evt)
    {
        if (evt.Date > DateTime.Now) return false;
        if (evt.Organiser == author) return false;

        bool hasBooking = _repo.Bookings.Any(b => b.User == author && b.Event == evt);
        if (!hasBooking) return false;

        bool alreadyReviewed = _repo.Reviews.Any(r => r.Author == author && r.Event == evt);
        if (alreadyReviewed) return false;

        return true;
    }

    /// <summary>
    /// Validates eligibility and creates a new review for an event.
    /// </summary>
    /// <param name="author">The user writing the review.</param>
    /// <param name="evt">The event being rated.</param>
    /// <param name="rating">The numerical rating (typically 1-5).</param>
    /// <param name="comment">An optional text comment.</param>
    /// <returns>True if the review was successfully created and stored; otherwise, false.</returns>
    public bool CreateReview(User author, Event evt, int rating, string? comment)
    {
        
        if (!IsEligible(author, evt)) return false;

        try
        {
            Review newReview = new Review(author, evt, rating, comment);
            _repo.Reviews.Add(newReview);
            return true;
        }
        catch (Exception)
        {
           
            return false;
        }
    }
    /// <summary>
    /// Retrieves all reviews written by a specific user.
    /// </summary>
    /// <param name="user">The author of the reviews.</param>
    /// <returns>A list of reviews sent by the user.</returns>
    public List<Review> GetReviewsByUser(User user)
    {
        return _repo.Reviews.Where(r => r.Author == user).ToList();
    }
    /// <summary>
    /// Retrieves all reviews received for events organised by a specific user.
    /// </summary>
    /// <param name="user">The organiser of the events.</param>
    /// <returns>A list of reviews received for the user's events.</returns>
    public List<Review> GetReviewsReceivedByUser(User user)
    {
        
        return _repo.Reviews.Where(r => r.Event.Organiser == user).ToList();
    }
}