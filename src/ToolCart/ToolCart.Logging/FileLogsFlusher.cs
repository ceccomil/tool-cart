namespace ToolCart.Logging;

internal sealed class FileLogsFlusher(
  ICaptainLoggerFlusher _flusher) : IFileLogsFlusher
{
  public Task FlushAsync(CancellationToken cancellationToken) => _flusher
    .FlushAsync(cancellationToken);
}
