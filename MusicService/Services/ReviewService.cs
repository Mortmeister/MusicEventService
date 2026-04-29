using MusicService.Data;
using MusicService.Models;
using MusicService.Models.Events;

public class ReviewService
{
    private readonly DataStorage _repo;

    public ReviewService(DataStorage repo)
    {
        _repo = repo;
    }


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
    public List<Review> GetReviewsByUser(User user)
    {
        return _repo.Reviews.Where(r => r.Author == user).ToList();
    }
    public List<Review> GetReviewsReceivedByUser(User user)
    {
        
        return _repo.Reviews.Where(r => r.Event.Organiser == user).ToList();
    }
}