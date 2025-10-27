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
}