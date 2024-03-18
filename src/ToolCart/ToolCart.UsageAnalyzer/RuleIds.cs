namespace ToolCart.UsageAnalyzer;

internal static class RuleIds
{
  public const string MSAPPHOST_LIFETIME =
    "IHostApplicationLifetime";

  public const string MSAPP_LIFETIME =
    "IApplicationLifetime";

  public const string APPHANDLER =
    "ToolCart.Services.IAppHandler";

  public const string SYS_CONSOLE =
    $"{nameof(System)}.{nameof(Console)}";

  public const string EXT_CONSOLE =
    "ToolCart.Services.ExtendedConsole";

  public const string ENV = $"{nameof(Environment)}";

  public const string EXIT = $"{nameof(Environment.Exit)}";

  public const string EXIT_CODE = $"ExitCode";

  public const string CONSOLE_USE = "TC0001";
  public const string APP_LIFETIME = "TC0002";
  public const string ENV_EXITS = "TC0003";

  public static readonly DiagnosticDescriptor ConsoleUse = new(
    CONSOLE_USE,
    $"Direct use of {SYS_CONSOLE}",
    $"Replace usage of {SYS_CONSOLE} with " +
    EXT_CONSOLE,
    "Design",
    DiagnosticSeverity.Warning,
    isEnabledByDefault: true);

  public static readonly DiagnosticDescriptor AppLifetimeUse = new(
    APP_LIFETIME,
    $"Direct use of {MSAPPHOST_LIFETIME} or {MSAPP_LIFETIME}",
    $"Replace usage of {MSAPPHOST_LIFETIME} or {MSAPP_LIFETIME}" +
    $" with {APPHANDLER}",
    "Design",
    DiagnosticSeverity.Warning,
    isEnabledByDefault: true);

  public static readonly DiagnosticDescriptor EnvExitsUse = new(
    ENV_EXITS,
    $"Direct use of {ENV}.{EXIT}() or {ENV}.{EXIT_CODE}",
    $"Avoid usage of {ENV}.{EXIT}() or {ENV}.{EXIT_CODE}" +
    $" rely on {APPHANDLER}",
    "Design",
    DiagnosticSeverity.Warning,
    isEnabledByDefault: true);
}
