namespace ToolCart.Foundation;

/// <summary>
/// Enables flushing of file logs.
/// </summary>
public interface IFileLogsFlusher
{
  /// <summary>
  /// Flushes the file logs.
  /// </summary>
  Task FlushAsync(CancellationToken cancellationToken);
}
