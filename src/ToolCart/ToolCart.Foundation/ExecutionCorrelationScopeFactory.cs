namespace ToolCart.Foundation;

/// <summary>
/// Provides a factory for creating logical operation scopes that associate actions with a specific execution context
/// identifier.
/// </summary>
public interface IExecutionCorrelationScopeFactory
{
  /// <summary>
  /// Begins a logical operation scope associated with the specified execution identifier.
  /// </summary>
  /// <remarks>Use the returned <see cref="IDisposable"/> within a <c>using</c> statement to ensure the scope is
  /// properly disposed. Scopes can be used to correlate log entries or operations within the same execution
  /// context.</remarks>
  /// <param name="executionId">The unique identifier for the execution context to associate with the scope.</param>
  /// <returns>An <see cref="IDisposable"/> that ends the scope when disposed.</returns>
  IDisposable BeginScope(Guid executionId);
}