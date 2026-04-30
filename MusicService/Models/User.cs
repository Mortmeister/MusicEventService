using MusicService.Models.Events;

namespace MusicService.Models;
/// <summary>
/// Represents a registered user of the platform.
/// </summary>
public class User
{
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    
    public User(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty");
        }
        
        
        Username = username;
        PasswordHash = password;
    }
}