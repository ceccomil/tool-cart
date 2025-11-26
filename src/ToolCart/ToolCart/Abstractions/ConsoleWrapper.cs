namespace ToolCart.Abstractions;

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
