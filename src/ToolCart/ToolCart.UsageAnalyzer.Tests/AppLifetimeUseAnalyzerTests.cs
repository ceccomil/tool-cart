namespace ToolCart.UsageAnalyzer.Tests;

[Collection("AppLifetimeUseAnalyzer Tests")]
public class AppLifetimeUseAnalyzerTests
{
  private static CSharpCompilation CreateCompilation(string source)
    => CSharpCompilation.Create("compilation",
      [
        CSharpSyntaxTree.ParseText(source)
      ],
      [
        MetadataReference
        .CreateFromFile(
          typeof(IHostApplicationLifetime)
          .GetTypeInfo()
          .Assembly
          .Location)
      ],
      new CSharpCompilationOptions(
        OutputKind
        .ConsoleApplication));

  [Fact]
  public async Task AppLifetimeUseAnalyzer_when_raise_warnings()
  {
    // Arrange
    var program =
      """
      using System;
      using Microsoft.Extensions.Hosting;

      namespace ToolCart.UsageAnalyzer.Tests;

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
          IApplicationLifetime lifeTime)
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
      new AppLifetimeUseAnalyzer()
    ];

    var compilation = CreateCompilation(program);

    // Act
    var diagnostics = await compilation
      .WithAnalyzers(auts)
      .GetAnalyzerDiagnosticsAsync();

    // Assert
    diagnostics
      .Length
      .ShouldBe(4);
  }
}
