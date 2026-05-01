using System;
using System.Linq;
using System.Collections.Generic;
using MusicService.Data;
using MusicService.Models;
namespace MusicService.Services;

/// <summary>
/// Represents the outcome of a registration attempt, including status and descriptive feedback.
/// </summary>
public class RegistrationResult 
{
    /// <summary>
    /// Gets or sets a value indicating whether the registration was successful.
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// Gets or sets a message providing details about the success or failure of the registration.
    /// </summary>
    public string Message { get; set; }
}
/// <summary>
/// Provides logic for registering new users within the music service system.
/// </summary>
public class RegisterService 
{
    private readonly DataStorage _repo;
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterService"/> class.
    /// </summary>
    /// <param name="repo">The data storage repository used to manage users.</param>
    public RegisterService(DataStorage repo) 
    {
        _repo = repo;
    }
    /// <summary>
    /// Attempts to register a new user by validating credentials and checking for duplicate usernames.
    /// </summary>
    /// <param name="username">The desired username for the new account.</param>
    /// <param name="password">The plain-text password to be hashed and stored.</param>
    /// <returns>A <see cref="RegistrationResult"/> indicating if the account was created or why it failed.</returns>
    public RegistrationResult Register(string username, string password) 
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return new RegistrationResult { Success = false, Message = "Fields cannot be empty." };

   
        if (_repo.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            return new RegistrationResult { Success = false, Message = "Username taken." };
       
        string hashed = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));


        var newUser = new User(username, hashed);

        _repo.Users.Add(newUser);

        return new RegistrationResult { Success = true, Message = "Account created!" };

    
    }
}