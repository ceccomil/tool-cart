namespace Example1;

internal sealed class MainSvc(
  ITestDiDisposable _testSvc,
  ICaptainLogger<MainSvc> _logger) : IExecutor
{
  public async Task MainTask(CancellationToken cancellationToken)
  {
    _logger.TraceLog("Main Task!");

    await Task.Delay(240, cancellationToken);

    _testSvc.Test();

    ExtendedConsole.WriteWarning(
      "The main task is completed!",
      isAlert: true);
  }
}