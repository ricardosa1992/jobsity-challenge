using JobsityChallenge.BotService;
using JobsityChallenge.BotService.Extensions;
using JobsityChallenge.BotService.Interfaces;
using JobsityChallenge.BotService.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHttpClient(nameof(StockQuoteService));
builder.Services.AddScoped<IStockQuoteService, StockQuoteService>();
builder.Services.AddMessageBroker(builder.Configuration);

var host = builder.Build();
host.Run();
