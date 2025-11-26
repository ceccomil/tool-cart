namespace ToolCart.Foundation;

/// <summary>
/// Wrapper for the ExtendedConsole class.
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
  /// <para>Use _d_ to change back to default color</para>
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
  /// Reads a password from the user.
  /// </summary>
  string ReadPasswordFromUser();

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
