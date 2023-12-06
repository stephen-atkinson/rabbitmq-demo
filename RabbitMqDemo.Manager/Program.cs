// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMqDemo.Common;
using RabbitMqDemo.Manager;
using RabbitMqDemo.Manager.Models;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, false)
    .AddRabbitMq()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var serviceProvider = new ServiceCollection()
    .Configure<ManagerSettings>(configuration.GetSection("Manager"))
    .Configure<List<ConsumerAccessSettings>>(configuration.GetSection("ConsumerAccess"))
    .Configure<List<GameSettings>>(configuration.GetSection("Games"))
    .AddLogging()
    .AddRabbitMq(configuration)
    .AddSingleton<ManagerService>()
    .BuildServiceProvider();

var managerService = serviceProvider.GetRequiredService<ManagerService>();

await managerService.SetupAsync();