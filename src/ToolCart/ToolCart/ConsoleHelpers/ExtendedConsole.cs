namespace ToolCart.ConsoleHelpers;

internal static partial class ExtendedConsole
{
  public static bool TryGetCursorCoord(out CursorCoord coord)
  {
    try
    {
      coord = new CursorCoord(Console.CursorLeft, Console.CursorTop);
      return true;
    }
    catch (IOException)
    {
      coord = default;
      return false;
    }
  }

  public static bool TrySetCursorCoord(CursorCoord coord)
  {
    try
    {
      Console.SetCursorPosition(coord.Left, coord.Top);
      return true;
    }
    catch (IOException)
    {
      return false;
    }
  }

  public static bool TrySetCursorVisibility(
    bool visible)
  {
    try
    {
      Console.GetCursorPosition();
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

    var left = Console.CursorLeft - 1;

    if (left < 0)
    {
      left = 0;
    }

    Console.SetCursorPosition(
      left,
      Console.CursorTop);
  }

  public static bool TryRepositioning(
    CursorCoord origin,
    bool clearFromOriginToCurrent = true)
  {
    try
    {
      var current = new CursorCoord(Console.CursorLeft, Console.CursorTop);

      if (clearFromOriginToCurrent)
      {
        ClearRange(origin, current);
      }

      Console.SetCursorPosition(origin.Left, origin.Top);
      return true;
    }
    catch (IOException)
    {
      Console.Write(Environment.NewLine);
      Console.Write(Environment.NewLine);
      return false;
    }
  }

  private static void ClearRange(
    CursorCoord start, 
    CursorCoord end)
  {
    var width = Console.BufferWidth;

    for (var row = start.Top; row <= end.Top; row++)
    {
      var from = row == start.Top ? start.Left : 0;
      var toExclusive = row == end.Top ? end.Left : width;

      var count = toExclusive - from;
      if (count <= 0)
      {
        continue;
      }

      Console.SetCursorPosition(from, row);
      Console.Write(new string(' ', count));
    }
  }
}
