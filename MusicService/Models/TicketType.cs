namespace MusicService.Models;

public class TicketType
{
    public decimal Price { get; }
    public string Name { get; }
    public int TotalQuantity { get;}
    public int RemainingQuantity { get; private set; }
    public bool IsAvailable =>  RemainingQuantity > 0;


    public TicketType(decimal price, string name, int quantity)
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
        
        Price = price;
        Name = name;
        TotalQuantity = quantity;
    }
    
    public void Reserve()
    {
        if (RemainingQuantity <= 0)
        {
            throw new InvalidOperationException("Ticket type is already reserved");
        }
        RemainingQuantity--;
    }
    
    public void Release(){
        if (RemainingQuantity == TotalQuantity)
        {
            throw new InvalidOperationException("All tickets are already released");
        }
        
        RemainingQuantity++;
    }
}


