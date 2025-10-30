using Microsoft.Extensions.Options;
using Moq;
using SimplifiedLotteryGame.Abstractions;
using SimplifiedLotteryGame.Infrastructure;

namespace SimplifiedLotteryGame.Tests;

public class TicketNumberParserTests
{
    private readonly Mock<IPresentation> _presentationMock = new();
    private readonly Mock<IOptionsMonitor<LotterySettings>> _lotterySettingsMock = new();
    
    [Fact]
    public void TryParseInput_InvalidInput_PromptsUntilValid()
    {
        // Arrange
        _lotterySettingsMock.Setup(ls => ls.CurrentValue).Returns(new LotterySettings
        {
            MinNumberOfTicketsPerPlayer = 1,
            MaxNumberOfTicketsPerPlayer = 10
        });
        _presentationMock.SetupSequence(p => p.ReadInput())
            .Returns("abc") 
            .Returns("2");

        var parser = new TicketNumberParser(_presentationMock.Object, _lotterySettingsMock.Object);

        // Act
        var result = parser.TryParseInput("xyz", out var output);

        // Assert
        Assert.True(result);
        Assert.Equal(2, output);
        _presentationMock.Verify(p => p.Present(MessageTemplates.InvalidInputMessage), Times.Exactly(2));
        _presentationMock.Verify(p => p.ReadInput(), Times.Exactly(2));
    }
    
    [Fact]
    public void TryParseInput_ValidInput_ReturnsTrueAndOutput()
    {
        // Arrange
        _lotterySettingsMock.Setup(ls => ls.CurrentValue).Returns(new LotterySettings
        {
            MinNumberOfTicketsPerPlayer = 1,
            MaxNumberOfTicketsPerPlayer = 10
        });
        var parser = new TicketNumberParser(_presentationMock.Object, _lotterySettingsMock.Object);

        // Act
        var result = parser.TryParseInput("3", out var output);

        // Assert
        Assert.True(result);
        Assert.Equal(3, output);
       _presentationMock.Verify(p => p.Present(It.IsAny<string>()), Times.Never());
    }
}
