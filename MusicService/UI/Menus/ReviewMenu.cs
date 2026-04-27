using MusicService.Models;
using MusicService.Models.Events;

namespace MusicService.UI.Menus;

public class ReviewMenu
{
    private readonly ReviewService _reviewService;
    private readonly User _currentUser;

    public ReviewMenu(ReviewService reviewService, User currentUser)
    {
        _reviewService = reviewService;
        _currentUser = currentUser;
    }

 

    
    public void SeeMyReviews()
    {
        Console.Clear();
        var myReviews = _reviewService.GetReviewsByUser(_currentUser);
        
        if (!myReviews.Any())
        {
            Console.WriteLine("You haven't written any reviews yet!");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        foreach (var review in myReviews)
        {
            Console.WriteLine($"Event: {review.Event.Title} | Rating: {review.Rating}/5");
            Console.WriteLine($"Comment: {review.Comment}");
            Console.WriteLine("-----------------------------");
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
        Console.Clear();
    }

     public void LeaveReview(Event selectedEvent)
    {
        string? errorMessage = "";


        if (!_reviewService.IsEligible(_currentUser, selectedEvent))
        {
            Console.Clear();
            Console.WriteLine("\n[!] Error: You cannot review this event.");
            Console.WriteLine("Requirements: Event must be finished, you must have a booking, and you cannot be the organizer.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

  
        int rating = 0;
        while (rating < 1 || rating > 5)
        {
            Console.Clear();
            Console.WriteLine($"--- Leaving a Review for: {selectedEvent.Title} ---");
            
            if (!string.IsNullOrEmpty(errorMessage)) 
            {
                Console.WriteLine($"\n[ATTENTION] {errorMessage}");
            }

            Console.Write("\nRate this event (1-5): ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out rating) || rating < 1 || rating > 5)
            {
                errorMessage = "Invalid entry. Please enter a whole number between 1 and 5.";
            }
        }

 
        errorMessage = ""; 
        string? choice = "";
        string? comment = null;

        while (choice != "1" && choice != "2")
        {
            Console.Clear();
            Console.WriteLine($"--- Reviewing: {selectedEvent.Title} ---");
            Console.WriteLine($"Current Rating: {rating}/5 Stars");

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.WriteLine($"\n[ATTENTION] {errorMessage}");
            }

            Console.WriteLine("\nWould you like to leave a comment?");
            Console.WriteLine("1. Yes, write a comment");
            Console.WriteLine("2. No, back to menu");
            
            Console.Write("\nChoice: ");
            choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter your comment: ");
                comment = Console.ReadLine();
            }
            else if (choice != "2")
            {
                errorMessage = "Please choose either 1 or 2.";
            }
        }

        
        bool success = _reviewService.CreateReview(_currentUser, selectedEvent, rating, comment);

        Console.Clear();
        if (success)
        {
            Console.WriteLine("\n[SUCCESS] Your review has been saved. Thank you!");
        }
        else
        {
            Console.WriteLine("\n[ERROR] Something went wrong while saving your review.");
        }

        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
    }
}