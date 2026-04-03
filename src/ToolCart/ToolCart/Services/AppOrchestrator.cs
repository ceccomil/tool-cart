namespace ToolCart.Services;

/// <summary>
/// The application orchestrator is responsible for managing the main execution flow of the application.
/// </summary>
public interface IAppOrchestrator : IHostedService
{
  /// <summary>
  /// Gets the unique identifier for the current execution context.
  /// </summary>
  Guid ExecutionId { get; }
}

internal sealed class AppOrchestrator(
  IAppHandler _appHandler,
  IServiceProvider _serviceProvider)
  : IAppOrchestrator
{
  private Task? _executingTask;

  private readonly ILogger? _logger =
    _serviceProvider
    .GetService<ILogger<AppOrchestrator>>();

  private readonly IExecutionCorrelationScopeFactory _correlationFactory = _serviceProvider
    .GetService<IExecutionCorrelationScopeFactory>() ??
    NullExecutionCorrelationScopeFactory.Instance;

  public Guid ExecutionId { get; private set; }

  private void AppendInfoLog(
    string mex)
  {
    if (_logger is null)
    {
      return;
    }

    _logger.LogInformation(
      "{Mex}",
      mex);
  }

  private void AppendDebugLog(
    string mex)
  {
    if (_logger is null)
    {
      return;
    }

    _logger.LogDebug(
      "{Mex}",
      mex);
  }

  private void AppendErrorLog(
    string mex,
    Exception? ex)
  {
    if (_logger is null)
    {
      return;
    }

    _logger.LogError(ex,
      "{Mex}",
      mex);
  }

  private async Task ExecMainTask(
    IServiceScope scope)
  {
    var executor = scope
      .ServiceProvider
      .GetService<IExecutor>();

    if (executor is null)
    {
      AppendErrorLog(
        "The executor service is not found!",
         null);

      await _appHandler.Exit(ErrorCode.MissingRequirements);

      return;
    }

    await executor.MainTask(
      _appHandler.StoppingToken);

    var flusher = scope
      .ServiceProvider
      .GetService<IFileLogsFlusher>();

    if (flusher is not null)
    {
      await flusher.FlushAsync(
        _appHandler.StoppingToken);
    }
  }

  private async Task Run()
  {
    var consecutiveFailures = 0;

    while (!_appHandler
      .StoppingToken
      .IsCancellationRequested &&
      consecutiveFailures <= 5)
    {

#if NET9_0_OR_GREATER
      ExecutionId = Guid.CreateVersion7();
#else
      ExecutionId = Guid.NewGuid();
#endif

      using var loggerScope = _correlationFactory
        .BeginScope(ExecutionId);

      var executionScope = _serviceProvider
        .CreateScope();

      AppendDebugLog(
        $"Starting a new execution ({ExecutionId}) " +
        $"a new scope has been created ({executionScope
          .GetHashCode()}).");

      try
      {
        // Small delay to avoid the CPU overloading.
        await Task.Delay(100, _appHandler.StoppingToken);
        await ExecMainTask(executionScope);

        AppendDebugLog(
        $"Execution: {ExecutionId} is completed.");

        consecutiveFailures = 0;
      }
      catch (OperationCanceledException) when (_appHandler.StoppingToken.IsCancellationRequested)
      {
        // Graceful shutdown: don't count as a failure, just break the loop.
        AppendDebugLog($"Execution: {ExecutionId} cancelled.");
        break;
      }
      catch (Exception ex)
      {
        AppendErrorLog(
          $"Execution: {ExecutionId} is " +
          $"completed with errors!",
          ex);

        consecutiveFailures++;
      }

      if (consecutiveFailures > 5)
      {
        AppendErrorLog(
          "The application is being stopped " +
          "due to consecutive failures!",
          null);

        await _appHandler.Exit(ErrorCode.ConsecutiveFailures);
      }

      executionScope.Dispose();
    }
  }

  public Task StartAsync(
    CancellationToken cancellationToken)
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromCanceled(
        cancellationToken);
    }

    AppendInfoLog(
      "The Application is running.");

    _executingTask = Run();

    return Task.CompletedTask;
  }

  public async Task StopAsync(
    CancellationToken cancellationToken)
  {
    if (cancellationToken.IsCancellationRequested)
    {
      await Task.FromCanceled(cancellationToken);
      return;
    }

    if (!_appHandler
      .StoppingToken
      .IsCancellationRequested)
    {
      await _appHandler.Exit();
    }

    if (_executingTask is null)
    {
      AppendInfoLog("The application has been shut down.");
      return;
    }

    await Task.WhenAny(
      _executingTask,
      Task.Delay(Timeout.Infinite, cancellationToken));

    AppendInfoLog("The application has been shut down.");
  }
}


