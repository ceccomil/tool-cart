namespace ToolCart.Logging;

/// <summary>
/// Log filter (namespace + level)
/// </summary>
public record LogFilter(
  string Namespace,
  LogLevel Level);
