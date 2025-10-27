namespace SimplifiedLotteryGame.Abstractions;

public interface IInputParser
{
    bool TryParseInput(string input, out int output);
}