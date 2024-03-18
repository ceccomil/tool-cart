var hostRunner = new HostRunner()
{
  ServicesConfig = async services =>
  {
    services
      .Configure<LoggerFilters>(opts =>
      {
        opts.Add(new("ToolExample1", LogLevel.Debug));
        opts.Add(new("", LogLevel.Error));
      })
      .AddDefaultLogger();

    await Task.Delay(2_500);
  }
};

await hostRunner.CreateAndRun<ClockSvc>(
  args,
  "Application is starting");
