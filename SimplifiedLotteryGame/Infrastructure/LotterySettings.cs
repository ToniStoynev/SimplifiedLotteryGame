namespace SimplifiedLotteryGame;

public class LotterySettings
{
    public decimal TicketPrice { get; set; }

    public int MinNumberOfTicketsPerPlayer { get; set; }

    public int MaxNumberOfTicketsPerPlayer { get; set; }

    public int MinNumberOfPlayers { get; set; }

    public int MaxNumberOfPlayers { get; set; }

    public decimal GrandPrizePercentageOfTotalTicketsRevenue { get; set; }
    
    public decimal SecondTierPercentageOfTotalTicketsRevenue { get; set; }
    
    public decimal ThirdTierPercentageOfTotalTicketsRevenue { get; set; }

    public decimal SecondTierPrizeShareRatio { get; set; }

    public decimal ThirdTierPrizeShareRatio { get; set; }
}