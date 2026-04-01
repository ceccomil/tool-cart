namespace ToolCart.Foundation;

/// <summary>
/// Represents a position in a two-dimensional coordinate system, typically used for cursor or console buffer locations.
/// </summary>
/// <param name="left">The horizontal position of the coordinate. Represents the number of columns from the left edge, where 0 is the
/// leftmost position.</param>
/// <param name="top">The vertical position of the coordinate. Represents the number of rows from the top edge, where 0 is the topmost
/// position.</param>
public readonly struct CursorCoord(int left, int top)
{
  /// <summary>
  /// Gets the left coordinate value.
  /// </summary>
  public int Left { get; } = left;

  /// <summary>
  /// Gets the top coordinate value.
  /// </summary>
  public int Top { get; } = top;
}
