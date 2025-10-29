using Microsoft.Extensions.Options;
using SimplifiedLotteryGame.Abstractions;
using SimplifiedLotteryGame.Domain;

namespace SimplifiedLotteryGame.Infrastructure;

public class SimplifiedLottery(
    IOptionsMonitor<LotterySettings> options,
    IInputParser inputParser,
    IRandomGenerator randomGenerator, 
    IPresentation presentation)
    : ILottery
{
    private readonly List<Ticket> _tickets = [];
    
    private decimal TotalTicketsRevenue => options.CurrentValue.TicketPrice * _tickets.Count;

    private decimal GrandPrize => options.CurrentValue.GrandPrizePercentageOfTotalTicketsRevenue * TotalTicketsRevenue;

    private decimal SecondTier => options.CurrentValue.SecondTierPercentageOfTotalTicketsRevenue * TotalTicketsRevenue;

    private decimal ThirdTier => options.CurrentValue.ThirdTierPercentageOfTotalTicketsRevenue * TotalTicketsRevenue;

    private decimal PrizeAllocation => GrandPrize + SecondTier + ThirdTier;

    private decimal HouseProfit => TotalTicketsRevenue - PrizeAllocation;
    
    public void SellTickets(ICollection<Player> players)
    {
        foreach (var player in players)
        {
            _tickets.AddRange(player.Tickets);
        }
    }
    
    public Ticket DrawGrandWinningTicket()
    {
        var randomIndex = randomGenerator.GenerateRandomNumberBetween(0, _tickets.Count);
       
        var ticket = _tickets[randomIndex];
       
        _tickets.Remove(ticket);

        return ticket;
    }
    
    public List<Ticket> DrawSecondTierWinningTickets()
    {
        var winningTicketsNumber = (int)Math.Round(_tickets.Count * options.CurrentValue.SecondTierPrizeShareRatio);
        var secondTierWinningTickets= _tickets.OrderBy(_ => randomGenerator.GenerateRandomNumber()).Take(winningTicketsNumber).ToList();
        _tickets.RemoveAll(ticket => secondTierWinningTickets.Contains(ticket));
        return secondTierWinningTickets;
    }
    
    public List<Ticket> DrawThirdTierWinningTickets()
    {
        var winningTicketsNumber = (int)Math.Round(_tickets.Count * options.CurrentValue.ThirdTierPrizeShareRatio);
        var thirdTierWinningTickets= _tickets.OrderBy(_ => randomGenerator.GenerateRandomNumber()).Take(winningTicketsNumber).ToList();
        return thirdTierWinningTickets;
    }
    
    public void Run()
    {
        var players = new List<Player>();
        var player = new Player(1);
        presentation.Present(MessageTemplates.WelcomePlayerTemplate(player, options.CurrentValue.TicketPrice));
        inputParser.TryParseInput(presentation.ReadInput(), out var ticketsCount);
        player.BuyTickets(ticketsCount);
        players.Add(player);
        
        var cpuPlayersNumber = randomGenerator.GenerateRandomNumberBetween(options.CurrentValue.MinNumberOfPlayers, options.CurrentValue.MaxNumberOfPlayers);

        for (var i = 2; i < cpuPlayersNumber + 2; i++)
        {
            var cpuPlayer = new Player(i);
            var cpuPlayerTicketsCount = randomGenerator
                .GenerateRandomNumberBetween(options.CurrentValue.MinNumberOfTicketsPerPlayer, options.CurrentValue.MaxNumberOfTicketsPerPlayer);
            cpuPlayer.BuyTickets(cpuPlayerTicketsCount);
            players.Add(cpuPlayer);
        }
        
        SellTickets(players);
        
        presentation.Present(MessageTemplates.CpuPlayersTicketPurchasedMessage(cpuPlayersNumber));
        
        var grandPrizeWinningTicket = DrawGrandWinningTicket();
        var secondTierWinningTickets= DrawSecondTierWinningTickets();
        var thirdTierWinningTickets= DrawThirdTierWinningTickets();
        
        presentation.PresentResults(GrandPrize, 
            grandPrizeWinningTicket.PlayerId, 
            secondTierWinningTickets.GroupBy(x => x.PlayerId).ToDictionary(x => x.Key, x => x.Count()), 
            thirdTierWinningTickets.GroupBy(x => x.PlayerId).ToDictionary(x => x.Key, x => x.Count()),
            SecondTier, ThirdTier, HouseProfit,
            _tickets.Count, options.CurrentValue);
    }
}