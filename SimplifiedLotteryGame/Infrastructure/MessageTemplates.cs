using SimplifiedLotteryGame.Domain;

namespace SimplifiedLotteryGame.Infrastructure;

public static class MessageTemplates
{
    public static string WelcomePlayerTemplate(Player player, decimal ticketPrice) => $"""
                                Welcome to the Bede Lottery, Player {player.Id}!
                                
                                * Your digital balance: ${player.Balance:F2}
                                * Ticket Price: ${ticketPrice:F2} each
                                
                                How many tickets do you want to buy, Player {player.Id}?
                                """;

    public const string InvalidInputMessage = "Invalid input! Please enter a numeric value!";

    public static string InvalidTicketNumberMessage(int minNumberOfTicketsPerPlayer, int maxNumberOfTicketsPerPlayer) 
        => $"You are limited to purchasing between {minNumberOfTicketsPerPlayer} and {maxNumberOfTicketsPerPlayer} tickets!";

    public static string InsufficientBalanceMessage => "Your balance is not enough to buy this amount of tickets!";

    public static string CpuPlayersTicketPurchasedMessage(int cpuPlayersNumber) =>
        $"""
         
         {cpuPlayersNumber} other CPU players also have purchased tickets.
         
         """;
    
    public static string ResultPresentationMessage(decimal grandPrize, 
        int grandPrizeWinnerId, Dictionary<int, int> secondTierWinners, Dictionary<int, int> thirdTierWinners,
        decimal secondTier, decimal thirdTier, decimal houseProfit, int ticketsCount, LotterySettings lotterySettings) => $"""
                                                          Ticket Draw Results:
                                                          
                                                          * Grand Prize: Player {grandPrizeWinnerId}(1) wins ${grandPrize:F2}!
                                                          * Second Tier: Players {string.Join(", ", secondTierWinners.Select(kvp => $"{kvp.Key}({kvp.Value})"))} win ${secondTier / (lotterySettings.SecondTierPrizeShareRatio * ticketsCount):F2} per winning ticket!
                                                          * Third Tier:  Players {string.Join(", ", thirdTierWinners.Select(kvp => $"{kvp.Key}({kvp.Value})"))} win ${thirdTier / (lotterySettings.ThirdTierPrizeShareRatio * ticketsCount):F2} per winning ticket!
                                                          
                                                          Congratulations to the winners!
                                                          
                                                          House Profit: ${houseProfit:F2}
                                                          """;
}