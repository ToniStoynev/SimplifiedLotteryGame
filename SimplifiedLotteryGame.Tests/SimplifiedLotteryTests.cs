using Microsoft.Extensions.Options;
using Moq;
using SimplifiedLotteryGame.Abstractions;
using SimplifiedLotteryGame.Domain;
using SimplifiedLotteryGame.Infrastructure;

namespace SimplifiedLotteryGame.Tests;

public class SimplifiedLotteryTests
{
    private readonly Mock<IOptionsMonitor<LotterySettings>> _lotterySettingsMock = new();
    private readonly Mock<IInputParser> _inputParserMock = new();
    private readonly Mock<IRandomGenerator> _randomGeneratorMock = new();
    private readonly Mock<IPresentation> _presentationMock = new();

    [Fact]
    public void SellTickets_Should_Add_All_Player_Tickets()
    {
        // Arrange
        const int playersCount = 3; 
        const int ticketsPerPlayer = 2;
        var lottery = new SimplifiedLottery(_lotterySettingsMock.Object, _inputParserMock.Object, 
            _randomGeneratorMock.Object, _presentationMock.Object);
        
        // Act
        BuyTickets(lottery, playersCount, ticketsPerPlayer);
        
        // Assert
        Assert.Equal(playersCount * ticketsPerPlayer, lottery.Tickets.Count);
    }

    
    [Fact]
    public void DrawGrandWinningTicket_Removes_And_Return_Winning_Ticket()
    {
        // Arrange
        const int playersCount = 5; 
        const int ticketsPerPlayer = 3;
        var lottery = new SimplifiedLottery(_lotterySettingsMock.Object, _inputParserMock.Object, 
            _randomGeneratorMock.Object, _presentationMock.Object);
        BuyTickets(lottery, playersCount, ticketsPerPlayer);
        
        _randomGeneratorMock.Setup(r => r.GenerateRandomNumberBetween(0, playersCount * ticketsPerPlayer)).Returns(1);

        // Act
        var ticket = lottery.DrawGrandWinningTicket();

        // Assert
        Assert.NotNull(ticket);
        Assert.Equal(1, ticket.PlayerId);
        Assert.Equal((playersCount * ticketsPerPlayer) - 1, lottery.Tickets.Count);
        Assert.DoesNotContain(ticket, lottery.Tickets);
    }

    [Fact]
    public void DrawSecondTierWinningTickets_ReturnsCorrectNumber_And_RemovesThem()
    {
        // Arrange
        const int playersCount = 5; 
        const int ticketsPerPlayer = 3;
        var lotterySettings = new LotterySettings
        {
            SecondTierPrizeShareRatio = 0.1m
        };
        var lottery = new SimplifiedLottery(_lotterySettingsMock.Object, _inputParserMock.Object, 
            _randomGeneratorMock.Object, _presentationMock.Object);
        BuyTickets(lottery, playersCount, ticketsPerPlayer);
        _lotterySettingsMock.Setup(ls => ls.CurrentValue).Returns(lotterySettings);
        _randomGeneratorMock.Setup(r => r.GenerateRandomNumber()).Returns(1);
    
        // Act
        var expectedWinningTicketsCount = Math.Round(lottery.Tickets.Count * lotterySettings.SecondTierPrizeShareRatio);
        var secondTierWinningTickets = lottery.DrawSecondTierWinningTickets();
        
        // Assert
        Assert.Equal(expectedWinningTicketsCount, secondTierWinningTickets.Count);
        Assert.All(secondTierWinningTickets, ticket => Assert.DoesNotContain(ticket, lottery.Tickets));
    }
    
    [Fact]
    public void DrawThirdTierWinningTickets_ReturnsCorrectNumber_And_RemovesThem()
    {
        // Arrange
        const int playersCount = 5; 
        const int ticketsPerPlayer = 3;
        var lotterySettings = new LotterySettings
        {
            ThirdTierPrizeShareRatio = 0.2m
        };
        var lottery = new SimplifiedLottery(_lotterySettingsMock.Object, _inputParserMock.Object, 
            _randomGeneratorMock.Object, _presentationMock.Object);
        BuyTickets(lottery, playersCount, ticketsPerPlayer);
        _lotterySettingsMock.Setup(ls => ls.CurrentValue).Returns(lotterySettings);
        _randomGeneratorMock.Setup(r => r.GenerateRandomNumber()).Returns(1);
    
        // Act
        var expectedWinningTicketsCount = Math.Round(lottery.Tickets.Count * lotterySettings.ThirdTierPrizeShareRatio);
        var thirdTierWinningTickets = lottery.DrawThirdTierWinningTickets();
        
        // Assert
        Assert.Equal(expectedWinningTicketsCount, thirdTierWinningTickets.Count);
        Assert.All(thirdTierWinningTickets, ticket => Assert.DoesNotContain(ticket, lottery.Tickets));
    }
    
    private static void BuyTickets(SimplifiedLottery lottery, int playersCount, int ticketsPerPlayer)
    {
        for (var i = 0; i < playersCount; i++)
        {
            var player = new Player(i + 1);
            player.BuyTickets(ticketsPerPlayer);
            lottery.SellTickets([player]);
        }
    }
}

