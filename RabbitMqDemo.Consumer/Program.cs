using RabbitMqDemo.Common;
using RabbitMqDemo.Consumer;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddRabbitMq();

builder.Services.AddLogging();
builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.Configure<ConsumerSettings>(builder.Configuration.GetSection("Consumer"));
builder.Services.AddHostedService<ConsumerService>();

var host = builder.Build();
host.Run();