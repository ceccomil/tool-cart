global using Microsoft.Extensions.Logging;
global using ToolCart.Foundation;
global using ToolCart.Services;
using Microsoft.Extensions.DependencyInjection;
using ToolCart.Host;
using ToolCart.Logging;
using ToolExample4;

var hostRunner = new HostRunner
{
  ServicesConfig = services =>
  {
    services
      .Configure<LoggerFilters>(opts =>
      {
        // Keep framework noise low
        opts.Add(new("System", LogLevel.Error));
        opts.Add(new("Microsoft", LogLevel.Warning));

        opts.Add(new("MyTool", LogLevel.Information));
        opts.Add(new("ToolExample4", LogLevel.Debug));
      })
      .AddDefaultLogger()
      .AddScoped<IExecutor, MyToolExecutor>();

    return Task.CompletedTask;
  }
};

await hostRunner.CreateAndRun<MyToolExecutor>(
  args,
  "MyTool is starting");