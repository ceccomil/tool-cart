namespace ToolCart.Host;

/// <summary>
/// Tool host runner.
/// </summary>
public sealed class HostRunner
{
  private readonly HostApplicationBuilder _builder = new();

  /// <summary>
  /// Configure the application builder.
  /// </summary>
  public Func<IHostApplicationBuilder, Task>? AppBuilderConfig { get; set; }

  /// <summary>
  /// Configure the services collection.
  /// </summary>
  public Func<IServiceCollection, Task>? ServicesConfig { get; set; }

  /// <summary>
  /// Configure the application host.
  /// </summary>
  public Func<IHost, Task>? AppConfig { get; set; }

  private async Task ConfigureAppBuilder(
    string[]? args)
  {
    _builder
      .AddArgsConfiguration(args)
      .AddAppSettingsConfiguration();

    if (AppBuilderConfig is null)
    {
      return;
    }

    await AppBuilderConfig
      .Invoke(_builder);
  }

  private async Task RegisterServices()
  {
    _builder
      .Services
      .AddSingleton<IAppHandler, AppHandler>()
      .AddSingleton<IAppOrchestrator, AppOrchestrator>()
      .AddHostedService(x => x
        .GetRequiredService<IAppOrchestrator>());

    if (ServicesConfig is null)
    {
      return;
    }

    await ServicesConfig
      .Invoke(_builder.Services);
  }

  private async Task Initialization(
    string startupMessage,
    string[]? args)
  {
    _ = ExtendedConsole.StartWait($"{startupMessage}...");

    await ConfigureAppBuilder(args);

    await RegisterServices();
  }

  private async Task AppBuildAndRun()
  {
    using var app = _builder.Build();

    if (AppConfig is not null)
    {
      await AppConfig.Invoke(app);
    }

    await ExtendedConsole.StopWait();

    await app.RunAsync();
  }

  /// <summary>
  /// Create and run the host application.
  /// </summary>
  /// <returns></returns>
  public async Task CreateAndRun<TMainService>(
    string[]? args,
    string startupMessage) where TMainService
    : class, IExecutor
  {
    ExtendedConsole.TrySetCursorVisibility(false);

    await Initialization(startupMessage, args);

    _builder
      .Services
      .AddScoped(typeof(IExecutor), typeof(TMainService));

    await AppBuildAndRun();

    ExtendedConsole.TrySetCursorVisibility(true);
  }
}
