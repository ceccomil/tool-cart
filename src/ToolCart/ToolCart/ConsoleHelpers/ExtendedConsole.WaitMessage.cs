namespace ToolCart.ConsoleHelpers;

internal static partial class ExtendedConsole
{
  private static CancellationTokenSource _waitCts = new();
  private static string _waitMessage = string.Empty;
  private static bool _isWaiting = false;
  private static Theme _waitTheme = null!;

  /// <summary>
  /// Starts a wait with a message.
  /// </summary>
  public static async Task StartWait(
    string mex,
    Theme? theme = default)
  {
    if (_isWaiting)
    {
      return;
    }

    _isWaiting = true;

    theme ??= new(
      ConsoleColor.Black,
      ConsoleColor.Green);

    _waitTheme = theme;

    Console.ForegroundColor = theme.Foreground;
    Console.BackgroundColor = theme.Background;

    _waitMessage = $"{mex}";

    Console.Write(_waitMessage);

    while (!_waitCts
      .Token
      .IsCancellationRequested)
    {
      await Spinner.Next(
        _waitCts.Token);

      await Task.Delay(
        100,
        _waitCts.Token);
    }
  }

  /// <summary>
  /// Stops the wait.
  /// </summary>
  public static Task StopWait()
  {
    if (!_isWaiting)
    {
      return Task.CompletedTask;
    }

    Console.ForegroundColor = _waitTheme.BeforeForeground;
    Console.BackgroundColor = _waitTheme.BeforeBackground;

    _waitCts.Cancel(true);

    Console.Write(" \b");

    TryRepositioning(_waitMessage);

    Spinner.Reset();

    _waitCts = new();
    _isWaiting = false;

    return Task.CompletedTask;
  }
}
