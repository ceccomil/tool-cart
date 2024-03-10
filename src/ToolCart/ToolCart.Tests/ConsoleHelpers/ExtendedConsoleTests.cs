namespace ToolCart.Tests.ConsoleHelpers;

[Collection("Console Tests")]
public class ExtendedConsoleTests
{
  [Fact]
  public async Task ExtendedConsole_when_async_waiting()
  {
    // Arrange
    using var output = new StringWriter();
    Console.SetOut(output);

    // Act
    _ = ExtendedConsole.StartWait(
      "Waiting for half a second...");

    await Task.Delay(500);

    await ExtendedConsole.StopWait();

    Console.WriteLine(
      "Wait completed!");

    // Assert
    var result = output
      .ToString();

    result
      .Should()
      .Contain("\b")
      .And
      .StartWith("Waiting for half a second...")
      .And
      .EndWith($"Wait completed!{Environment.NewLine}");
  }
}
