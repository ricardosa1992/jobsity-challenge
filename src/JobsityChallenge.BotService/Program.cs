using JobsityChallenge.BotService;
using JobsityChallenge.BotService.Extensions;
using JobsityChallenge.BotService.Interfaces;
using JobsityChallenge.BotService.Services;
using JobsityChallenge.Shared.Hubs;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHttpClient(nameof(StockQuoteService));
builder.Services.AddScoped<IStockQuoteService, StockQuoteService>();
builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddSingleton<ISignalRService, SignalRClient>(p =>
                        new SignalRClient(builder.Configuration.GetValue<string>("SignalRHub")));

var host = builder.Build();
host.Run();
