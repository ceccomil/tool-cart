namespace ToolCart.Services;

/// <summary>
/// Main service responsible for the 
/// single or repeatable scoped execution.
/// </summary>
public interface IExecutor
{
  /// <summary>
  /// Main task to be executed.
  /// </summary>
  Task MainTask(
    CancellationToken cancellationToken);
}
