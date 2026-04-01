namespace ToolCart.Host;

/// <summary>
/// Tool host runner.
/// </summary>
public sealed class HostRunner
{
  private readonly HostApplicationBuilder _builder = new();

  private IHost? _app;

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

    if (AppBuilderConfig is not null)
    {
      await AppBuilderConfig.Invoke(_builder);
    }
  }

  private async Task RegisterServices()
  {
    _builder
      .Services
      .AddSingleton<IAppHandler, AppHandler>()
      .AddSingleton<IAppOrchestrator, AppOrchestrator>()
      .AddSingleton<IConsoleWrapper, ConsoleWrapper>()
      .AddHostedService(x => x
        .GetRequiredService<IAppOrchestrator>());

    if (ServicesConfig is not null)
    {
      await ServicesConfig.Invoke(_builder.Services);
    }
  }

  private async Task Initialization(
    string[]? args)
  {
    await ConfigureAppBuilder(args);
    await RegisterServices();
  }

  private async Task AppBuild()
  {
    _app = _builder.Build();

    if (AppConfig is not null)
    {
      await AppConfig.Invoke(_app);
    }
  }

  private async Task AppRun()
  {
    if (_app is null)
    {
      throw new InvalidOperationException(
        "Host has not been built.");
    }
    using (_app)
    {
      await _app.RunAsync();
    }  
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

    try
    {
      _ = ExtendedConsole.StartWait($"{startupMessage}...");

      await Initialization(args);

      _builder
        .Services
        .AddScoped<IExecutor, TMainService>();

      await AppBuild();
      await ExtendedConsole.StopWait();

      await AppRun();
    }
    finally
    {
      ExtendedConsole.TrySetCursorVisibility(true);
    }
  }

  /// <summary>
  /// Create and run the host application.
  /// </summary>
  /// <returns></returns>
  public async Task CreateAndRun<TMainService>(string[]? args) 
    where TMainService : class, IExecutor
  {
    await Initialization(args);

    _builder
      .Services
      .AddScoped<IExecutor, TMainService>();

    await AppBuild();
    await AppRun();
  }
}
