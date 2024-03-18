namespace ToolExample1;

internal class ClockSvc : IExecutor
{
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

    ExtendedConsole.WriteMixed(
      now,
      appendNewLine: false);

    ExtendedConsole.TryRepositioning(
      67,
      avoidBlanks: true);

    await Task.Delay(
      250,
      cancellationToken);
  }
}
