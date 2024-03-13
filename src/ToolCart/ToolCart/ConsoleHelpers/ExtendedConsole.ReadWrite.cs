namespace ToolCart.ConsoleHelpers;

public static partial class ExtendedConsole
{
  private static Theme Question { get; set; } = new(
    ConsoleColor.Cyan,
    ConsoleColor.Black);

  private static Theme AlertQuestion { get; set; } = new(
    ConsoleColor.Black,
    ConsoleColor.Cyan);

  private static Theme UserInput { get; set; } = new(
    ConsoleColor.Magenta,
    ConsoleColor.Black);

  private static Theme AlertUserInput { get; set; } = new(
    ConsoleColor.Black,
    ConsoleColor.Magenta);

  private static Theme Info { get; set; } = new(
     ConsoleColor.Green,
     ConsoleColor.Black);

  private static Theme AlertInfo { get; set; } = new(
    ConsoleColor.Black,
    ConsoleColor.Green);

  private static Theme Warning { get; set; } = new(
     ConsoleColor.Yellow,
     ConsoleColor.Black);

  private static Theme AlertWarning { get; set; } = new(
    ConsoleColor.Black,
    ConsoleColor.Yellow);

  private static Theme Error { get; set; } = new(
     ConsoleColor.Red,
     ConsoleColor.Black);

  private static Theme AlertError { get; set; } = new(
    ConsoleColor.Black,
    ConsoleColor.Red);

  private static void AcceptUserInput(
    Action consoleAction,
    Theme? theme = default)
  {
    TrySetCursorVisibility(true);

    theme ??= new(
      Console.ForegroundColor,
      Console.BackgroundColor);

    Console.ForegroundColor = theme.Foreground;
    Console.BackgroundColor = theme.Background;

    consoleAction();

    Console.ForegroundColor = theme.BeforeForeground;
    Console.BackgroundColor = theme.BeforeBackground;

    TrySetCursorVisibility(false);
  }

  private static void Write(
    string mex,
    Theme? theme,
    bool appendNewLine)
  {
    Write(mex, theme);

    if (appendNewLine)
    {
      Write(Environment.NewLine);
    }
  }

  /// <summary>
  /// Writes a themed message to the console.
  /// </summary>
  public static void Write(
    string mex,
    Theme? theme = default)
  {
    theme ??= new(
      Console.ForegroundColor,
      Console.BackgroundColor);

    Console.ForegroundColor = theme.Foreground;
    Console.BackgroundColor = theme.Background;

    Console.Write(mex);

    Console.ForegroundColor = theme.BeforeForeground;
    Console.BackgroundColor = theme.BeforeBackground;
  }

  /// <summary>
  /// Writes a themed message to the console followed by a new line.
  /// </summary>
  public static void WriteLine(
    string mex = "",
    Theme? theme = default) => Write(
      mex,
      theme,
      appendNewLine: true);

  /// <summary>
  /// Sets custom themes.
  /// </summary>
  public static void SetCustomThemes(
    Theme question,
    Theme alertQuestion,
    Theme userInput,
    Theme alertUserInput,
    Theme info,
    Theme alertInfo,
    Theme warning,
    Theme alertWarning,
    Theme error,
    Theme alertError)
  {
    Question = question;
    AlertQuestion = alertQuestion;
    UserInput = userInput;
    AlertUserInput = alertUserInput;
    Info = info;
    AlertInfo = alertInfo;
    Warning = warning;
    AlertWarning = alertWarning;
    Error = error;
    AlertError = alertError;
  }

  /// <summary>
  /// Writes a question to the console.
  /// </summary>
  public static void WriteQuestion(
    string question,
    bool appendNewLine = true,
    bool isAlert = false)
  {
    var theme = Question;

    if (isAlert)
    {
      theme = AlertQuestion;
    }

    Write(
      question,
      theme,
      appendNewLine);
  }

  /// <summary>
  /// Writes an info message to to the console.
  /// </summary>
  public static void WriteInfo(
    string info,
    bool appendNewLine = true,
    bool isAlert = false)
  {
    var theme = Info;

    if (isAlert)
    {
      theme = AlertInfo;
    }

    Write(
      info,
      theme,
      appendNewLine);
  }

  /// <summary>
  /// Writes a warning message to the console.
  /// </summary>
  public static void WriteWarning(
    string warning,
    bool appendNewLine = true,
    bool isAlert = false)
  {
    var theme = Warning;

    if (isAlert)
    {
      theme = AlertWarning;
    }

    Write(
      warning,
      theme,
      appendNewLine);
  }

  /// <summary>
  /// Writes an error message to the console.
  /// </summary>
  public static void WriteError(
    string error,
    bool appendNewLine = true,
    bool isAlert = false)
  {
    var theme = Error;

    if (isAlert)
    {
      theme = AlertError;
    }

    Write(
      error,
      theme,
      appendNewLine);
  }

  /// <summary>
  /// Reads the next character from the standard input stream.
  /// </summary>
  public static int Read(
    Theme? theme = default)
  {
    var result = -1;

    AcceptUserInput(
      () => result = Console.Read(),
      theme);

    return result;
  }

  /// <summary>
  /// Reads the next character from the standard input stream.
  /// </summary>
  public static int ReadFromUser(
    bool isAlert = false)
  {
    var theme = UserInput;

    if (isAlert)
    {
      theme = AlertUserInput;
    }

    return Read(theme);
  }

  /// <summary>
  /// Obtains the next character or function key pressed by the user.
  /// The pressed key is displayed in the console window.
  /// </summary>
  public static ConsoleKeyInfo ReadKey(
    Theme? theme = default)
  {
    ConsoleKeyInfo key = default;

    AcceptUserInput(
      () => key = Console.ReadKey(),
      theme);

    return key;
  }

  /// <summary>
  /// Obtains the next character or function key pressed by the user.
  /// The pressed key is displayed in the console window.
  /// </summary>
  public static ConsoleKeyInfo ReadKeyFromUser(
    bool isAlert = false)
  {
    var theme = UserInput;

    if (isAlert)
    {
      theme = AlertUserInput;
    }

    return ReadKey(theme);
  }

  /// <summary>
  /// Reads the next line of characters from the standard input stream.
  /// </summary>
  public static string ReadLine(
    Theme? theme = default)
  {
    var result = string.Empty;

    AcceptUserInput(
      () => result = Console.ReadLine(),
      theme);

    return result;
  }

  /// <summary>
  /// Reads the next line of characters from the standard input stream.
  /// </summary>
  public static string ReadLineFromUser(
    bool isAlert = false)
  {
    var theme = UserInput;

    if (isAlert)
    {
      theme = AlertUserInput;
    }

    return ReadLine(theme);
  }
}
