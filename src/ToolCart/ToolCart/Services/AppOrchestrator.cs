namespace ToolCart.Services;

internal interface IAppOrchestrator : IHostedService
{
  Guid ExecutionId { get; }
}

internal sealed class AppOrchestrator(
  IAppHandler _appHandler,
  IServiceProvider _serviceProvider)
  : IAppOrchestrator
{
  private readonly ILogger? _logger =
    _serviceProvider
    .GetService<ILogger<AppOrchestrator>>();

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

      _appHandler.Exit(
        ErrorCode.MissingRequirements);

      return;
    }

    await executor.MainTask(
      _appHandler.StoppingToken);
  }

  private async Task Run()
  {
    var consecutiveFailures = 0;

    while (!_appHandler
      .StoppingToken
      .IsCancellationRequested &&
      consecutiveFailures <= 5)
    {
      ExecutionId = Guid.NewGuid();

      var executionScope = _serviceProvider
        .CreateScope();

      AppendDebugLog(
        $"Starting a new execution ({ExecutionId}) " +
        $"a new scope has been created ({executionScope
          .GetHashCode()}).");

      try
      {
        // Small delay to avoid the CPU overloading.
        await Task.Delay(100);
        await ExecMainTask(executionScope);

        AppendDebugLog(
        $"Execution: {ExecutionId} is completed.");

        consecutiveFailures = 0;
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

        _appHandler.Exit(
          ErrorCode.ConsecutiveFailures);
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

    _ = Run();

    return Task.CompletedTask;
  }

  public Task StopAsync(
    CancellationToken cancellationToken)
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromCanceled(
        cancellationToken);
    }

    if (!_appHandler
      .StoppingToken
      .IsCancellationRequested)
    {
      _appHandler.Exit();
    }

    AppendInfoLog(
      "The application has been shut down.");

    return Task.CompletedTask;
  }
}


