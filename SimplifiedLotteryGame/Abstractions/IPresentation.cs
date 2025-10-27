namespace SimplifiedLotteryGame.Abstractions;

public interface IPresentation
{
    void Present(string text);
    
    string ReadInput();

    void PresentResults(decimal grandPrize,
        int grandPrizeWinnerId, Dictionary<int, int> secondTierWinners, Dictionary<int, int> thirdTierWinners,
        decimal secondTier, decimal thirdTier, decimal houseProfit, int ticketsCount, LotterySettings lotterySettings);
}