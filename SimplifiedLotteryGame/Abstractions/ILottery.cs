using SimplifiedLotteryGame.Domain;

namespace SimplifiedLotteryGame.Abstractions;

public interface ILottery
{
    void WelcomePlayer(Player player);

    void PurchaceTicket(Player player);

    void PurchaseTicketsForCpuPlayers();

    void DeterminePrizes();

    void PresentResult();

    void Run();
}