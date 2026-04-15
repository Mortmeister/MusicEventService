namespace MusicService.Models;

public class LoginService 
{
    private readonly UserRepository _repo;
    public User? CurrentUser { get; private set; } 

    public LoginService(UserRepository repo) 
    {
        _repo = repo;
    }

    public bool Login(string username, string password) 
    {
        var user = _repo.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        
   
        string hashedInput = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

        if (user != null && user.PasswordHash == hashedInput) 
        {
            CurrentUser = user;
            return true;
        }

        CurrentUser = null;
        return false;
    }

    public void Logout() => CurrentUser = null;
}