using ToolCart.Foundation;

namespace ToolCart.Tests.Services;

[Collection("AppHandler Tests")]
public class AppHandlerTests
{
  private readonly IHostApplicationLifetime _appLifetime = Substitute
    .For<IHostApplicationLifetime>();

  private readonly ILogger<AppHandler> _logger = Substitute
    .For<ILogger<AppHandler>>();

  [Fact]
  public void Exit_when_exit_code_is_zero()
  {
    // Arrange
    var sut = new AppHandler(
      _appLifetime,
      _logger);

    Environment.ExitCode = 0;

    // Act
    sut.Exit();

    // Assert
    _logger
      .Received(1)
      .LogInformation(
        "Exiting application");

    Environment.ExitCode = 0;
  }

  [Theory]
  [InlineData(ErrorCode.ConsecutiveFailures, "[101] ConsecutiveFailures")]
  [InlineData(ErrorCode.MissingRequirements, "[201] MissingRequirements")]
  [InlineData(ErrorCode.CriticalError, "[301] CriticalError")]
  [InlineData((ErrorCode)1_000, "1000")]
  public void Exit_when_exit_when_error(
    ErrorCode error,
    string expected)
  {
    // Arrange
    var sut = new AppHandler(
      _appLifetime,
      _logger);

    Environment.ExitCode = 0;

    var mex =
      "General failure. Exiting application" +
      $" with code {expected}.";

    // Act
    sut.Exit(error);

    // Assert
    _logger
      .Received(1)
      .Log(
        LogLevel.Error,
        0,
        Arg.Is<object>(x => x.ToString() == mex),
        null,
        Arg.Any<Func<object, Exception?, string>>());

    Environment.ExitCode = 0;
  }
}
