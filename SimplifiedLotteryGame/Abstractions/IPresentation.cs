namespace SimplifiedLotteryGame.Abstractions;

public interface IPresentation
{
    void Present(string text);
    
    string ReadInput();
}