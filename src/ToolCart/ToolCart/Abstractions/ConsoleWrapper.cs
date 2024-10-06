namespace ToolCart.Abstractions;

/// <summary>
/// Wrapper for the <see cref="ExtendedConsole"/> class.
/// Inject this interface to read/write from/to the console.
/// </summary>
public interface IConsoleWrapper
{
  /// <summary>
  /// Tries to set the cursor position.
  /// </summary>
  bool TryRepositioning(
    string mex,
    bool avoidBlanks = false);

  /// <summary>
  /// Tries to set the cursor position.
  /// </summary>
  bool TryRepositioning(
    int len,
    bool avoidBlanks = false);

  /// <summary>
  /// Tries to set the cursor visibility.
  /// </summary>
  bool TrySetCursorVisibility(
    bool visible);

  /// <summary>
  /// Deletes the last character.
  /// </summary>
  void DeleteLastChar();

  /// <summary>
  /// Writes a themed message to the console.
  /// </summary>
  void Write(
    string mex,
    Theme? theme = default);

  /// <summary>
  /// Writes a themed message to the console followed by a new line.
  /// </summary>
  void WriteLine(
    string mex = "",
    Theme? theme = default);

  /// <summary>
  /// Sets custom themes.
  /// </summary>
  void SetCustomThemes(
    Theme question,
    Theme alertQuestion,
    Theme userInput,
    Theme alertUserInput,
    Theme info,
    Theme alertInfo,
    Theme warning,
    Theme alertWarning,
    Theme error,
    Theme alertError);


  /// <summary>
  /// Writes a question to the console.
  /// </summary>
  void WriteQuestion(
    string question,
    bool appendNewLine = true,
    bool isAlert = false);

  /// <summary>
  /// Writes an info message to to the console.
  /// </summary>
  void WriteInfo(
    string info,
    bool appendNewLine = true,
    bool isAlert = false);

  /// <summary>
  /// Writes a warning message to the console.
  /// </summary>
  void WriteWarning(
    string warning,
    bool appendNewLine = true,
    bool isAlert = false);

  /// <summary>
  /// Writes an error message to the console.
  /// </summary>
  void WriteError(
    string error,
    bool appendNewLine = true,
    bool isAlert = false);

  /// <summary>
  /// Write a mixed color message to the console.
  /// To change colors within the message, 
  /// use the following tags:
  /// <para>_i_ (info), _w_ (warning), _e_ (error), 
  /// _q_ (question), _u_ (user input)</para>
  /// <para>Add "a" to a tag to make it an alert.</para>
  /// e.g. "_q_Is this an _ae_error message _q_?"
  /// </summary>
  void WriteMixed(
    string taggedText,
    bool appendNewLine = true);

  /// <summary>
  /// Reads the next character from the standard input stream.
  /// </summary>
  int Read(Theme? theme = default);

  /// <summary>
  /// Reads the next character from the standard input stream.
  /// </summary>
  int ReadFromUser(bool isAlert = false);

  /// <summary>
  /// Obtains the next character or function key pressed by the user.
  /// The pressed key is displayed in the console window.
  /// </summary>
  ConsoleKeyInfo ReadKey(
    Theme? theme = default);

  /// <summary>
  /// Obtains the next character or function key pressed by the user.
  /// The pressed key is displayed in the console window.
  /// </summary>
  ConsoleKeyInfo ReadKeyFromUser(bool isAlert = false);

  /// <summary>
  /// Reads the next line of characters from the standard input stream.
  /// </summary>
  string ReadLine(Theme? theme = default);

  /// <summary>
  /// Reads the next line of characters from the standard input stream.
  /// </summary>
  string ReadLineFromUser(
    bool isAlert = false);

  /// <summary>
  /// Starts a wait with a message.
  /// </summary>
  Task StartWait(
    string mex,
    Theme? theme = default);

  /// <summary>
  /// Stops the wait.
  /// </summary>
  Task StopWait();
}

internal sealed class ConsoleWrapper
  : IConsoleWrapper
{
  public void DeleteLastChar() => ExtendedConsole.DeleteLastChar();

  public int Read(
    Theme? theme = null) => ExtendedConsole.Read(theme);

  public int ReadFromUser(
    bool isAlert = false) => ExtendedConsole.ReadFromUser(isAlert);

  public ConsoleKeyInfo ReadKey(
    Theme? theme = null) => ExtendedConsole.ReadKey(theme);

  public ConsoleKeyInfo ReadKeyFromUser(
    bool isAlert = false) => ExtendedConsole
      .ReadKeyFromUser(isAlert);

  public string ReadLine(
    Theme? theme = null) => ExtendedConsole.ReadLine(theme);

  public string ReadLineFromUser(
    bool isAlert = false) => ExtendedConsole
      .ReadLineFromUser(isAlert);

  public void SetCustomThemes(
    Theme question,
    Theme alertQuestion,
    Theme userInput,
    Theme alertUserInput,
    Theme info,
    Theme alertInfo,
    Theme warning,
    Theme alertWarning,
    Theme error,
    Theme alertError) => ExtendedConsole
      .SetCustomThemes(
        question,
        alertQuestion,
        userInput,
        alertUserInput,
        info,
        alertInfo,
        warning,
        alertWarning,
        error,
        alertError);

  public Task StartWait(
    string mex,
    Theme? theme = null) => ExtendedConsole
      .StartWait(mex, theme);

  public Task StopWait() => ExtendedConsole.StopWait();

  public bool TryRepositioning(
    string mex,
    bool avoidBlanks = false) => ExtendedConsole
      .TryRepositioning(mex, avoidBlanks);

  public bool TryRepositioning(
    int len,
    bool avoidBlanks = false) => ExtendedConsole
      .TryRepositioning(len, avoidBlanks);

  public bool TrySetCursorVisibility(
    bool visible) => ExtendedConsole
      .TrySetCursorVisibility(visible);

  public void Write(
    string mex,
    Theme? theme = null) => ExtendedConsole
      .Write(mex, theme);

  public void WriteError(
    string error,
    bool appendNewLine = true,
    bool isAlert = false) => ExtendedConsole
      .WriteError(error, appendNewLine, isAlert);

  public void WriteInfo(
    string info,
    bool appendNewLine = true,
    bool isAlert = false) => ExtendedConsole
      .WriteInfo(info, appendNewLine, isAlert);

  public void WriteLine(
    string mex = "",
    Theme? theme = null) => ExtendedConsole
      .WriteLine(mex, theme);

  public void WriteMixed(
    string taggedText,
    bool appendNewLine = true) => ExtendedConsole
      .WriteMixed(taggedText, appendNewLine);

  public void WriteQuestion(
    string question,
    bool appendNewLine = true,
    bool isAlert = false) => ExtendedConsole
      .WriteQuestion(question, appendNewLine, isAlert);

  public void WriteWarning(
    string warning,
    bool appendNewLine = true,
    bool isAlert = false) => ExtendedConsole
      .WriteWarning(warning, appendNewLine, isAlert);
}
