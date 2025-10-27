using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimplifiedLotteryGame;
using SimplifiedLotteryGame.Abstractions;
using SimplifiedLotteryGame.Infrastructure;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var config = builder.Build();

var serviceCollection = new ServiceCollection();
serviceCollection.Configure<LotterySettings>(config.GetSection("LotterySettings"));
serviceCollection.AddSingleton<IRandomGenerator, RandomGenerator>();
serviceCollection.AddSingleton<IInputParser, TicketNumberParser>();
serviceCollection.AddSingleton<IPresentation, ConsolePresentation>();
serviceCollection.AddSingleton<ILottery, SimplifiedLottery>();
var serviceProvider = serviceCollection.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<ILottery>();
app.Run();

