namespace ToolExample4;

internal sealed class MyToolExecutor(
  ILogger<MyToolExecutor> logger,
  IConsoleWrapper console,
  IAppHandler appHandler)
  : IExecutor
{
  private readonly ILogger<MyToolExecutor> _logger = logger;
  private readonly IConsoleWrapper _console = console;
  private readonly IAppHandler _appHandler = appHandler;

  public async Task MainTask(
    CancellationToken cancellationToken)
  {
    _logger.LogInformation(
      "MyTool main task started.");

    _console.WriteMixed(
      "_i_Welcome to _au_MyTool_i_ powered by ToolCart!");

    for (var i = 0; i < 3 && !cancellationToken.IsCancellationRequested; i++)
    {
      _console.WriteQuestion(
        $"Tick {i + 1}");

      _logger.LogInformation(
        "Tick {Tick}",
        i + 1);

      await Task.Delay(
        500,
        cancellationToken);
    }

    _console.WriteMixed(
      "Done. Press _au_ENTER_d_ to repeat or _au_ESCAPE_d_ to exit.");

    var key = _console.ReadKeyFromUser();

    _console.WriteLine();

    if (key.Key == ConsoleKey.Escape)
    {
      _logger.LogInformation(
        "User chose to exit the tool.");

      _appHandler.Exit();
      return;
    }
    
    if (key.Key == ConsoleKey.Enter)
    {
      _logger.LogInformation(
        "User chose to repeat the main task.");

      return;
    }

    _logger.LogError(
      "User input is not valid!");

   _appHandler.Exit(1);
  }
}
