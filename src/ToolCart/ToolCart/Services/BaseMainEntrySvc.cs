namespace ToolCart.Services;

/// <summary>
/// Base entry service.
/// </summary>
public abstract class BaseMainEntrySvc(
  IHostApplicationLifetime _appLifetime,
  IServiceProvider _serviceProvider)
  : IMainEntrySvc
{
  private readonly ILogger<BaseMainEntrySvc>? _logger =
    _serviceProvider
    .GetService<ILogger<BaseMainEntrySvc>>();

  private readonly CancellationToken _stoppingToken =
    _appLifetime.ApplicationStopping;

  /// <summary>
  /// Execution identifier.
  /// </summary>
  public Guid ExecutionId { get; private set; }

  private void AppendInfoLog(
    string mex)
  {
    if (_logger is null)
    {
      return;
    }

    _logger.LogInformation(mex);
  }

  private void AppendDebugLog(
    string mex)
  {
    if (_logger is null)
    {
      return;
    }

    _logger.LogDebug(mex);
  }

  private void AppendErrorLog(
    string mex,
    Exception? ex)
  {
    if (_logger is null)
    {
      return;
    }

    _logger.LogError(ex, mex);
  }

  private async Task Run()
  {
    while (!_stoppingToken.IsCancellationRequested)
    {
      ExecutionId = Guid.NewGuid();

      AppendDebugLog(
        $"Starting a new execution ({ExecutionId}).");

      try
      {
        await MainTask(_stoppingToken);
      }
      catch (Exception ex)
      {
        AppendErrorLog(
          $"Execution: {ExecutionId} is " +
          $"completed with errors!",
          ex);

        continue;
      }

      AppendDebugLog(
        $"Execution: {ExecutionId} is completed.");
    }
  }

  /// <summary>
  /// Quits the application.
  /// </summary>
  protected void Quit()
  {
    _appLifetime.StopApplication();
  }

  /// <summary>
  /// Tool kick off.
  /// </summary>
  public abstract Task MainTask(
    CancellationToken cancellationToken);

  /// <summary>
  /// Hosted service start.
  /// </summary>
  public Task StartAsync(
    CancellationToken cancellationToken)
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromCanceled(
        cancellationToken);
    }

    _ = Run();

    AppendInfoLog(
      "The Application is running.");

    return Task.CompletedTask;
  }

  /// <summary>
  /// Hosted service stop.
  /// </summary>
  public async Task StopAsync(
    CancellationToken cancellationToken)
  {
    if (cancellationToken.IsCancellationRequested)
    {
      await Task.FromCanceled(
        cancellationToken);

      return;
    }

    if (!_stoppingToken.IsCancellationRequested)
    {
      _appLifetime.StopApplication();
    }

    AppendInfoLog(
      "The application has been shut down.");
  }
}
