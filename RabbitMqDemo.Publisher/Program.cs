using RabbitMqDemo.Common;
using RabbitMqDemo.Publisher;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddRabbitMq();

builder.Services.AddLogging();
builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.Configure<PublisherSettings>(builder.Configuration.GetSection("Publisher"));
builder.Services.AddHostedService<PublisherService>();

var host = builder.Build();
host.Run();