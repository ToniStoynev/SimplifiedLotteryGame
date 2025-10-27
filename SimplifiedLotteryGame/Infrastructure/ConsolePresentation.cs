using System.Globalization;
using SimplifiedLotteryGame.Abstractions;

namespace SimplifiedLotteryGame.Infrastructure;

public class ConsolePresentation : IPresentation
{
    public void Present(string text)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Console.WriteLine(text);
    }

    public string ReadInput() => Console.ReadLine()!;
    
    public void PresentResults(decimal grandPrize, 
        int grandPrizeWinnerId, Dictionary<int, int> secondTierWinners, Dictionary<int, int> thirdTierWinners,
        decimal secondTier, decimal thirdTier, decimal houseProfit, int ticketsCount, LotterySettings lotterySettings)
    {
        Console.WriteLine(MessageTemplates.ResultPresentationMessage(grandPrize, grandPrizeWinnerId, secondTierWinners, thirdTierWinners,
            secondTier, thirdTier, houseProfit,
            ticketsCount, lotterySettings));
    }
}