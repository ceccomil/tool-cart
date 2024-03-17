using CaptainLogger.Options;
using Microsoft.Extensions.Configuration;

namespace ToolCart.Logging.Tests;

[Collection("LoggerRegistration Tests")]
public class LoggerRegistrationTests
{
  [Fact]
  public void AddDefaultLogger_when_no_filters()
  {
    // Arrange
    var services = new ServiceCollection();

    // Act
    services.AddDefaultLogger();

    var sp = services
      .BuildServiceProvider();

    var options = sp.GetRequiredService<
      IOptions<LoggerFilterOptions>>();

    // Assert
    options
      .Value
      .Rules
      .Should()
      .BeEmpty();
  }

  [Fact]
  public void AddDefaultLogger_when_filtering()
  {
    // Arrange
    var services = new ServiceCollection();

    // Act
    services
      .Configure<LoggerFilters>(opts =>
      {
        opts.Add(new(
          "ACategory",
          LogLevel.Critical));
      })
      .AddDefaultLogger();

    var sp = services
      .BuildServiceProvider();

    var filter = sp.GetRequiredService<
      IOptions<LoggerFilterOptions>>()
      .Value
      .Rules
      .Single();

    // Assert
    filter
      .CategoryName
      .Should()
      .Be("ACategory");

    filter
      .LogLevel
      .Should()
      .Be(LogLevel.Critical);
  }

  [Theory]
  [InlineData("true", Recipients.File | Recipients.Console)]
  [InlineData("false", Recipients.File)]
  public void AddDefaultLogger_when_recipients_are_expected(
    string consoleLogging,
    Recipients expected)
  {
    // Arrange
    var config = new ConfigurationBuilder()
      .AddInMemoryCollection(
        new Dictionary<string, string?>()
        {
          { "AllowConsoleLogging", consoleLogging }
        })
      .Build();

    var services = new ServiceCollection()
      .AddSingleton<IConfiguration>(config);

    // Act
    services.AddDefaultLogger();

    var sp = services
      .BuildServiceProvider();

    var options = sp.GetRequiredService<
      IOptions<CaptainLoggerOptions>>()
      .Value;

    // Assert
    options
      .LogRecipients
      .Should()
      .Be(expected);
  }
}
