namespace ToolCart.ConsoleHelpers;

internal static partial class ExtendedConsole
{
  private static string GetBlanks(
    int length) => new(' ', length);

  public static bool TryRepositioning(
    string mex,
    bool avoidBlanks = false) => TryRepositioning(
      mex.Length,
      avoidBlanks);

  public static bool TryRepositioning(
    int len,
    bool avoidBlanks = false)
  {
    try
    {
      var left = Console
        .CursorLeft - len;

      if (left < 0)
      {
        left = 0;
      }

      Console.SetCursorPosition(
        left,
        Console.CursorTop);

      if (avoidBlanks)
      {
        return true;
      }

      Console.Write(
        GetBlanks(len));

      Console.SetCursorPosition(
        left,
        Console.CursorTop);

      return true;
    }
    catch (IOException)
    {
      Console.Write(
        Environment.NewLine);

      Console.Write(
        Environment.NewLine);

      return false;
    }
  }

  public static bool TrySetCursorVisibility(
    bool visible)
  {
    try
    {
      Console.CursorVisible = visible;
      return true;
    }
    catch (IOException)
    {
      return false;
    }
  }

  public static void DeleteLastChar()
  {
    var backspace = "\b";
    Console.Write(backspace);
    TryRepositioning(backspace);
  }
}
