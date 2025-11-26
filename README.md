# tool-cart
Simple framework for generating .NET tools
=====================================================================

------------------------------------------------------------------
Source: [GitHub repo](https://github.com/ceccomil/tool-cart)

Packages
--------
| Package | NuGet Stable | NuGet Pre-release | Downloads |
| ------- | ------------ | ----------------- | --------- | 
| [ToolCart](https://www.nuget.org/packages/ToolCart) | [![ToolCart](https://img.shields.io/nuget/v/ToolCart.svg)](https://www.nuget.org/packages/ToolCart) | [![ToolCart](https://img.shields.io/nuget/vpre/ToolCart.svg)](https://www.nuget.org/packages/ToolCart/) | [![ToolCart](https://img.shields.io/nuget/dt/ToolCart.svg)](https://www.nuget.org/packages/ToolCart/) |
| [ToolCart.Foundation](https://www.nuget.org/packages/ToolCart.Foundation) | [![ToolCart.Foundation](https://img.shields.io/nuget/v/ToolCart.Foundation.svg)](https://www.nuget.org/packages/ToolCart.Foundation) | [![ToolCart.Foundation](https://img.shields.io/nuget/vpre/ToolCart.Foundation.svg)](https://www.nuget.org/packages/ToolCart.Foundation/) | [![ToolCart.Foundation](https://img.shields.io/nuget/dt/ToolCart.Foundation.svg)](https://www.nuget.org/packages/ToolCart.Foundation/) |
| [ToolCart.Logging](https://www.nuget.org/packages/ToolCart.Logging) | [![ToolCart.Logging](https://img.shields.io/nuget/v/ToolCart.Logging.svg)](https://www.nuget.org/packages/ToolCart.Logging) | [![ToolCart.Logging](https://img.shields.io/nuget/vpre/ToolCart.Logging.svg)](https://www.nuget.org/packages/ToolCart.Logging/) | [![ToolCart.Logging](https://img.shields.io/nuget/dt/ToolCart.Logging.svg)](https://www.nuget.org/packages/ToolCart.Logging/) |
| [ToolCart.UsageAnalyzer](https://www.nuget.org/packages/ToolCart.UsageAnalyzer) | [![ToolCart.UsageAnalyzer](https://img.shields.io/nuget/v/ToolCart.UsageAnalyzer.svg)](https://www.nuget.org/packages/ToolCart.UsageAnalyzer) | [![ToolCart.UsageAnalyzer](https://img.shields.io/nuget/vpre/ToolCart.UsageAnalyzer.svg)](https://www.nuget.org/packages/ToolCart.UsageAnalyzer/) | [![ToolCart.UsageAnalyzer](https://img.shields.io/nuget/dt/ToolCart.UsageAnalyzer.svg)](https://www.nuget.org/packages/ToolCart.UsageAnalyzer/) |


Solution overview
-----------------
- Targets: `.NET 8`, `.NET 10` for `ToolCart`, `ToolCart.Foundation`, `ToolCart.Logging`; `.NET Standard 2.0` for `ToolCart.UsageAnalyzer`.
- Examples: `Example1`, `Example2` show setup and usage.
- Tests: unit tests for console helpers and logging.

Core features (ToolCart)
-----------------------
- `ConsoleHelpers` with themed output and input:
  - `ExtendedConsole.Write`, `WriteLine`, `WriteInfo`, `WriteWarning`, `WriteError`, `WriteQuestion`.
  - `WriteMixed` supports inline tags: `_i_` info, `_w_` warning, `_e_` error, `_q_` question, `_u_` user input; add `a` for alert (e.g. `_ae_`). Use `_d_` to reset to default.
  - `Read`, `ReadKey`, `ReadLine`, `ReadPasswordFromUser` with proper cursor visibility handling.
  - `StartWait`/`StopWait` to show a wait message/spinner.
- `Abstractions` include `IConsoleWrapper` with `ConsoleWrapper` forwarding to `ExtendedConsole` for DI-friendly console interactions.
- Theme customization via `ExtendedConsole.SetCustomThemes` or `IConsoleWrapper.SetCustomThemes`.

Foundation
----------
- Common helpers and types (e.g., configuration helpers, `ErrorCode`).
- Shared `Usings` and conventions for tool apps.

Logging
-------
- Filter and registration helpers: `LoggerFilters`, `LoggerRegistration`, `LogFilter` to integrate with Microsoft.Extensions.Logging.
- Tests show expected registrations and filtering behavior.

Usage Analyzer
--------------
- Roslyn analyzer (`ToolCart.UsageAnalyzer`) to guide best practices.
- Rules:
  - `TC0001` Console use: warns on direct `System.Console` usage; prefer `ToolCart.Abstractions.IConsoleWrapper`.
  - `TC0002` App lifetime: warns on direct `IHostApplicationLifetime`/`IApplicationLifetime`; prefer `ToolCart.Services.IAppHandler`.
  - `TC0003` Environment exits: warns on `Environment.Exit` or `Environment.ExitCode`; prefer controlled shutdown via app handler.

Quick start
-----------
1. Add packages:
   - Core: `ToolCart`
   - Optional: `ToolCart.Foundation`, `ToolCart.Logging`, `ToolCart.UsageAnalyzer`
2. Register services and console abstraction in DI.
3. Use `IConsoleWrapper` for IO and theming.

Examples
--------
- `Example1` shows a simple service using DI and console interactions.
- `Example2` demonstrates configuration via `appsettings.json` and tool bootstrap.

Theming and mixed output
------------------------
- Customize themes:
  - Call `SetCustomThemes(question, alertQuestion, userInput, alertUserInput, info, alertInfo, warning, alertWarning, error, alertError)` to override defaults.
- Mixed output:
  - Pass a tagged string to `WriteMixed("_q_Question _i_Info _w_Warn _e_Error _d_Back")` to switch themes inline. Use `appendNewLine` when needed.

User input
----------
- Use `ReadLineFromUser()` / `ReadKeyFromUser()`; set `isAlert: true` to use alert theme.
- `ReadPasswordFromUser()` reads without echoing characters; supports backspace and enter termination.

Wait messages
-------------
- `StartWait(mex, theme)` shows a wait indicator; call `StopWait()` to finish. Cursor visibility is managed for a clean UX.

Testing
-------
- `ToolCart.Tests` include `ExtendedConsoleTests` covering theming and mixed writes.
- `ToolCart.Logging.Tests` validate logging registration and filters.

Targets and compatibility
-------------------------
- Libraries target modern runtimes for performance and features.
- Analyzer targets `.NET Standard 2.0` for broad IDE compatibility.

Example: Interactive tool with logging, correlation & retries
-------------------------

Program.cs
```csharp
global using Microsoft.Extensions.Logging;
global using ToolCart.Foundation;
global using ToolCart.Services;
using Microsoft.Extensions.DependencyInjection;
using ToolCart.Host;
using ToolCart.Logging;
using ToolExample4;

var hostRunner = new HostRunner
{
  ServicesConfig = services =>
  {
    services
      .Configure<LoggerFilters>(opts =>
      {
        // Keep framework noise low
        opts.Add(new("System", LogLevel.Error));
        opts.Add(new("Microsoft", LogLevel.Warning));

        opts.Add(new("MyTool", LogLevel.Information));
        opts.Add(new("ToolExample4", LogLevel.Debug));
      })
      .AddDefaultLogger()
      .AddScoped<IExecutor, MyToolExecutor>();

    return Task.CompletedTask;
  }
};

await hostRunner.CreateAndRun<MyToolExecutor>(
  args,
  "MyTool is starting");
```

MyToolExecutor.cs
```csharp
namespace ToolExample4;

internal sealed class MyToolExecutor(
  ILogger<MyToolExecutor> logger,
  IConsoleWrapper console,
  IAppHandler appHandler)
  : IExecutor
{
  private readonly ILogger<MyToolExecutor> _logger = logger;
  private readonly IConsoleWrapper _console = console;
  private readonly IAppHandler _appHandler = appHandler;

  public async Task MainTask(
    CancellationToken cancellationToken)
  {
    _logger.LogInformation(
      "MyTool main task started.");

    _console.WriteMixed(
      "_i_Welcome to _au_MyTool_i_ powered by ToolCart!");

    for (var i = 0; i < 3 && !cancellationToken.IsCancellationRequested; i++)
    {
      _console.WriteQuestion(
        $"Tick {i + 1}");

      _logger.LogInformation(
        "Tick {Tick}",
        i + 1);

      await Task.Delay(
        500,
        cancellationToken);
    }

    _console.WriteMixed(
      "Done. Press _au_ENTER_d_ to repeat or _au_ESCAPE_d_ to exit.");

    var key = _console.ReadKeyFromUser();

    _console.WriteLine();

    if (key.Key == ConsoleKey.Escape)
    {
      _logger.LogInformation(
        "User chose to exit the tool.");

      _appHandler.Exit();
      return;
    }
    
    if (key.Key == ConsoleKey.Enter)
    {
      _logger.LogInformation(
        "User chose to repeat the main task.");

      return;
    }

    _logger.LogError(
      "User input is not valid!");

   _appHandler.Exit(1);
  }
}
```

