﻿namespace ToolCart.Configuration;

/// <summary>
/// Extension methods for <see cref="HostApplicationBuilder"/>.
/// </summary>
public static class Extensions
{
  /// <summary>
  /// Adds the <see cref="CommandArgumentsProvider"/> to the configuration.
  /// </summary>
  public static IHostApplicationBuilder AddArgsConfiguration(
    this IHostApplicationBuilder builder,
    string[]? args = default)
  {
    IConfigurationBuilder confBuilder = builder
      .Configuration;

    confBuilder.Add(
      new CommandArgumentsProvider(args));

    return builder;
  }

  /// <summary>
  /// Adds the appsettings.json file to the configuration.
  /// </summary>
  public static IConfigurationBuilder AddAppSettingsConfiguration(
    this IConfigurationBuilder builder)
  {
    string assemblyFolder = Path
      .GetDirectoryName(
        Assembly
        .GetExecutingAssembly()
        .Location)!;

    builder
     .AddJsonFile(
       Path.Combine(
         assemblyFolder,
         "appsettings.json"),
       optional: true);

    return builder;
  }
}
