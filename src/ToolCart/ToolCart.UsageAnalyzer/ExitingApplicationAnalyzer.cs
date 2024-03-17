namespace ToolCart.UsageAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ExitingApplicationAnalyzer : DiagnosticAnalyzer
{
  public override ImmutableArray<
    DiagnosticDescriptor> SupportedDiagnostics =>
    [
      RuleIds.AppLifetimeUse,
      RuleIds.EnvExitsUse
    ];

  private void RaiseAppLifetimeUse(
    SyntaxTreeAnalysisContext ctx)
  {
    var names = ctx
        .Tree
        .GetRoot()
        .DescendantNodes()
        .OfType<IdentifierNameSyntax>()
        .Where(x =>
          x.Identifier.Text == RuleIds.MSAPPHOST_LIFETIME ||
          x.Identifier.Text == RuleIds.MSAPP_LIFETIME)
        .ToList();

    foreach (var nis in names)
    {
      var location = nis.GetLocation();

      var diagnostic = Diagnostic.Create(
        RuleIds.AppLifetimeUse,
        location);

      ctx.ReportDiagnostic(diagnostic);
    }
  }

  private void RaiseEnvExitsUse(
    SyntaxTreeAnalysisContext ctx)
  {
    var exits = ctx
        .Tree
        .GetRoot()
        .DescendantNodes()
        .OfType<IdentifierNameSyntax>()
        .Where(x =>
          (x.Identifier.Text == RuleIds.EXIT ||
          x.Identifier.Text == RuleIds.EXIT_CODE ||
          x.Identifier.Text == RuleIds.ENV) &&
          x.Parent is not null)
        .Select(x => x.Parent)
        .Where(x => $"{x}".StartsWith(RuleIds.ENV))
        .Distinct()
        .ToList();

    foreach (var exit in exits)
    {
      var location = exit!.GetLocation();

      var diagnostic = Diagnostic.Create(
        RuleIds.EnvExitsUse,
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
      .RegisterSyntaxTreeAction(x =>
      {
        RaiseAppLifetimeUse(x);
        RaiseEnvExitsUse(x);
      });
  }
}
