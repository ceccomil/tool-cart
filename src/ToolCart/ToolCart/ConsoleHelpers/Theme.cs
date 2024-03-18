namespace ToolCart.ConsoleHelpers;

/// <summary>
/// Represents a color theme.
/// </summary>
public record Theme(
  ConsoleColor Foreground,
  ConsoleColor Background)
{
  /// <summary>
  /// Gets the console's foreground color before the theme was applied.
  /// </summary>
  public ConsoleColor BeforeForeground { get; } = Console.ForegroundColor;

  /// <summary>
  /// Gets the console's background color before the theme was applied.
  /// </summary>
  public ConsoleColor BeforeBackground { get; } = Console.BackgroundColor;

  /// <summary>
  /// <inheritdoc />
  /// </summary>
  public override string ToString() =>
    $"Foreground: {Foreground}, " +
    $"Background: {Background}";
}
