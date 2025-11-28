namespace ToolCart.UsageAnalyzer.Tests;

[Collection("ExitingApplicationAnalyzer Tests")]
public class ExitingApplicationAnalyzerTests
{
  private static CSharpCompilation CreateCompilation(string source)
    => CSharpCompilation.Create("compilation",
      [
        CSharpSyntaxTree.ParseText(source)
      ],
      [
        MetadataReference
        .CreateFromFile(
          typeof(Environment)
          .GetTypeInfo()
          .Assembly
          .Location)
      ],
      new CSharpCompilationOptions(
        OutputKind
        .ConsoleApplication));

  [Fact]
  public async Task ExitingApplicationAnalyzer_when_raise_warnings()
  {
    // Arrange
    var program =
      """
      using System;

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
      new ExitingApplicationAnalyzer()
    ];

    var compilation = CreateCompilation(program);

    // Act
    var diagnostics = await compilation
      .WithAnalyzers(auts)
      .GetAnalyzerDiagnosticsAsync();

    // Assert
    diagnostics
      .Length
      .ShouldBe(2);
  }

  [Fact]
  public async Task ExitingApplicationAnalyzer_when_using_static_raise_warnings()
  {
    // Arrange
    var program =
      """
      using static System.Environment;

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

          var test = NewLine;
        }

        public void Dispose()
        {
          ExitCode = 10;
          Exit(0);
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
      .Length
      .ShouldBe(2);
  }

  [Fact]
  public async Task ExitingApplicationAnalyzer_when_using_aliases_raise_warnings()
  {
    // Arrange
    var program =
      """
      using AliasEnv = System.Environment;

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

          var test = AliasEnv.NewLine;
        }

        public void Dispose()
        {
          AliasEnv.ExitCode = 10;
          AliasEnv.Exit(10);
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
      .Length
      .ShouldBe(2);
  }
}