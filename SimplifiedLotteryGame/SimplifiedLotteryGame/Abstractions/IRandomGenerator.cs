namespace SimplifiedLotteryGame.Abstractions;

public interface IRandomGenerator
{
    int GenerateRandomNumber();
    
    int GenerateRandomNumberBetween(int min, int max);
}