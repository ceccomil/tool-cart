namespace Example1;

public interface ITestDiDisposable : IDisposable
{
  void Test();
}

internal sealed class TestDiDisposable(
  IAppHandler _appHandler,
  ICaptainLogger<TestDiDisposable> _logger) : ITestDiDisposable
{
  private readonly Random _rng = new();

  public void Dispose()
  {
    ExtendedConsole.WriteInfo(
      "TestDiDisposable is disposed!",
      isAlert: true);
  }

  public void Test()
  {
    _logger.TraceLog("Test method is called!");

    ExtendedConsole.WriteInfo(
      $"Random: {_rng.GetHashCode()}");

    var v = _rng.Next(0, 4);

    ExtendedConsole.WriteInfo(
      $"Random value: {v}",
      isAlert: true);

    if (v == 0)
    {
      _appHandler.Exit();
    }

    if (v == 1)
    {
      _appHandler.Exit(
        ErrorCode.CriticalError);
    }
  }
}
