namespace ToolCart.Tests.Host;

[Collection(CONSOLE)]
public class HostRunnerTests
{
  public static int FailOver { get; set; }
  public static int CustomExitCode { get; set; }

  [Theory]
  [InlineData("--testSwitch", 0)]
  [InlineData("--unexpected", 1)]
  public async Task CreateAndRun_when_no_additional_configurations(
    string switchArg,
    int exitCode)
  {
    // Arrange
    var runner = new HostRunner();
    var args = new string[]
    {
      switchArg,
      "--timesUp",
      $"{DateTime.UtcNow.AddSeconds(2)}"
    };

    FailOver = 0;
    CustomExitCode = 0;

    // Act
    await runner.CreateAndRun<HostRunnerTestService>(
      args,
      "Bootstrapping");

    // Assert
    CustomExitCode
      .ShouldBe(exitCode);
  }
}

file class HostRunnerTestService(
  IConfiguration _config,
  IHostApplicationLifetime _lifeTime) : IExecutor
{
  private void Stop(
    bool withErrors = true)
  {
    if (withErrors)
    {
      HostRunnerTests.CustomExitCode = 1;
    }

    _lifeTime.StopApplication();
  }

  public async Task MainTask(CancellationToken cancellationToken)
  {
    HostRunnerTests.FailOver++;

    if (HostRunnerTests.FailOver > 50)
    {
      Stop();
    }

    if (!_config.SwitchIsOn("TestSwitch"))
    {
      Stop();
    }

    var _timesUp = DateTime.Parse(
      _config["TimesUp"]!);

    if (DateTime.UtcNow > _timesUp)
    {
      Stop(withErrors: false);
    }

    await Task.Delay(
      10,
      cancellationToken);
  }
}
