namespace ToolCart.UsageAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ConsoleUseAnalyzer : DiagnosticAnalyzer
{
  public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [RuleIds.ConsoleUse];

  public override void Initialize(AnalysisContext context)
  {
    context.EnableConcurrentExecution();
    context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

    context.RegisterSyntaxNodeAction(
      AnalyzeInvocation,
      SyntaxKind.InvocationExpression);

    context.RegisterSyntaxNodeAction(
      AnalyzeIdentifierName,
      SyntaxKind.IdentifierName);
  }

  private static INamedTypeSymbol? GetConsoleType(
    SyntaxNodeAnalysisContext context) => context
      .SemanticModel
      .Compilation
      .GetTypeByMetadataName("System.Console");

  private static void AnalyzeInvocation(
    SyntaxNodeAnalysisContext context)
  {
    var consoleType = GetConsoleType(context);

    if (consoleType is null)
    {
      return;
    }

    var invocation = (InvocationExpressionSyntax)context.Node;

    var symbolInfo = context.SemanticModel.GetSymbolInfo(
      invocation,
      context.CancellationToken);

    var method = symbolInfo.Symbol as IMethodSymbol
      ?? symbolInfo
        .CandidateSymbols
        .OfType<IMethodSymbol>()
        .FirstOrDefault();

    if (method is null)
    {
      return;
    }

    if (!SymbolEqualityComparer.Default.Equals(
      method.ContainingType,
      consoleType))
    {
      return;
    }

    var diagnostic = Diagnostic.Create(
      RuleIds.ConsoleUse,
      invocation.GetLocation());

    context.ReportDiagnostic(diagnostic);
  }

  private static void AnalyzeIdentifierName(
    SyntaxNodeAnalysisContext context)
  {
    var consoleType = GetConsoleType(context);

    if (consoleType is null)
    {
      return;
    }

    var identifier = (IdentifierNameSyntax)context.Node;

    var symbolInfo = context.SemanticModel.GetSymbolInfo(
      identifier,
      context.CancellationToken);

    var symbol = symbolInfo.Symbol
      ?? symbolInfo
        .CandidateSymbols
        .FirstOrDefault();

    if (symbol is null)
    {
      return;
    }

    // Only properties/fields/events
    // (skip methods so we don't double-report WriteLine)
    ITypeSymbol? containingType = symbol switch
    {
      IPropertySymbol p => p.ContainingType,
      IFieldSymbol f => f.ContainingType,
      IEventSymbol e => e.ContainingType,
      _ => null
    };

    if (containingType is null)
    {
      return;
    }

    if (!SymbolEqualityComparer.Default.Equals(
      containingType,
      consoleType))
    {
      return;
    }

    var diagnostic = Diagnostic.Create(
      RuleIds.ConsoleUse,
      identifier.GetLocation());

    context.ReportDiagnostic(diagnostic);
  }
}
