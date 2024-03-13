namespace ToolCart.Logging;

/// <summary>
/// The class for the logger registration.
/// </summary>
public static class LoggerRegistration
{
  private const string ALLOWCONSOLE = "AllowConsoleLogging";

  private static Recipients GetRecipients(
    this IServiceProvider sp)
  {
    var config = sp
      .GetRequiredService<IConfiguration>();

    var recipients = Recipients.File;

    if (config.ParameterlessArgCommandFound(
      ALLOWCONSOLE))
    {
      recipients |= Recipients.Console;
    }

    return recipients;
  }

  private static LoggerFilters GetFilters(
    this IServiceProvider sp)
  {
    var filtersOptions = sp
      .GetService<IOptions<LoggerFilters>>();

    if (filtersOptions is null)
    {
      return [];
    }

    return filtersOptions.Value;
  }

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

  private static IServiceCollection ConfigLogger(
    this IServiceCollection services,
    Recipients recipients)
  {
    services
      .Configure<CaptainLoggerOptions>(opts =>
      {
        opts.DefaultColor = ConsoleColor.DarkGray;
        opts.LogRecipients = recipients;
        opts.FileRotation = LogRotation.Day;
        opts.TimeIsUtc = true;
        opts.TriggerAsyncEvents = true;
      });

    return services;
  }

  private static IServiceCollection AddLogging(
    this IServiceCollection services,
    LoggerFilters filters)
  {
    services
      .AddLogging(
        builder => builder
          .ToolCartLoggerRegistration(filters));

    return services;
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

    services
      .ConfigLogger(sp.GetRecipients())
      .AddLogging(sp.GetFilters());

    return services;
  }
}
