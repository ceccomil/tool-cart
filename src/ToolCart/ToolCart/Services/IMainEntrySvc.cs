namespace ToolCart.Services;

/// <summary>
/// Main entry service.
/// </summary>
public interface IMainEntrySvc : IHostedService
{
  /// <summary>
  /// Execution identifier.
  /// </summary>
  Guid ExecutionId { get; }

  /// <summary>
  /// Tool kick off.
  /// </summary>
  Task MainTask(CancellationToken cancellationToken);
}
