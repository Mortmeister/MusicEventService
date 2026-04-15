using MusicService.Services;

namespace MusicService.UI.Menus;


public class GuestMenu
{
    private readonly RegisterService _registerService;
    private readonly LoginService _loginService;

    public GuestMenu(RegisterService registerService , LoginService loginService)
    {
        _registerService = registerService;
        _loginService = loginService;
    }
    public void ShowGuestMenu()
    {
        while(true){
            Console.WriteLine("Main menu");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Log in");
            Console.WriteLine("3. Exit");
            
            int choice = ConsoleHelper.GetValidChoice(1,3);
            switch (choice)
            {
                case 1: Register(); break;
                case 2: Login(); break;
                case 3: return;
            }
        }
    }

    public void Register()
    {
        string username = ConsoleHelper.GetValidString("Username: ");
        string password = ConsoleHelper.GetMaskedPassword("Password: ");

        var register = _registerService.Register(username, password);
        Console.WriteLine(register.Message);
    }

    public void Login()
    {
        string username = ConsoleHelper.GetValidString("Username: ");
        string password = ConsoleHelper.GetMaskedPassword("Password: ");
        
        var login = _loginService.Login(username, password);
        Console.WriteLine(login);
    }
   
}