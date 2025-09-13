namespace JobsityChallenge.BotService.Interfaces;

public interface IStockQuoteService
{
    Task<string> GetStockQuoteAsync(string stockCode);
}
