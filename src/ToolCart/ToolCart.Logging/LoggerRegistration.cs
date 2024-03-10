namespace ToolCart.Logging;

/// <summary>
/// The class for the logger registration.
/// </summary>
public static class LoggerRegistration
{
  private static ILoggingBuilder ToolCartLoggerRegistration(
    this ILoggingBuilder builder,
    LoggerFilters filters)
  {
    builder
      .ClearProviders()
      .AddCaptainLogger();

    foreach (var filter in filters)
    {
      builder.AddFilter(
        filter.Namespace,
        filter.Level);
    }

    return builder;
  }

  /// <summary>
  /// Default logger registration and configuration.
  /// To enable Console logging, either set the 
  /// "AllowConsoleLogging" to true in the appsettings.json,
  /// or add a "--allowConsoleLogging" command line argument.
  /// </summary>
  public static IServiceCollection AddDefaultLogger(
    this IServiceCollection services)
  {
    using var sp = services
      .BuildServiceProvider();

    var conf = sp
      .GetRequiredService<IConfiguration>();

    var recipients = Recipients.File;

    if ($"{conf["AllowConsoleLogging"]}"
      .Equals("true", StringComparison.OrdinalIgnoreCase))
    {
      recipients |= Recipients.Console;
    }

    var filtersOptions = sp
      .GetService<IOptions<LoggerFilters>>();

    var filters = filtersOptions?.Value
      ?? [];

    services
      .Configure<CaptainLoggerOptions>(opts =>
      {
        opts.DefaultColor = ConsoleColor.DarkGray;
        opts.LogRecipients = recipients;
        opts.FileRotation = LogRotation.Day;
        opts.TimeIsUtc = true;
      })
      .AddLogging(builder => builder
        .ToolCartLoggerRegistration(filters));

    return services;
  }
}
