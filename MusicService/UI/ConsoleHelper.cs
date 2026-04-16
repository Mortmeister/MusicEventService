namespace MusicService.UI;

public class ConsoleHelper
{
    
    
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
    
    List<T> HandleEnumToList<T>(){
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
}