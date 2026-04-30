using MusicService.Enums;

namespace MusicService.UI;

public class ConsoleHelper
{
    /// <summary>Prompts the user to select an event category from a numbered list.</summary>
    public static EventCategory SelectCategory(){
        
        var categories = HandleEnumToList<EventCategory>();
        
        Console.WriteLine("Select a category:");
        
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i]}");
        }
        
        return categories[ConsoleHelper.GetValidChoice(1, categories.Count)-1];
    }

    /// <summary>Loops until the user enters a valid integer within the given range.</summary>
    public static int GetValidChoice(int min, int max)
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int choice)
                && choice >= min && choice <= max)
            {
                return choice;
            }
            Console.Write("Invalid selection. Try again: ");
        }
    }
    
    /// <summary>Loops until the user enters a non-empty string.</summary>
    public static string GetValidString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if(!string.IsNullOrEmpty(input))
                return input;
            Console.WriteLine("This field cannot be empty.");
        }
    }
    
    /// <summary>
    /// Prompts for a string, showing the current value. Returns current value if input is empty.
    /// </summary>
    public static string GetValidStringPrefill(string prompt, string currentValue)
    {
        Console.Write($"{prompt} (current: {currentValue}): ");
        string? input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? currentValue : input;
    }
    
    /// <summary>Loops until the user enters a valid future date.</summary>
    public static DateTime GetDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (DateTime.TryParse(Console.ReadLine(), out DateTime date) && date > DateTime.Now)
                return date;
            Console.WriteLine("Please enter a valid future date.");
        }
    }
    
    /// <summary>
    /// Prompts for a date, showing the current value. Returns current value if input is empty or invalid.
    /// </summary>
    public static DateTime GetDatePrefill(string prompt, DateTime currentValue)
    {
        Console.Write($"{prompt} (current: {currentValue:dd MMM yyyy HH:mm}): ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) return currentValue;
        return DateTime.TryParse(input, out DateTime date) && date > DateTime.Now 
            ? date 
            : currentValue;
    }
    
    /// <summary>Loops until the user enters a valid decimal number for a price.</summary>
    public static decimal SetPrice()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if(!decimal.TryParse(input, out decimal price))
            {
                Console.WriteLine("Price must be a number");
                continue;
            }
            return price;
        }
    }
    
    /// <summary>Loops until the user enters a valid integer.</summary>
    public static int GetValidInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int result) )
                return result;
        }
    }
    /// <summary>Reads a password from the console, masking each character with an asterisk.</summary>
    public static string GetMaskedPassword(string prompt)
    {
        Console.Write(prompt);
        string password = "";
        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter) break;
            password += key.KeyChar;
            Console.Write("*");
        }
        Console.WriteLine();
        return password;
    }
    /// <summary>Converts all values of an enum type to a typed list.</summary>
    public static List<T> HandleEnumToList<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
    
    
}