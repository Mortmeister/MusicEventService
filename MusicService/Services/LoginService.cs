using MusicService.Data;
using MusicService.Models;
namespace MusicService.Services;

public class LoginService 
{
    private readonly DataStorage _repo;
    public User? CurrentUser { get; private set; } 

    public LoginService(DataStorage repo) 
    {
        _repo = repo;
    }

    public User? Login(string username, string password) 
    {
        var user = _repo.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        
   
        string hashedInput = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

        if (user != null && user.PasswordHash == hashedInput) 
        {
            CurrentUser = user;
            return CurrentUser;
        }

        CurrentUser = null;
        return CurrentUser;
    }

    public void Logout() => CurrentUser = null;
}