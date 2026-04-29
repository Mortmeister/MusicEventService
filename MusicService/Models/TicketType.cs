namespace MusicService.Models;

public class TicketType
{
    public decimal Price { get; }
    public string Name { get; }
    public int TotalQuantity { get;}
    public int RemainingQuantity { get; private set; }
    public bool IsAvailable =>  RemainingQuantity > 0;


    public TicketType(string name, decimal price, int quantity)
    {
        if(quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero");
        }

        if (price <= 0)
        {
            throw new ArgumentException("Price must be greater than zero");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty");
        }
        
        Name = name;
        Price = price;
        TotalQuantity = quantity;
        RemainingQuantity = quantity; 
    }
    
    public void Reserve(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero");
        }
        if (RemainingQuantity < quantity)
        {
            throw new InvalidOperationException("Not enough tickets available");
        }
        RemainingQuantity -= quantity;
    }

    public void Release(int quantity){
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero");
        }
        if (RemainingQuantity + quantity > TotalQuantity)
        {
            throw new InvalidOperationException("Cannot release more than total");
        }

        RemainingQuantity += quantity;
    }
}


