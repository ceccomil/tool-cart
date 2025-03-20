namespace Example1;

internal sealed class MainSvc(
  ITestDiDisposable _testSvc,
  ICaptainLogger<MainSvc> _logger,
  IConsoleWrapper _console) : IExecutor
{
  public async Task MainTask(CancellationToken cancellationToken)
  {
    _logger.TraceLog("Main Task!");

    await Task.Delay(240, cancellationToken);

    _testSvc.Test();

    var end = $"_d_Date: _au_{DateTime.UtcNow:dd/MM/yyyy}" +
      $"_d_ Time: _au_{DateTime.UtcNow:HH:mm:ss}";

    _console.WriteMixed(
      "_i_The main task is completed!" +
      Environment.NewLine +
      end);

    _console.WriteQuestion("Please check read password by typing something!");

    var pwd = _console.ReadPasswordFromUser();

    _console.WriteMixed(
      $"_i_You typed: _au_{pwd}");
  }
}