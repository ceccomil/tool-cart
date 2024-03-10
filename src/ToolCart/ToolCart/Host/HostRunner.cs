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
    : class, IExecutor
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
      .AddScoped(typeof(IExecutor), typeof(TMainService))
      .AddSingleton<IAppOrchestrator, AppOrchestrator>()
      .AddHostedService(x => x
        .GetRequiredService<IAppOrchestrator>());

    servicesConfig?.Invoke(builder.Services);

    var app = builder
      .Build();

    appConfig?.Invoke(app);

    await ExtendedConsole.StopWait();

    await app.RunAsync();

    ExtendedConsole.TrySetCursorVisibility(true);
  }
}
