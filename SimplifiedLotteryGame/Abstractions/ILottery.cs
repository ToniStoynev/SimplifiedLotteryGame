using SimplifiedLotteryGame.Domain;

namespace SimplifiedLotteryGame.Abstractions;

public interface  ILottery
{
    void SellTickets(ICollection<Player> players);

    Ticket DrawGrandWinningTicket();
    
    List<Ticket> DrawSecondTierWinningTickets();

    List<Ticket> DrawThirdTierWinningTickets();
    
    void Run();
}