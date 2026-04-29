using MusicService.Data;
using MusicService.Enums;
using MusicService.Models;
using MusicService.Models.Events;
using MusicService.Services;

namespace MusicService.UI.Menus;

public class MyEventsMenu
{
  private readonly User _currentUser;
  private readonly EventService _eventService;

  public MyEventsMenu(EventService eventService, User currentUser)
  {
    _eventService = eventService;
    _currentUser = currentUser;
  }
  public void ShowMyEventsMenu()
  {
    Console.WriteLine("My Events");
    var myEvents = _eventService.GetMyEvents(_currentUser);
    
    var upcoming = myEvents.Where(e => e.Status == EventStatus.Upcoming).ToList();
    var cancelled = myEvents.Where(e => e.Status == EventStatus.Cancelled).ToList();
    var completed = myEvents.Where(e => e.Status == EventStatus.Completed).ToList();

    if (myEvents.Count == 0)
    {
      Console.WriteLine("You have no events listed at the moment");
    }
    
    Console.WriteLine("Upcoming events:");
    if (upcoming.Count == 0)
      Console.WriteLine(" None");
    for (int i = 0; i < upcoming.Count; i++)
    {
      Console.WriteLine($"{i + 1}. {upcoming[i].Title}");
    }
    
    
    if (completed.Count > 0)
      Console.WriteLine("Completed events:");
    for (int i = 0; i < completed.Count; i++)
    {
      Console.WriteLine($"{completed[i].Title}");
    }
    
    
    if (cancelled.Count > 0)
      Console.WriteLine("Cancelled events:");
    for (int i = 0; i < cancelled.Count; i++)
    {
      Console.WriteLine($"{cancelled[i].Title}");
    }
    
    Console.WriteLine("0. Go back");
    
    int choice = ConsoleHelper.GetValidChoice(0, upcoming.Count + 1);
    if (choice == 0) return;
    ShowEventDetails(upcoming[choice - 1]);
  }

  private void ShowEventDetails(Event evt)
  {
    while (true)
    {
      Console.WriteLine($"{evt.Title}");
      Console.WriteLine($"{evt.GetEventTypeName()}");
      Console.WriteLine($"Description: {evt.Description}");
      Console.WriteLine($"Lineup: {evt.GetPerformers()}");
      Console.WriteLine($"Date: {evt.Date:dd MMM yyyy HH:mm}");
      Console.WriteLine($"Venue: {evt.Venue}");
      Console.WriteLine($"Status: {evt.Status}");
      Console.WriteLine($"Category: {evt.Category}");
      
      Console.WriteLine("Ticket Types:");

      foreach (var ticket in evt.TicketTypes)
      {
        Console.WriteLine($"{ticket.Name}: {ticket.Price} kr ({ticket.RemainingQuantity} remaining) ");
      }
      
      Console.WriteLine("1. Edit event");
      Console.WriteLine("2. Cancel event");
      Console.WriteLine("3. Go back");
      
      int choice = ConsoleHelper.GetValidChoice(1, 3);
      switch (choice)
      {
        case 1: EditEvent(evt); 
          break;
        case 2:
          CancelEvent(evt); return;
        case 3: return;
      }
    }
  }
  
  private void CancelEvent(Event evt)
  {
    Console.WriteLine("Are you sure you want to cancel this event?");
    Console.WriteLine("1. Yes");
    Console.WriteLine("2. No");
    
    int choice = ConsoleHelper.GetValidChoice(1, 2);
    if (choice == 1)
    {
      try
      {
        _eventService.CancelEvent(evt.Id,  _currentUser);
        Console.WriteLine($"{evt.Title} cancelled");
      }
      catch (Exception e)
      {
        Console.WriteLine($"Could not cancel event {e.Message}");
      }
    }
  }


  public void EditEvent(Event evt)
  {
    if (evt is Festival festival)
    {
      EditFestival(festival);
    }
    else if (evt is Concert concert)
    {
      EditConcert(concert);
      
    }
  }
private void EditFestival(Festival festival)
{
    Console.WriteLine($"\n=== Editing: {festival.Title} ===");
    Console.WriteLine("Press Enter to keep current value.\n");

    string title = ConsoleHelper.GetValidStringPrefill("Title", festival.Title);
    string description = ConsoleHelper.GetValidStringPrefill("Description", festival.Description);
    DateTime date = ConsoleHelper.GetDatePrefill("Date", festival.Date);
    string venue = ConsoleHelper.GetValidStringPrefill("Venue", festival.Venue);

    string lineupInput = ConsoleHelper.GetValidStringPrefill(
        "Lineup (comma-separated)", string.Join(", ", festival.LineUp));
    List<string> lineup = lineupInput.Split(',').Select(p => p.Trim()).ToList();

    string durationInput = ConsoleHelper.GetValidStringPrefill(
        "Duration in days", festival.DurationInDays.ToString());
    int duration = int.TryParse(durationInput, out int d) && d > 0 ? d : festival.DurationInDays;

    try
    {
        _eventService.EditFestival(festival.Id, title, description,ConsoleHelper.SelectCategory(), date, venue, lineup, duration, _currentUser);
        Console.WriteLine("Festival updated successfully.");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Could not update festival: {ex.Message}");
    }
}

private void EditConcert(Concert concert)
{
  Console.WriteLine($"Editing: {concert.Title}");
  Console.WriteLine("Press Enter to keep current value.\n");

  string title = ConsoleHelper.GetValidStringPrefill("Title", concert.Title);
  string description = ConsoleHelper.GetValidStringPrefill("Description", concert.Description);
  DateTime date = ConsoleHelper.GetDatePrefill("Date", concert.Date);
  string venue = ConsoleHelper.GetValidStringPrefill("Venue", concert.Venue);
  string genre = ConsoleHelper.GetValidStringPrefill("Genre", concert.Genre);

  string performerInput = ConsoleHelper.GetValidStringPrefill(
    "Performers (comma-separated)", string.Join(", ", concert.Performers));
  List<string> performers = performerInput.Split(',').Select(p => p.Trim()).ToList();

  try
  {
    _eventService.EditConcert(concert.Id, title, description, ConsoleHelper.SelectCategory(), date, venue, performers,
      genre, _currentUser);
    Console.WriteLine("Concert updated successfully.");
  }
  catch (InvalidOperationException ex)
  {
    Console.WriteLine($"Could not update concert: {ex.Message}");
  }
}
}