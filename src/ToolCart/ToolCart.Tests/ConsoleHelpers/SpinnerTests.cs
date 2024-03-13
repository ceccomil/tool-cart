namespace ToolCart.Tests.ConsoleHelpers;

[Collection("Console Tests")]
public class SpinnerTests
{
  [Fact]
  public async Task Next_when_full_circle()
  {
    // Arrange
    using var output = new StringWriter();
    Console.SetOut(output);

    var tasks = new List<Task>();

    // Act
    for (var i = 0; i < 5; i++)
    {
      tasks.Add(Spinner.Next());
    }

    await Task.WhenAll(tasks);

    Spinner.Reset();

    // Assert
    output
      .ToString()
      .Should()
      .Be("-\b\\\b|\b/\b-\b");
  }

  [Fact]
  public async Task Next_when_cancellation_requested()
  {
    // Arrange
    using var output = new StringWriter();
    Console.SetOut(output);

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

    output
      .ToString()
      .Should()
      .Be("-\b");
  }
}
