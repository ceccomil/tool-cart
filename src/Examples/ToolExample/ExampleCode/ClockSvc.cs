namespace ToolExample;

public sealed class ClockSvc(
  IConsoleWrapper console,
  ILogger<ClockSvc> logger) : IExecutor
{
  private readonly IConsoleWrapper _console = console;
  private readonly ILogger _logger = logger;

  public async Task MainTask(
    CancellationToken cancellationToken)
  {
    var dt = DateTime.Now;
    var utc = dt.ToUniversalTime();

    var now = $"_q_Local time is {dt:yyyy-MM-dd} " +
      $"{dt:_aq_HH_e_:_aq_mm_e_:_aq_ss}" +
      "_d_ - " +
      $"_w_Utc time is {utc:yyyy-MM-dd} " +
      $"{utc:_aw_HH_e_:_aw_mm_e_:_aw_ss}";

    _console.WriteMixed(
      now,
      appendNewLine: false);

    _console.TryRepositioning(
      new CursorCoord(0, 0),
      clearFromOriginToCurrent: false);

    _logger.LogInformation(
      "ClockSvc MainTask executed at {Time}",
      DateTimeOffset.Now);

    await Task.Delay(
      250,
      cancellationToken);
  }
}
