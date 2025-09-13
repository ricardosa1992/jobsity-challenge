using JobsityChallenge.BotService.Consumers;
using MassTransit;

namespace JobsityChallenge.BotService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<StockQuoteBotConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("RabbitMq"));

                cfg.ReceiveEndpoint("stock-quote-requests", e =>
                {
                    e.ConfigureConsumer<StockQuoteBotConsumer>(context);
                });
            });
        });

        return services;
    }
}
