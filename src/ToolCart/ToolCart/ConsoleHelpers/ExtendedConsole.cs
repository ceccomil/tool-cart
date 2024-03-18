namespace ToolCart.ConsoleHelpers;

/// <summary>
/// Provides methods to extend the <see cref="Console"/> class.
/// </summary>
public static partial class ExtendedConsole
{
  private static string GetBlanks(
    int length) => new(' ', length);

  /// <summary>
  /// Tries to set the cursor position.
  /// </summary>
  public static bool TryRepositioning(
    string mex,
    bool avoidBlanks = false) => TryRepositioning(
      mex.Length,
      avoidBlanks);

  /// <summary>
  /// Tries to set the cursor position.
  /// </summary>
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

  /// <summary>
  /// Tries to set the cursor visibility.
  /// </summary>
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

  /// <summary>
  /// Deletes the last character, and move the cursor back.
  /// </summary>
  public static void DeleteLastChar()
  {
    var backspace = "\b";
    Console.Write(backspace);
    TryRepositioning(backspace);
  }
}
