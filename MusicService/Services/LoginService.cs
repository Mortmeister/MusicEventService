using MusicService.Data;
using MusicService.Models;
namespace MusicService.Services;
/// <summary>
/// Provides functionality for authenticating users and managing the current session state.
/// </summary>

public class LoginService 
{
    private readonly DataStorage _repo;
    /// <summary>
    /// Gets the currently authenticated user, or null if no user is logged in.
    /// </summary>
    public User? CurrentUser { get; private set; } 
    

// <summary>
    /// Initializes a new instance of the <see cref="LoginService"/> class.
    /// </summary>
    /// <param name="repo">The data storage repository containing registered user information.</param>
    public LoginService(DataStorage repo) 
    {
        _repo = repo;
    }
    /// <summary>
    /// Attempts to authenticate a user by verifying their username and password against the repository.
    /// </summary>
    /// <param name="username">The username of the user attempting to log in.</param>
    /// <param name="password">The plain-text password to be validated.</param>
    /// <returns>The authenticated <see cref="User"/> if successful; otherwise, null.</returns>

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
    /// <summary>
    /// Ends the current user session by setting the CurrentUser to null.
    /// </summary>
    public void Logout() => CurrentUser = null;
}