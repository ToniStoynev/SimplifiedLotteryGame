using Microsoft.Extensions.Options;
using SimplifiedLotteryGame.Abstractions;

namespace SimplifiedLotteryGame.Infrastructure;

public class TicketNumberParser(IPresentation presentation, IOptionsMonitor<LotterySettings> options) : IInputParser
{
    public bool TryParseInput(string input, out int output)
    {
        while (true)
        {
            if (!int.TryParse(input, out var ticketsCount))
            {
                presentation.Present(MessageTemplates.InvalidInputMessage);
                input = presentation.ReadInput();
                continue;
            }

            if (ticketsCount < options.CurrentValue.MinNumberOfTicketsPerPlayer || ticketsCount > options.CurrentValue.MaxNumberOfTicketsPerPlayer)
            {
                presentation.Present(MessageTemplates.InvalidTicketNumberMessage(options.CurrentValue.MinNumberOfTicketsPerPlayer, options.CurrentValue.MaxNumberOfTicketsPerPlayer));
                continue;
            }
            
            output = ticketsCount;
            
            return true;
        }
    }
}