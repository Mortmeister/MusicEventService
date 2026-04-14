using System;
using System.Linq;
using System.Collections.Generic;

namespace logincode;

public class RegistrationResult 
{
    public bool Success { get; set; }
    public string Message { get; set; }
}

public class RegisterService 
{
    private readonly UserRepository _repo;

    public RegisterService(UserRepository repo) 
    {
        _repo = repo;
    }

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