using JobsityChallenge.BotService.Interfaces;
using JobsityChallenge.Shared.Hubs;
using JobsityChallenge.Shared.MessageBroker.Events;
using MassTransit;

namespace JobsityChallenge.BotService.Consumers;

public class StockQuoteBotConsumer(
    IStockQuoteService stockQuoteService,
    ISignalRService signalRService,
    ILogger<StockQuoteBotConsumer> logger) : IConsumer<StockQuoteBotEvent>
{
    public async Task Consume(ConsumeContext<StockQuoteBotEvent> context)
    {
        var message = context.Message;

        if (!message.Content.Contains("stock="))
        {
            logger.LogWarning("Invalid StockQuote message format");
            return;
        }

        var stockCode = message.Content.Split('=')[1];

        try
        {
            var stockQuote = await stockQuoteService.GetStockQuoteAsync(stockCode);

            Console.WriteLine($"stockQuote: {stockQuote}");

            await signalRService.SendMessage(message.RoomId.ToString(), "Bot", stockQuote);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(StockQuoteBotConsumer));

            await signalRService.SendMessage(message.RoomId.ToString(), "Bot", $"Could not retrieve data for stock code {stockCode}.");
        }
    }
}
