namespace SimplifiedLotteryGame.Domain;

public class Player
{
    public int Id { get; init; }

    public decimal Balance { get; set; } = 10;
    
    public List<Ticket> Tickets = [];
}