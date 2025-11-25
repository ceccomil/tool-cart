using ToolExample;

var hostRunner = HostBuilder.Get();

await hostRunner.CreateAndRun<ClockSvc>(
  args,
  ".NET8 Application is starting");
