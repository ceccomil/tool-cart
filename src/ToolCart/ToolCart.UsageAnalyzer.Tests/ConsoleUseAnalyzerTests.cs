namespace ToolCart.UsageAnalyzer.Tests;

[Collection("ConsoleUseAnalyzer Tests")]
public class ConsoleUseAnalyzerTests
{
  private static CSharpCompilation CreateCompilation(string source)
    => CSharpCompilation.Create("compilation",
      [
        CSharpSyntaxTree.ParseText(source)
      ],
      new[]
      {
        MetadataReference
        .CreateFromFile(
          typeof(Console)
          .GetTypeInfo()
          .Assembly
          .Location)
      },
      new CSharpCompilationOptions(
        OutputKind
        .ConsoleApplication));

  [Fact]
  public async Task ConsoleUseAnalyzer_when_raise_warnings()
  {
    // Arrange
    var program =
      """
      using System;

      public class Program
      {
        private async Task ConsoleWrite(
          string message)
        {
          Console
            .WriteLine(message);

          await Task.CompletedTask;
        }

        public async Task Main(
          string[]? args = default)
        {
          await Task.Delay(1000);
          await new ConsoleWrite("message 1");
          await Task.Delay(1000);
          await new ConsoleWrite("message 2");
          Console.WriteLine("Hello World!");
        }
      }
      """;

    ImmutableArray<DiagnosticAnalyzer> auts =
    [
      new ConsoleUseAnalyzer()
    ];

    var compilation = CreateCompilation(program);

    // Act
    var diagnostics = await compilation
      .WithAnalyzers(auts)
      .GetAnalyzerDiagnosticsAsync();

    // Assert
    diagnostics
      .Should()
      .HaveCount(2);
  }

  [Fact]
  public async Task ExitingApplicationAnalyzer_when_raise_warnings()
  {
    // Arrange
    var program =
      """
      public interface ITestSvc : IDisposable
      {
        void Test();
      }

      internal sealed class TestSvc : ITestSvc
      {
        private readonly IHostApplicationLifetime _hostLifeTime;
        private readonly IApplicationLifetime _lifeTime;

        public TestSvc(
          IHostApplicationLifetime hostLifeTime,
          IHostApplicationLifetime lifeTime)
        {
          _hostLifeTime = hostLifeTime;
          _lifeTime = lifeTime;

          var test = Environment.NewLine;
        }

        public void Dispose()
        {
          Environment.ExitCode = 10;
          Environment.Exit(0);
          GC.SuppressFinalize(this);
        }

        public void Test()
        {
          _hostLifeTime.StopApplication();
        }
      }
      """;

    ImmutableArray<DiagnosticAnalyzer> auts =
    [
      new ExitingApplicationAnalyzer()
    ];

    var compilation = CreateCompilation(program);

    // Act
    var diagnostics = await compilation
      .WithAnalyzers(auts)
      .GetAnalyzerDiagnosticsAsync();

    // Assert
    diagnostics
      .Should()
      .HaveCount(6);
  }
}
