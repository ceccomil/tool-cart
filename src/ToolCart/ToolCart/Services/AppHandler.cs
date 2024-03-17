namespace ToolCart.Services;

/// <summary>
/// The application handler.
/// </summary>
public interface IAppHandler
{
  /// <summary>
  /// The application stopping token.
  /// </summary>
  CancellationToken StoppingToken { get; }

  /// <summary>
  /// Exits the application
  /// with an optional exit code.
  /// </summary>
  /// <param name="exitCode"></param>
  void Exit(int exitCode = 0);

  /// <summary>
  /// Exits the application
  /// with a known error code.
  /// </summary>
  /// <param name="errorCode"></param>
  void Exit(ErrorCode errorCode);
}

internal sealed class AppHandler(
  IHostApplicationLifetime _appLifetime,
  ILogger<AppHandler> _logger)
  : IAppHandler
{
  public CancellationToken StoppingToken =>
    _appLifetime.ApplicationStopping;

  private void LogExit(int exitCode)
  {
    if (exitCode == 0)
    {
      _logger.LogInformation(
        "Exiting application");

      return;
    }

    var strCode = $"{exitCode}";

    if (Enum.IsDefined(typeof(ErrorCode), exitCode))
    {
      strCode = $"[{exitCode}] {(ErrorCode)exitCode}";
    }

    _logger.LogError(
      "General failure. Exiting application" +
      " with code {ExitCode}.",
      strCode);
  }

  public void Exit(int exitCode = 0)
  {
    LogExit(exitCode);
    Environment.ExitCode = exitCode;
    _appLifetime.StopApplication();
  }

  public void Exit(ErrorCode errorCode) => Exit(
    (int)errorCode);
}
