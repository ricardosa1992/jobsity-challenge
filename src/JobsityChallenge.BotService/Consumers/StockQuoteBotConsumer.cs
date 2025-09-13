using JobsityChallenge.BotService.Interfaces;
using JobsityChallenge.Shared.MessageBroker.Events;
using MassTransit;

namespace JobsityChallenge.BotService.Consumers;

public class StockQuoteBotConsumer(IStockQuoteService stockQuoteService) : IConsumer<StockQuoteBotEvent>
{
    public async Task Consume(ConsumeContext<StockQuoteBotEvent> context)
    {
        var stockCode = context.Message.Content.Split('=')[1];

        if (!string.IsNullOrWhiteSpace(stockCode))
        {
            var stockQuote = await stockQuoteService.GetStockQuoteAsync(stockCode);

            Console.WriteLine($"stockQuote: {stockQuote}");
        }
    }
}
