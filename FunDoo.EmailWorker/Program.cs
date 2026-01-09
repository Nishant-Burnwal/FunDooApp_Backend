using FunDoo.EmailWorker;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Register Worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
