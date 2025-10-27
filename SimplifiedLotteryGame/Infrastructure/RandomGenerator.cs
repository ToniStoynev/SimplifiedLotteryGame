using SimplifiedLotteryGame.Abstractions;

namespace SimplifiedLotteryGame.Infrastructure;

public class RandomGenerator : IRandomGenerator
{
    public int GenerateRandomNumber() => Random.Shared.Next();

    public int GenerateRandomNumberBetween(int min, int max) => Random.Shared.Next(min, max);
}