namespace ToolCart.Tests;

public static class Collections
{
  public const string CONSOLE = "Console and Statics";

  public static StringWriter SharedConsoleOut { get; private set; } = new();

  public static void ResetConsoleOut()
  {
    SharedConsoleOut.Close();
    SharedConsoleOut.Dispose();

    SharedConsoleOut = new();

    Console.SetOut(SharedConsoleOut);
  }
}
