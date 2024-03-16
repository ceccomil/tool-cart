namespace ToolCart.Tests.ConsoleHelpers;

[Collection(CONSOLE)]
public class SpinnerTests
{
  [Fact]
  public async Task Next_when_full_circle()
  {
    // Arrange
    Console.SetOut(SharedConsoleOut);
    var tasks = new List<Task>();

    // Act
    for (var i = 0; i < 5; i++)
    {
      tasks.Add(Spinner.Next());
    }

    await Task.WhenAll(tasks);

    Spinner.Reset();

    // Assert
    SharedConsoleOut
      .ToString()
      .Should()
      .Be("-\b\\\b|\b/\b-\b");

    ResetConsoleOut();
  }

  [Fact]
  public async Task Next_when_cancellation_requested()
  {
    // Arrange
    Console.SetOut(SharedConsoleOut);

    var cts = new CancellationTokenSource();

    // Act
    await Spinner.Next(cts.Token);
    cts.Cancel();
    var act = () => Spinner.Next(cts.Token);

    // Assert
    await act
      .Should()
      .ThrowAsync<TaskCanceledException>();

    Spinner.Reset();

    SharedConsoleOut
      .ToString()
      .Should()
      .Be("-\b");

    ResetConsoleOut();
  }
}
