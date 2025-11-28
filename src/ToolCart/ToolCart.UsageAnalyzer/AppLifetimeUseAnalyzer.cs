namespace ToolCart.UsageAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class AppLifetimeUseAnalyzer : DiagnosticAnalyzer
{
  public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    => [RuleIds.AppLifetimeUse];

  public override void Initialize(AnalysisContext context)
  {
    context.EnableConcurrentExecution();
    context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

    context.RegisterCompilationStartAction(StartAnalysis);
  }

  private static void StartAnalysis(
    CompilationStartAnalysisContext compilationContext)
  {
    var hostLifetimeType = compilationContext.Compilation
      .GetTypeByMetadataName(
        "Microsoft.Extensions.Hosting.IHostApplicationLifetime");

    var appLifetimeType = compilationContext.Compilation
      .GetTypeByMetadataName(
        "Microsoft.Extensions.Hosting.IApplicationLifetime");

    if (hostLifetimeType is null && appLifetimeType is null)
    {
      return;
    }

    compilationContext.RegisterSyntaxNodeAction(
      ctx => AnalyzeLifetimeUsage(
        ctx,
        hostLifetimeType,
        appLifetimeType),
      SyntaxKind.IdentifierName,
      SyntaxKind.QualifiedName);
  }

  private static void AnalyzeLifetimeUsage(
    SyntaxNodeAnalysisContext context,
    INamedTypeSymbol? hostLifetimeType,
    INamedTypeSymbol? appLifetimeType)
  {
    var node = context.Node;

    if (node.IsInNameOfOrTypeOf())
    {
      return;
    }

    var semanticModel = context.SemanticModel;

    SymbolInfo symbolInfo;
    switch (node)
    {
      case IdentifierNameSyntax id:
        symbolInfo = semanticModel.GetSymbolInfo(
          id, 
          context.CancellationToken);
        
        break;

      case QualifiedNameSyntax qn:
        symbolInfo = semanticModel.GetSymbolInfo(
          qn, 
          context.CancellationToken);
        
        break;

      default:
        return;
    }

    var symbol = symbolInfo.Symbol
      ?? symbolInfo.CandidateSymbols.FirstOrDefault();

    if (symbol is null)
    {
      return;
    }

    // Handle aliases: `using Lifetime = IHostApplicationLifetime;`
    INamedTypeSymbol? typeSymbol = null;

    switch (symbol)
    {
      case INamedTypeSymbol named:
        typeSymbol = named;
        break;

      case IAliasSymbol alias
        when alias.Target is INamedTypeSymbol aliasTarget:
        typeSymbol = aliasTarget;
        break;
    }

    if (typeSymbol is null)
    {
      return;
    }

    var matchesHost =
      hostLifetimeType is not null &&
      SymbolEqualityComparer.Default.Equals(typeSymbol, hostLifetimeType);

    var matchesApp =
      appLifetimeType is not null &&
      SymbolEqualityComparer.Default.Equals(typeSymbol, appLifetimeType);

    if (!matchesHost && !matchesApp)
    {
      return;
    }

    var diagnostic = Diagnostic.Create(
      RuleIds.AppLifetimeUse,
      node.GetLocation());

    context.ReportDiagnostic(diagnostic);
  }
}
