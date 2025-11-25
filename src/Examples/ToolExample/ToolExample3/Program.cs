using ToolExample;

var hostRunner = HostBuilder.Get();

await hostRunner.CreateAndRun<ClockSvc>(
  args,
  ".NET tool is starting");
