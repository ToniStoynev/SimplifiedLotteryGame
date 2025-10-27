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

    public void WelcomePlayer(Player player) => presentation.Present(MessageTemplates.WelcomePlayerTemplate(player, options.CurrentValue.TicketPrice));

    public void PurchaceTicket(Player player)
    {
        inputParser.TryParseInput(presentation.ReadInput(), out var ticketsCounts);
        player.Balance -= ticketsCounts * options.CurrentValue.TicketPrice;
        var playerOneTickets = Enumerable.Range(1, ticketsCounts).Select(_ => new Ticket(Guid.NewGuid(), 1)).ToList();
        _tickets.AddRange(playerOneTickets);
    }

    public void PurchaseTicketsForCpuPlayers()
    {
        var cpuPlayersNumber = randomGenerator.GenerateRandomNumberBetween(options.CurrentValue.MinNumberOfPlayers, options.CurrentValue.MaxNumberOfPlayers);
        
        for (var i = 2; i < cpuPlayersNumber + 2; i++)
        {
            var player = new Player { Id = i };
            var ticketsCount = randomGenerator.GenerateRandomNumberBetween(options.CurrentValue.MinNumberOfTicketsPerPlayer, options.CurrentValue.MaxNumberOfTicketsPerPlayer);
            var playerTickets = Enumerable.Range(1, ticketsCount).Select(_ => new Ticket(Guid.NewGuid(), player.Id)).ToList();
            _tickets.AddRange(playerTickets);
        }
        
        presentation.Present(MessageTemplates.CpuPlayersTicketPurchasedMessage(cpuPlayersNumber));
    }

    public void DeterminePrizes()
    {
        DetermineGrandPrize();
        DetermineSecondTier();
        DetermineThirdTier();
    }

    public void PresentResult()
    {
        var grandPrizeWinnerId = DetermineGrandPrize();
        var secondTierWinners = DetermineSecondTier();
        var thirdTierWinners = DetermineThirdTier();
        presentation.Present(MessageTemplates.ResultPresentationMessage(GrandPrize, grandPrizeWinnerId, secondTierWinners, thirdTierWinners,
            SecondTier, ThirdTier, HouseProfit,
            _tickets.Count, options.CurrentValue));
    }

    public void Run()
    {
        var player = new Player { Id = 1 };
        WelcomePlayer(player);
        PurchaceTicket(player);
        PurchaseTicketsForCpuPlayers();
        DeterminePrizes();
        PresentResult();
    }
    
    private int DetermineGrandPrize()
    {
       var randomIndex = randomGenerator.GenerateRandomNumberBetween(0, _tickets.Count);
       
       var ticket = _tickets[randomIndex];
       
       _tickets.RemoveAt(randomIndex);

      return ticket.PlayerId;
    }
    
    private Dictionary<int, int> DetermineSecondTier()
    {
        var winningTicketsNumber = (int)Math.Round(_tickets.Count * options.CurrentValue.SecondTierPrizeShareRatio);
        var secondTierWinningTickets= _tickets.OrderBy(_ => randomGenerator.GenerateRandomNumber()).Take(winningTicketsNumber).ToList();
        _tickets.RemoveAll(ticket => secondTierWinningTickets.Contains(ticket));
        return secondTierWinningTickets.GroupBy(x => x.PlayerId).ToDictionary(x => x.Key, x => x.Count());
    }
    
    private Dictionary<int, int> DetermineThirdTier()
    {
        var winningTicketsNumber = (int)Math.Round(_tickets.Count * options.CurrentValue.ThirdTierPrizeShareRatio);
        var thirdTierWinningTickets= _tickets.OrderBy(_ => randomGenerator.GenerateRandomNumber()).Take(winningTicketsNumber).ToList();
        return thirdTierWinningTickets.GroupBy(x => x.PlayerId).ToDictionary(x => x.Key, x => x.Count());
    }
}