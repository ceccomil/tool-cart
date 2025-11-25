using ToolExample;

var hostRunner = HostBuilder.Get();

await hostRunner.CreateAndRun<ClockSvc>(
  args,
  ".NET10 Application is starting");
