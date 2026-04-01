namespace ToolCart.Abstractions;

internal sealed class ConsoleWrapper : IConsoleWrapper
{
  public bool TryGetCursorCoord(out CursorCoord coord) => ExtendedConsole
    .TryGetCursorCoord(out coord);

  public bool TrySetCursorCoord(CursorCoord coord) => ExtendedConsole
    .TrySetCursorCoord(coord);

  public bool TryRepositioning(
    CursorCoord origin,
    bool clearFromOriginToCurrent = true) => ExtendedConsole
      .TryRepositioning(origin, clearFromOriginToCurrent);

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
    Theme? theme = null,
    bool avoidTabAutoCompletion = false) => ExtendedConsole
    .ReadLine(theme, avoidTabAutoCompletion);

  public string ReadLineFromUser(
    bool isAlert = false,
    bool avoidTabAutoCompletion = false) => ExtendedConsole
      .ReadLineFromUser(isAlert, avoidTabAutoCompletion);

  public string ReadPasswordFromUser() => ExtendedConsole
    .ReadPasswordFromUser();

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
