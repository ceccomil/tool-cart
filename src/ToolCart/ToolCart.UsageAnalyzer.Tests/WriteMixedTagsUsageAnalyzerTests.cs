namespace ToolCart.UsageAnalyzer.Tests;

[Collection("WriteMixedTagsUsageAnalyzer Tests")]
public class WriteMixedTagsUsageAnalyzerTests
{
  private static CSharpCompilation CreateCompilation(string source)
    => CSharpCompilation.Create("compilation",
      [
        CSharpSyntaxTree.ParseText(source)
      ],
      [
        MetadataReference
        .CreateFromFile(
          typeof(IConsoleWrapper)
          .GetTypeInfo()
          .Assembly
          .Location)
      ],
      new CSharpCompilationOptions(
        OutputKind
        .ConsoleApplication));

  [Fact]
  public async Task WriteMixedTagsUsageAnalyzer_when_raise_warnings()
  {
    // Arrange
    var program =
      """
      using ToolCart.Foundation;
      using System;

      internal sealed class TestWriteMixed(
        IConsoleWrapper _console) : IDisposable
      {
        public void Dispose()
        {
          _console.WriteLine();

          _console.WriteMixed(
            "Class is _au_disposed_d_!");

          _console.WriteMixed(
            "Test _ax_wrong alert_d_!");

          GC.SuppressFinalize(this);
        }
      }
      """;

    ImmutableArray<DiagnosticAnalyzer> auts =
    [
      new WriteMixedTagsUsageAnalyzer()
    ];

    var compilation = CreateCompilation(program);

    // Act
    var diagnostics = await compilation
      .WithAnalyzers(auts)
      .GetAnalyzerDiagnosticsAsync();

    // Assert
    diagnostics
      .Length
      .ShouldBe(1);
  }
}
