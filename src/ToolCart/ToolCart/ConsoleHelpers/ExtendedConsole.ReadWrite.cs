namespace ToolCart.ConsoleHelpers;

internal static partial class ExtendedConsole
{
  internal static Theme Question { get; private set; } = new(
    ConsoleColor.Cyan,
    ConsoleColor.Black);

  internal static Theme AlertQuestion { get; private set; } = new(
    ConsoleColor.Black,
    ConsoleColor.Cyan);

  internal static Theme UserInput { get; private set; } = new(
    ConsoleColor.Magenta,
    ConsoleColor.Black);

  internal static Theme AlertUserInput { get; private set; } = new(
    ConsoleColor.Black,
    ConsoleColor.Magenta);

  internal static Theme Info { get; private set; } = new(
     ConsoleColor.Green,
     ConsoleColor.Black);

  internal static Theme AlertInfo { get; private set; } = new(
    ConsoleColor.Black,
    ConsoleColor.Green);

  internal static Theme Warning { get; private set; } = new(
     ConsoleColor.Yellow,
     ConsoleColor.Black);

  internal static Theme AlertWarning { get; private set; } = new(
    ConsoleColor.Black,
    ConsoleColor.Yellow);

  internal static Theme Error { get; private set; } = new(
     ConsoleColor.Red,
     ConsoleColor.Black);

  internal static Theme AlertError { get; private set; } = new(
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
  /// Write a mixed color message to the console.
  /// To change colors within the message, 
  /// use the following tags:
  /// <para>_i_ (info), _w_ (warning), _e_ (error), 
  /// _q_ (question), _u_ (user input)</para>
  /// <para>Add "a" to a tag to make it an alert.</para>
  /// e.g. "_q_Is this an _ae_error message _q_?"
  /// </summary>
  public static void WriteMixed(
    string taggedText,
    bool appendNewLine = true)
  {
    var texts = taggedText
      .SplitIntoThemes();

    foreach (var t in texts)
    {
      Write(t.Text.ToString(), t.Theme);
    }

    if (appendNewLine)
    {
      Write(Environment.NewLine);
    }
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
