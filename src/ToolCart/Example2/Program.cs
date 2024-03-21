using CaptainLogger;
using ToolCart.Host;
using ToolCart.Logging;
using ToolCart.Services;

var host = new HostRunner()
{
  ServicesConfig = services =>
  {
    services.AddDefaultLogger();
    return Task.Delay(10_000);
  }
};

await host.CreateAndRun<MainSvc>(
  args,
  "Start");


file sealed class MainSvc(
  ICaptainLogger<MainSvc> _logger) : IExecutor
{
  public async Task MainTask(CancellationToken cancellationToken)
  {
    _logger.InformationLog("MainSvc is running");

    _logger.WarningLog("Warning");

    _logger.ErrorLog("Error");

    _logger.DebugLog("CTRL + C to quit");

    await Task.Delay(3_000, cancellationToken);
  }
}
