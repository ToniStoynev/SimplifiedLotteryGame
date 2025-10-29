namespace SimplifiedLotteryGame.Domain;

public record Ticket(Guid Id, int PlayerId, decimal Price = 1);