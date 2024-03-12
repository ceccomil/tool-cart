using System.Reflection;

namespace ToolCart.Tests.Services;

[Collection("AppOrchestratorTests")]
public class AppOrchestratorTests
{
  private readonly IHostApplicationLifetime _appLifetime = Substitute
    .For<IHostApplicationLifetime>();

  private readonly ILogger<AppOrchestrator> _logger = Substitute
    .For<ILogger<AppOrchestrator>>();

  [Fact]
  public async Task StartAsync_when_started()
  {
    // Arrange
    var cts = new CancellationTokenSource();
    cts.Cancel();

    _appLifetime
      .ApplicationStopping
      .Returns(cts.Token);

    var sp = new ServiceCollection()
      .AddSingleton(_appLifetime)
      .AddSingleton(_logger)
      .BuildServiceProvider();

    var sut = new AppOrchestrator(
      _appLifetime,
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
      _appLifetime,
      Substitute.For<IServiceProvider>());

    var cts = new CancellationTokenSource();
    cts.Cancel();

    // Act
    var task = sut.StartAsync(cts.Token);

    // Assert
    task
      .Status
      .Should()
      .Be(TaskStatus.Canceled);
  }

  [Fact]
  public async Task StopAsync_when_stops()
  {
    // Arrange
    var sp = new ServiceCollection()
      .AddSingleton(_appLifetime)
      .AddSingleton(_logger)
      .BuildServiceProvider();

    var sut = new AppOrchestrator(
      _appLifetime,
      sp);

    var cts = new CancellationTokenSource();
    cts.Cancel();

    _appLifetime
    .ApplicationStopping
    .Returns(cts.Token);

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

    _appLifetime
      .Received(1)
      .StopApplication();
  }

  [Fact]
  public void StopAsync_when_stop_is_canceled()
  {
    // Arrange
    var sut = new AppOrchestrator(
      _appLifetime,
      Substitute.For<IServiceProvider>());

    var cts = new CancellationTokenSource();
    cts.Cancel();

    // Act
    var task = sut.StopAsync(cts.Token);

    // Assert
    task
      .Status
      .Should()
      .Be(TaskStatus.Canceled);
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
      .AddSingleton(_appLifetime)
      .AddSingleton(_logger)
      .AddSingleton<IExecutor, TestExecutor>()
      .AddSingleton(new Thrower(throwEx))
      .BuildServiceProvider();

    _appLifetime
      .ApplicationStopping
      .Returns(cts.Token);

    var sut = new AppOrchestrator(
      _appLifetime,
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
      .Should()
      .Be(expected);
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

