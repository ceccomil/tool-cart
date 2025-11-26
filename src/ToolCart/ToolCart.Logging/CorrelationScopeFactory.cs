namespace ToolCart.Logging;

internal sealed class CorrelationScopeFactory
  : IExecutionCorrelationScopeFactory
{
  public IDisposable BeginScope(Guid executionId)
  {
    return CaptainLoggerCorrelationScope.BeginScope(executionId);
  }
}
