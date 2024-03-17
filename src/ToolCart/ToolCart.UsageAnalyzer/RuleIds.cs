namespace ToolCart.UsageAnalyzer;

internal static class RuleIds
{
  public const string CONSOLE_USE = "TC0001";

  public static readonly DiagnosticDescriptor ConsoleUse = new(
    CONSOLE_USE,
    "Direct use of System.Console",
    "Replace usage of System.Console with " +
    "ToolCart.ConsoleHelpers.ExtendedConsole",
    "Design",
    DiagnosticSeverity.Warning,
    isEnabledByDefault: true);
}
