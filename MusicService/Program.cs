using MusicService.Data;
using MusicService.Enums;
using MusicService.Models;
using MusicService.Services;
using MusicService.Models.Events;
using MusicService.UI.Menus;

namespace MusicService;

class Program
{
    
    static void Main(string[] args)
    {
        
        var dataStorage = new DataStorage();
        var eventService = new EventService(dataStorage);
        var registerService = new RegisterService(dataStorage);
        var loginService = new LoginService(dataStorage);
        var guestMenu = new GuestMenu(registerService, loginService);
        /*var mainMenu = new MainMenu(eventService);*/

        var testUser = registerService.Register("test", "test");
        
        

        while (true)
        {
            User? loggedIn = guestMenu.ShowGuestMenu();
            if (loggedIn != null)
            {
                var mainMenu = new MainMenu(eventService, loggedIn);
                mainMenu.ShowMainMenu();
            }
        }
    }
}