global using Example1;
global using Microsoft.Extensions.Hosting;
global using ToolCart.ConsoleHelpers;
global using ToolCart.Host;
global using ToolCart.Services;


await HostRunner.CreateAndRun<TestSvc>(
  args,
  "Application is starting");