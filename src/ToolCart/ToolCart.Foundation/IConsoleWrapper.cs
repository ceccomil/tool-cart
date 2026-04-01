namespace ToolCart.Foundation;

/// <summary>
/// Wrapper for the ExtendedConsole class.
/// Inject this interface to read/write from/to the console.
/// </summary>
public interface IConsoleWrapper
{
  /// <summary>
  /// Attempts to retrieve the current cursor coordinates.
  /// </summary>
  /// <param name="coord">When this method returns, contains the coordinates of the cursor if the operation succeeds; otherwise, contains
  /// the default value.</param>
  /// <returns><see langword="true"/> if the cursor coordinates were successfully retrieved; otherwise, <see langword="false"/>.</returns>
  bool TryGetCursorCoord(out CursorCoord coord);

  /// <summary>
  /// Attempts to set the cursor position to the specified coordinates.
  /// </summary>
  /// <param name="coord">The target coordinates to move the cursor to.</param>
  /// <returns>true if the cursor position was successfully set; otherwise, false.</returns>
  bool TrySetCursorCoord(CursorCoord coord);

  /// <summary>
  /// Attempts to reposition the cursor to the specified origin coordinate.
  /// </summary>
  /// <param name="origin">The target coordinate to which the cursor should be moved.</param>
  /// <param name="clearFromOriginToCurrent">true to clear the area between the origin and the current cursor position after repositioning; otherwise, false.
  /// The default is true.</param>
  /// <returns>true if the cursor was successfully repositioned; otherwise, false.</returns>
  bool TryRepositioning(
    CursorCoord origin,
    bool clearFromOriginToCurrent = true);

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
  string ReadLine(
    Theme? theme = default, 
    bool avoidTabAutoCompletion = false);

  /// <summary>
  /// Reads the next line of characters from the standard input stream.
  /// </summary>
  string ReadLineFromUser(
    bool isAlert = false,
    bool avoidTabAutoCompletion = false);

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
