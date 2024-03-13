namespace ToolCart.Foundation.Helpers;

/// <summary>
/// Helper class for the <see cref="IConfiguration"/>."/>
/// </summary>
public static class ConfigurationExtensions
{
  private const string TRUE = "true";

  /// <summary>
  /// Check if the command line argument is found.
  /// </summary>
  public static bool ParameterlessArgCommandFound(
    this IConfiguration conf,
    string arg) => $"{conf[arg]}".Equals(
      TRUE,
      StringComparison
      .OrdinalIgnoreCase);
}
