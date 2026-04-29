using MusicService.Models;
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
    public User? ShowGuestMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== Welcome ===");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Log in");
            Console.WriteLine("3. Exit");

            int choice = ConsoleHelper.GetValidChoice(1, 3);
            switch (choice)
            {
                case 1: Register(); break;
                case 2:
                    User? user = Login();
                    if (user != null) return user;
                    break;
                case 3:
                    Console.WriteLine("Goodbye!");
                    Exit();
                    return null;
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

    public User? Login()
    {
        string username = ConsoleHelper.GetValidString("Username: ");
        string password = ConsoleHelper.GetMaskedPassword("Password: ");

        var login = _loginService.Login(username, password);
        Console.WriteLine(login);
        return login;
    }

    public void Exit()
    {
        Console.WriteLine("Goodbye!");
        Environment.Exit(0);
    }
   
}