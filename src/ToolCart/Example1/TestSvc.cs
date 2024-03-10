namespace Example1;

public interface ITestSvc : IMainEntrySvc
{
}

internal sealed class TestSvc(
  IHostApplicationLifetime _appLifetime,
  IServiceProvider _serviceProvider)
  : BaseMainEntrySvc(_appLifetime, _serviceProvider),
  ITestSvc
{
  public override async Task MainTask(
    CancellationToken cancellationToken)
  {
    await Task.Delay(1500, cancellationToken);

    ExtendedConsole.WriteQuestion(
      "What is your name?");

    var name = ExtendedConsole.ReadLineFromUser();

    await Task.Delay(1500, cancellationToken);

    ExtendedConsole.WriteInfo(
      $"Hello, {name}!",
      isAlert: true);

    ExtendedConsole.WriteLine();

    ExtendedConsole.WriteQuestion(
      "Press ESC to quit.",
      isAlert: true);

    var key = ExtendedConsole
      .ReadKeyFromUser();

    ExtendedConsole.DeleteLastChar();

    if (key.Key == ConsoleKey.Escape)
    {
      Quit();
    }
  }
}
