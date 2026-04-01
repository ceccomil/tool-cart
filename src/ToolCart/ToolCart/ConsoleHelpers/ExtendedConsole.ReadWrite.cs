namespace ToolCart.ConsoleHelpers;

internal static partial class ExtendedConsole
{
#if NET9_0_OR_GREATER
  private static readonly Lock _consoleLock = new();
#else
  private static readonly object _consoleLock = new();
#endif

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
    lock (_consoleLock)
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
  /// <para>Use _d_ to change back to default color</para>
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
    Theme? theme = default,
    bool avoidTabAutoCompletion = false)
  {
    var result = string.Empty;

    AcceptUserInput(() => 
      result = ReadLine(avoidTabAutoCompletion),
      theme);

    return result;
  }

  /// <summary>
  /// Reads the next line of characters from the standard input stream.
  /// </summary>
  public static string ReadLineFromUser(
    bool isAlert = false,
    bool avoidTabAutoCompletion = false)
  {
    var theme = UserInput;

    if (isAlert)
    {
      theme = AlertUserInput;
    }

    var result = string.Empty;

    AcceptUserInput(() =>
      result = ReadLine(avoidTabAutoCompletion),
      theme);

    return result;
  }

  /// <summary>
  /// Reads the next line of characters from the standard input stream without echoing the characters to the console.
  /// </summary>
  public static string ReadPasswordFromUser()
  {
    var password = "";

    while (true)
    {
      var key = Console.ReadKey(intercept: true);
      if (key.Key == ConsoleKey.Enter)
      {
        break;
      }

      if (key.Key == ConsoleKey.Backspace)
      {
        if (password.Length > 0)
        {
          password = password[..^1];
        }

        continue;
      }

      password += key.KeyChar;
    }

    return password;
  }

  private static string? ReadLine(bool avoidTab)
  {
    if (avoidTab)
    {
      return Console.ReadLine();
    }

    return ReadLineCoreWithTabPath();
  }

  private static string ReadLineCoreWithTabPath()
  {
    var sb = new StringBuilder();

    // where the input starts (caller wrote the question already)
    var startLeft = Console.CursorLeft;
    var startTop = Console.CursorTop;

    var lastLen = 0;

    void Render()
    {
      lock (_consoleLock)
      {
        Console.SetCursorPosition(startLeft, startTop);

        // clear previous content
        if (lastLen > 0)
        {
          Console.Write(new string(' ', lastLen));
          Console.SetCursorPosition(startLeft, startTop);
        }

        Console.Write(sb.ToString());
        lastLen = sb.Length;
      }
    }

    while (true)
    {
      var key = Console.ReadKey(intercept: true);

      if (key.Key == ConsoleKey.Enter)
      {
        Console.WriteLine();
        return sb.ToString();
      }

      if (key.Key == ConsoleKey.Backspace)
      {
        if (sb.Length > 0)
        {
          sb.Length--;
          Render();
        }

        continue;
      }

      if (key.Key == ConsoleKey.Tab)
      {
        if (TryTabCompletePath(sb))
        {
          Render();
        }
        else
        {
          Console.Beep();
        }

        continue;
      }

      if (!char.IsControl(key.KeyChar))
      {
        sb.Append(key.KeyChar);
        Render();
      }
    }
  }

  private static bool TryTabCompletePath(StringBuilder sb)
  {
    var line = sb.ToString();

    // token = from last whitespace (or inside quotes) to end
    var cursor = line.Length;

    var quoteCount = 0;
    for (var i = 0; i < cursor; i++)
    {
      if (line[i] == '"') { quoteCount++; }
    }

    var insideQuotes = (quoteCount % 2) == 1;

    int tokenStart;
    bool wasQuoted;

    if (insideQuotes)
    {
      var lastQuote = line.LastIndexOf('"');
      tokenStart = lastQuote >= 0 ? lastQuote + 1 : 0;
      wasQuoted = true;
    }
    else
    {
      var lastWs = line.LastIndexOfAny([' ', '\t']);
      tokenStart = lastWs >= 0 ? lastWs + 1 : 0;
      wasQuoted = false;
    }

    var token = line[tokenStart..];
    var tokenRaw = token.Trim('"');

    // Normalize to OS separator
    tokenRaw = tokenRaw
      .Replace('/', Path.DirectorySeparatorChar)
      .Replace('\\', Path.DirectorySeparatorChar);

    var endsWithSep = tokenRaw.EndsWith(Path.DirectorySeparatorChar);

    var dir = Path.GetDirectoryName(tokenRaw);
    var prefix = Path.GetFileName(tokenRaw) ?? string.Empty;

    var searchDir = string.IsNullOrEmpty(dir) ? "." : dir;

    if (endsWithSep)
    {
      searchDir = tokenRaw;
      prefix = string.Empty;
      dir = tokenRaw.TrimEnd(Path.DirectorySeparatorChar);
    }

    if (!Directory.Exists(searchDir))
    {
      return false;
    }

    // Get matches (dirs + files) starting with prefix
    var matches = Directory.EnumerateFileSystemEntries(searchDir, prefix + "*", SearchOption.TopDirectoryOnly)
      .Select(Path.GetFileName)
      .Where(x => !string.IsNullOrEmpty(x))
      .Distinct(StringComparer.OrdinalIgnoreCase)
      .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
      .ToArray();

    if (matches.Length == 0)
    {
      return false;
    }

    // One match => complete fully (+ \ if dir)
    if (matches.Length == 1)
    {
      var name = matches[0]!;

      var full = string.IsNullOrEmpty(dir) 
        ? name 
        : Path.Combine(dir, name);

      if (Directory.Exists(Path.Combine(searchDir, name)))
      {
        full += Path.DirectorySeparatorChar;
      }

      var replacement = BuildPathCompletionReplacement(
        full,
        alreadyQuoted: wasQuoted,
        isFinalCompletion: true);

      sb.Length = tokenStart;
      sb.Append(replacement);
      
      return true;
    }

    // Many matches => extend to common prefix
    var common = CommonPrefix(matches);
    if (common.Length <= prefix.Length)
    {
      return false; // nothing more to add
    }

    var extended = string.IsNullOrEmpty(dir) ? common : Path.Combine(dir, common);
    
    var repl2 = BuildPathCompletionReplacement(
      extended,
      alreadyQuoted: wasQuoted,
      isFinalCompletion: false);

    sb.Length = tokenStart;
    sb.Append(repl2);

    return true;
  }

  private static string BuildPathCompletionReplacement(
    string value,
    bool alreadyQuoted,
    bool isFinalCompletion)
  {
    if (alreadyQuoted)
    {
      return value;
    }

    if (!value.Contains(' '))
    {
      return value;
    }

    if (isFinalCompletion)
    {
      return $"\"{value}\"";
    }

    return $"\"{value}";
  }

  private static string CommonPrefix(string?[] names)
  {
    if (names.Length == 0)
    {
      return string.Empty;
    }

    var prefix = names[0] ?? string.Empty;

    for (var i = 1; i < names.Length; i++)
    {
      var other = names[i] ?? string.Empty;
      var len = Math.Min(prefix.Length, other.Length);

      var j = 0;
      while (j < len && char.ToLowerInvariant(prefix[j]) == char.ToLowerInvariant(other[j]))
      {
        j++;
      }

      prefix = prefix[..j];

      if (prefix.Length == 0)
      {
        break;
      }
    }

    return prefix;
  }
}
