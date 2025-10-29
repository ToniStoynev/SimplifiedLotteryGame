namespace SimplifiedLotteryGame.Domain;

public class Player(int id)
{
    private readonly List<Ticket> _tickets = new();

    public int Id { get; init; } = id;

    public decimal Balance { get; set; } = 10;

    public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();

    public void BuyTickets(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var ticket = new Ticket(Guid.NewGuid(), Id);

            if (Balance - ticket.Price < 0)
            {
                throw new InvalidOperationException("Insufficient funds! Can not buy ticket");
            }
            
            _tickets.Add(ticket);
        }
    }
}