namespace ToolCart.Host;

/// <summary>
/// Tool host runner.
/// </summary>
public static class HostRunner
{
  /// <summary>
  /// Create and run the host application.
  /// </summary>
  /// <returns></returns>
  public static async Task CreateAndRun<TMainService>(
    string[]? args,
    string startupMessage,
    Action<IHostApplicationBuilder>? appBuilderConfig = default,
    Action<IServiceCollection>? servicesConfig = default,
    Action<IHost>? appConfig = default) where TMainService
    : class, IMainEntrySvc
  {
    ExtendedConsole.TrySetCursorVisibility(false);

    _ = ExtendedConsole.StartWait($"{startupMessage}...");

    await Task.Delay(2_000);

    var builder = new HostApplicationBuilder();

    appBuilderConfig?.Invoke(builder);

    builder
      .AddArgsConfiguration(args);

    builder
      .Configuration
      .AddAppSettingsConfiguration();

    builder
      .Services
      .AddSingleton(typeof(IMainEntrySvc), typeof(TMainService))
      .AddHostedService(x => x
        .GetRequiredService<IMainEntrySvc>());

    servicesConfig?.Invoke(builder.Services);

    var app = builder
      .Build();

    appConfig?.Invoke(app);

    await ExtendedConsole.StopWait();

    await app.RunAsync();

    ExtendedConsole.TrySetCursorVisibility(true);
  }
}
