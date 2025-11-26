namespace ToolCart.Services;

internal sealed class NullExecutionCorrelationScopeFactory
  : IExecutionCorrelationScopeFactory
{
  public static readonly NullExecutionCorrelationScopeFactory Instance = new();

  private sealed class NoopScope : IDisposable
  {
    public static readonly NoopScope Instance = new();
    public void Dispose()
    {
      // Nothing to do.
      // We keep a single instance to avoid allocations.
    }
  }

  public IDisposable BeginScope(Guid executionId) => NoopScope.Instance;
}
