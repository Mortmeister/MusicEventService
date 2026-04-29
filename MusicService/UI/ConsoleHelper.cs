using MusicService.Enums;

namespace MusicService.UI;

public class ConsoleHelper
{
    
    public static EventCategory SelectCategory(){
        
        var categories = HandleEnumToList<EventCategory>();
        
        Console.WriteLine("Select a category:");
        
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i]}");
        }
        
        return categories[ConsoleHelper.GetValidChoice(1, categories.Count)-1];
    }

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
    public static string GetValidStringPrefill(string prompt, string currentValue)
    {
        Console.Write($"{prompt} (current: {currentValue}): ");
        string? input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? currentValue : input;
    }
    
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
    
    public static DateTime GetDatePrefill(string prompt, DateTime currentValue)
    {
        Console.Write($"{prompt} (current: {currentValue:dd MMM yyyy HH:mm}): ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) return currentValue;
        return DateTime.TryParse(input, out DateTime date) && date > DateTime.Now 
            ? date 
            : currentValue;
    }
    
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
    
    public static int GetValidInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int result) )
                return result;
        }
    }
    
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
    
    public static List<T> HandleEnumToList<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
    
    
}