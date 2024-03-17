global using CaptainLogger;
global using Example1;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using ToolCart.ConsoleHelpers;
global using ToolCart.Foundation;
global using ToolCart.Host;
global using ToolCart.Logging;
global using ToolCart.Services;


var hostRunner = new HostRunner()
{
  ServicesConfig = services =>
  {
    services
      .Configure<LoggerFilters>(opts =>
      {
        opts.Add(new("System", LogLevel.Error));
        opts.Add(new("Microsoft", LogLevel.Warning));
        opts.Add(new("Example1", LogLevel.Debug));
        opts.Add(new("ToolCart", LogLevel.Debug));
      })
      .AddDefaultLogger()
      .AddScoped<ITestDiDisposable, TestDiDisposable>();

    return Task.CompletedTask;
  }
};

await hostRunner.CreateAndRun<MainSvc>(
  args,
  "Application is starting");