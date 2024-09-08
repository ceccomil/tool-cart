namespace ToolCart.UsageAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ConsoleUseAnalyzer : DiagnosticAnalyzer
{
  public override ImmutableArray<
    DiagnosticDescriptor> SupportedDiagnostics =>
      ImmutableArray.Create(RuleIds.ConsoleUse);

  private void RaiseConsoleUseWarning(
    SyntaxTreeAnalysisContext ctx)
  {
    var consoleNodes = ctx
        .Tree
        .GetRoot()
        .DescendantNodes()
        .OfType<IdentifierNameSyntax>()
        .Where(x => x.Identifier.Text == nameof(Console))
        .ToList();

    foreach (var node in consoleNodes)
    {
      var location = node.GetLocation();

      var diagnostic = Diagnostic.Create(
        RuleIds.ConsoleUse,
        location);

      ctx.ReportDiagnostic(diagnostic);
    }
  }

  public override void Initialize(AnalysisContext context)
  {
    context
      .EnableConcurrentExecution();

    context
      .ConfigureGeneratedCodeAnalysis(
        GeneratedCodeAnalysisFlags.None);

    context
      .RegisterSyntaxTreeAction(
        RaiseConsoleUseWarning);
  }
}
