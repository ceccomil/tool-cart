namespace ToolCart.Tests.ConsoleHelpers;

[Collection(CONSOLE)]
public class ExtendedConsoleTests
{
  [Fact]
  public async Task ExtendedConsole_when_async_waiting()
  {
    // Arrange
    Console.SetOut(SharedConsoleOut);

    // Act
    _ = ExtendedConsole.StartWait(
      "Waiting for half a second...");

    await Task.Delay(500);

    await ExtendedConsole.StopWait();

    Console.WriteLine(
      "Wait completed!");

    // Assert
    var result = SharedConsoleOut
      .ToString();

    result
      .ShouldContain("\b");

    result
      .ShouldStartWith("Waiting for half a second...");


    result
      .ShouldEndWith($"Wait completed!{Environment.NewLine}");

    ResetConsoleOut();
  }
}
