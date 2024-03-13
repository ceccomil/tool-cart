namespace ToolCart.Logging;

/// <summary>
/// Logger filters.
/// </summary>
public class LoggerFilters : ICollection<LogFilter>
{
  private readonly List<LogFilter> _list = [];

  /// <summary>
  /// <inheritdoc />
  /// </summary>
  public int Count => _list.Count;

  /// <summary>
  /// <inheritdoc />
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// <inheritdoc />
  /// </summary>
  public void Add(LogFilter item) => _list
    .Add(item);

  /// <summary>
  /// <inheritdoc />
  /// </summary>
  public void Clear() => _list.Clear();

  /// <summary>
  /// <inheritdoc />
  /// </summary>
  public bool Contains(LogFilter item) => _list
    .Contains(item);

  /// <summary>
  /// <inheritdoc />
  /// </summary>
  public void CopyTo(
    LogFilter[] array,
    int arrayIndex) => _list.CopyTo(
      array,
      arrayIndex);

  /// <summary>
  /// <inheritdoc />
  /// </summary>
  public IEnumerator<LogFilter> GetEnumerator() => _list
    .GetEnumerator();

  /// <summary>
  /// <inheritdoc />
  /// </summary>
  public bool Remove(LogFilter item) => _list
    .Remove(item);

  /// <summary>
  /// <inheritdoc />
  /// </summary>
  IEnumerator IEnumerable.GetEnumerator() => _list
    .GetEnumerator();
}
