﻿namespace Example1;

public interface ITestDiDisposable : IDisposable
{
  void Test();
}

internal sealed class TestDiDisposable(
  IAppHandler _appHandler,
  IConsoleWrapper _console,
  ICaptainLogger<TestDiDisposable> _logger) : ITestDiDisposable
{
  private readonly Random _rng = new();

  public void Dispose()
  {
    _console.WriteLine();

    _console.WriteMixed(
      "TestDiDisposable is _au_disposed_d_!");

    GC.SuppressFinalize(this);
  }

  public void Test()
  {
    _logger.TraceLog("Test method is called!");

    _console.WriteInfo(
      $"Random: {_rng.GetHashCode()}");

    var v = _rng.Next(0, 4);

    _console.WriteInfo(
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
