namespace ToolCart.Configuration;

internal static class Extensions
{
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

  public static IHostApplicationBuilder AddAppSettingsConfiguration(
    this IHostApplicationBuilder builder)
  {
    IConfigurationBuilder confBuilder = builder
      .Configuration;

    var basePath = AppContext.BaseDirectory;

    confBuilder.AddJsonFile(
      Path.Combine(basePath, "appsettings.json"),
      optional: true,
      reloadOnChange: false);

    return builder;
  }
}
