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
          Console.ForegroundColor = ConsoleColor.Yellow;
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
      .Length
      .ShouldBe(3);
  }

  [Fact]
  public async Task ConsoleUseAnalyzer_when_using_static_raise_warnings()
  {
    // Arrange
    var program =
      """
      using static System.Console;

      namespace TestNs;

      public class Program
      {
        private static async Task ConsoleWrite(string message)
        {
          WriteLine(message);
          await Task.CompletedTask;
        }

        public async Task Main()
        {
          await Task.Delay(1000);
          await ConsoleWrite("message 1");
          await Task.Delay(1000);
          await ConsoleWrite("message 2");
          ForegroundColor = ConsoleColor.Yellow;
          WriteLine("Hello World!");
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
      .Length
      .ShouldBe(3);
  }
}
