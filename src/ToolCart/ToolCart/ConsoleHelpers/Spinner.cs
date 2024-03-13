namespace ToolCart.ConsoleHelpers;

internal static class Spinner
{
  private static short _counter;

  public static void Reset() => _counter = 0;

  public static Task Next(
    CancellationToken? cancellationToken = default)
  {
    cancellationToken ??= CancellationToken
      .None;

    if (cancellationToken.Value.IsCancellationRequested)
    {
      return Task.FromCanceled(
        cancellationToken.Value);
    }

    _counter++;

    switch (_counter % 4)
    {
      case 0:
        Console.Write("/");
        break;
      case 1:
        Console.Write("-");
        break;
      case 2:
        Console.Write("\\");
        break;
      case 3:
        Console.Write("|");
        break;
      default:
        throw new InvalidOperationException(
          "Invalid counter value.");
    }

    if (_counter == 4)
    {
      _counter = 0;
    }

    Console.Write('\b');

    return Task
      .CompletedTask;
  }
}
