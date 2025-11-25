global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using ToolCart.Abstractions;
global using ToolCart.Host;
global using ToolCart.Logging;
global using ToolCart.Services;
using Microsoft.Extensions.Configuration;

namespace ToolExample;

public static class HostBuilder
{
  public static HostRunner Get()
  {
    var hostRunner = new HostRunner()
    {
      ServicesConfig = async services =>
      {
        services
          .Configure<LoggerFilters>(opts =>
          {
            opts.Add(new("ToolExample", LogLevel.Debug));
            opts.Add(new("", LogLevel.Error));
          })
          .AddDefaultLogger();

        await Task.Delay(2_500);
      },
      AppBuilderConfig = app =>
      {
        app
          .Configuration
          .AddEnvironmentVariables("TEST_HOST_VAR_");

        return Task.CompletedTask;
      }
    };


    return hostRunner;
  }
}
