namespace ToolCart.Tests.Services;

[Collection("AppOrchestrator Tests")]
public class AppOrchestratorTests
{
  private readonly IAppHandler _appHandler = Substitute
    .For<IAppHandler>();

  private readonly ILogger<AppOrchestrator> _logger = Substitute
    .For<ILogger<AppOrchestrator>>();

  [Fact]
  public async Task StartAsync_when_started()
  {
    // Arrange
    var cts = new CancellationTokenSource();
    cts.Cancel();

    _appHandler
      .StoppingToken
      .Returns(cts.Token);

    var sp = new ServiceCollection()
      .AddSingleton(_appHandler)
      .AddSingleton(_logger)
      .BuildServiceProvider();

    var sut = new AppOrchestrator(
      _appHandler,
      sp);

    // Act
    await sut.StartAsync(CancellationToken
      .None);

    // Assert
    _logger
    .Received(1)
    .Log(
      LogLevel.Information,
      0,
      Arg.Any<object?>(),
      Arg.Any<Exception>(),
      Arg.Any<Func<object?, Exception?, string>>());
  }

  [Fact]
  public void StartAsync_when_start_is_canceled()
  {
    // Arrange
    var sut = new AppOrchestrator(
      _appHandler,
      Substitute.For<IServiceProvider>());

    var cts = new CancellationTokenSource();
    cts.Cancel();

    // Act
    var task = sut.StartAsync(cts.Token);

    // Assert
    task
      .Status
      .ShouldBe(TaskStatus.Canceled);
  }

  [Fact]
  public async Task StopAsync_when_stops()
  {
    // Arrange
    var sp = new ServiceCollection()
      .AddSingleton(_appHandler)
      .AddSingleton(_logger)
      .BuildServiceProvider();

    var sut = new AppOrchestrator(
      _appHandler,
      sp);

    // Act
    await sut.StopAsync(CancellationToken
      .None);

    // Assert
    _logger
    .Received(1)
    .Log(
      LogLevel.Information,
      0,
      Arg.Any<object?>(),
      Arg.Any<Exception>(),
      Arg.Any<Func<object?, Exception?, string>>());

    _appHandler
      .Received(1)
      .Exit();
  }

  [Fact]
  public void StopAsync_when_stop_is_canceled()
  {
    // Arrange
    var sut = new AppOrchestrator(
      _appHandler,
      Substitute.For<IServiceProvider>());

    var cts = new CancellationTokenSource();
    cts.Cancel();

    // Act
    var task = sut.StopAsync(cts.Token);

    // Assert
    task
      .Status
      .ShouldBe(TaskStatus.Canceled);
  }

  [Theory]
  [InlineData(true, true)]
  [InlineData(false, false)]
  public async Task Run_when_executions_succeeded_or_failed(
    bool throwEx,
    bool expected)
  {
    // Arrange
    var cts = new CancellationTokenSource();

    var sp = new ServiceCollection()
      .AddSingleton(cts)
      .AddSingleton(_appHandler)
      .AddSingleton(_logger)
      .AddSingleton<IExecutor, TestExecutor>()
      .AddSingleton(new Thrower(throwEx))
      .BuildServiceProvider();

    _appHandler
      .StoppingToken
      .Returns(cts.Token);

    var sut = new AppOrchestrator(
      _appHandler,
      sp);

    var mi = sut
      .GetType()
      .GetMethod("Run",
        BindingFlags.NonPublic |
        BindingFlags.Instance);

    var task = mi!
      .Invoke(sut, null) as Task;

    // Act
    await task!;

    // Assert
    var errors = _logger
      .ReceivedCalls()
      .SelectMany(x => x.GetArguments())
      .Where(x => x is LogLevel.Error)
      .ToList();

    (errors.Count > 0)
      .ShouldBe(expected);
  }
}

file class TestExecutor(
  CancellationTokenSource _cts,
  Thrower _thrower) : IExecutor
{
  private int _executions = 0;

  public async Task MainTask(
    CancellationToken cancellationToken)
  {
    await Task.Delay(5, cancellationToken);

    _executions++;

    if (_executions == 3 &&
      !_thrower.ShouldThrow)
    {
      _cts.Cancel();
    }

    if (_thrower.ShouldThrow)
    {
      throw new InvalidOperationException(
        "To test failures!");
    }
  }

  public int GetExecutions() => _executions;
}

file record Thrower(bool ShouldThrow);

