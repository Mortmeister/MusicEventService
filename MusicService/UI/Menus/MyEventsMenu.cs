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
        case 1: // EditEvent(evt); 
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
  
}