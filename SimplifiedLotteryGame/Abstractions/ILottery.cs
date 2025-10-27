using SimplifiedLotteryGame.Domain;

namespace SimplifiedLotteryGame.Abstractions;

public interface ILottery
{
    void PurchaseTickets(Player player);

    int PurchaseTicketsForCpuPlayers();

    int DetermineGrandPrize();
    
    Dictionary<int, int> DetermineSecondTier();
    
    Dictionary<int, int> DetermineThirdTier();
    
    void Run();
}