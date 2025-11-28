namespace ToolCart.UsageAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ExitingApplicationAnalyzer : DiagnosticAnalyzer
{
  public override ImmutableArray<
    DiagnosticDescriptor> SupportedDiagnostics =>
    [
      RuleIds.AppLifetimeUse, 
      RuleIds.EnvExitsUse
    ];

  public override void Initialize(AnalysisContext context)
  {
    context.EnableConcurrentExecution();
    context.ConfigureGeneratedCodeAnalysis(
      GeneratedCodeAnalysisFlags.None);

    context.RegisterCompilationStartAction(StartAnalysis);
  }

  private static void StartAnalysis(
    CompilationStartAnalysisContext compilationContext)
  {
    var envType = compilationContext
      .Compilation
      .GetTypeByMetadataName("System.Environment");

    if (envType is null)
    {
      return;
    }

    compilationContext.RegisterSyntaxNodeAction(
      x => AnalyzeEnvironmentExitInvocation(x, envType),
      SyntaxKind.InvocationExpression);

    compilationContext.RegisterSyntaxNodeAction(
      x => AnalyzeEnvironmentExitCodeMemberAccess(x, envType),
      SyntaxKind.SimpleMemberAccessExpression);

    compilationContext.RegisterSyntaxNodeAction(
      x => AnalyzeEnvironmentExitCodeIdentifier(x, envType),
      SyntaxKind.IdentifierName);
  }

  // --------------------------------------------------------------------
  // EnvExit: Environment.Exit(..) and Exit(..) with `using static System.Environment`
  // --------------------------------------------------------------------
  private static void AnalyzeEnvironmentExitInvocation(
    SyntaxNodeAnalysisContext context,
    INamedTypeSymbol envType)
  {
    var invocation = (InvocationExpressionSyntax)context.Node;

    if (invocation.IsInNameOfOrTypeOf())
    {
      return;
    }

    var symbolInfo = context.SemanticModel.GetSymbolInfo(
      invocation,
      context.CancellationToken);

    var method = symbolInfo.Symbol as IMethodSymbol
      ?? symbolInfo
        .CandidateSymbols
        .OfType<IMethodSymbol>()
        .FirstOrDefault();

    if (method is null ||
      !string.Equals(method.Name, "Exit", StringComparison.Ordinal))
    {
      return;
    }

    if (!SymbolEqualityComparer.Default.Equals(
      method.ContainingType,
      envType))
    {
      return;
    }

    var diagnostic = Diagnostic.Create(
      RuleIds.EnvExitsUse,
      invocation.GetLocation());

    context.ReportDiagnostic(diagnostic);
  }

  // --------------------------------------------------------------------
  // EnvExit: Alias.ExitCode and ExitCode with `using Alias = System.Environment`
  // --------------------------------------------------------------------
  private static void AnalyzeEnvironmentExitCodeMemberAccess(
  SyntaxNodeAnalysisContext context,
  INamedTypeSymbol envType)
  {
    var memberAccess = (MemberAccessExpressionSyntax)context.Node;

    if (memberAccess.IsInNameOfOrTypeOf())
    {
      return;
    }

    // Fast text filter
    if (memberAccess.Name.Identifier.Text != "ExitCode")
    {
      return;
    }

    var symbolInfo = context.SemanticModel.GetSymbolInfo(
      memberAccess,
      context.CancellationToken);

    var property = symbolInfo.Symbol as IPropertySymbol
      ?? symbolInfo
        .CandidateSymbols
        .OfType<IPropertySymbol>()
        .FirstOrDefault();

    if (property is null ||
        !SymbolEqualityComparer.Default.Equals(
          property.ContainingType,
          envType))
    {
      return;
    }

    var diagnostic = Diagnostic.Create(
      RuleIds.EnvExitsUse,
      memberAccess.GetLocation());

    context.ReportDiagnostic(diagnostic);
  }


  // --------------------------------------------------------------------
  // EnvExit: Environment.ExitCode and ExitCode with `using static System.Environment`
  // --------------------------------------------------------------------
  private static void AnalyzeEnvironmentExitCodeIdentifier(
    SyntaxNodeAnalysisContext context,
    INamedTypeSymbol envType)
  {
    var identifier = (IdentifierNameSyntax)context.Node;

    // Ignore if part of a member access (handled AnalyzeEnvironmentExitCodeMemberAccess)
    if (identifier.Parent is MemberAccessExpressionSyntax ma &&
      ma.Name == identifier)
    {
      return;
    }

    // Fast text filter so we don't ask the semantic model for everything
    if (!string.Equals(identifier.Identifier.Text, "ExitCode", StringComparison.Ordinal))
    {
      return;
    }

    if (identifier.IsInNameOfOrTypeOf())
    {
      return;
    }

    var symbolInfo = context.SemanticModel.GetSymbolInfo(
      identifier,
      context.CancellationToken);

    var symbol = symbolInfo.Symbol
      ?? symbolInfo
      .CandidateSymbols
      .FirstOrDefault();

    // We only care about the ExitCode property on System.Environment
    var property = symbol as IPropertySymbol
      ?? symbolInfo
        .CandidateSymbols
        .OfType<IPropertySymbol>()
        .FirstOrDefault();

    if (property is null ||
      !SymbolEqualityComparer.Default.Equals(
      property.ContainingType,
      envType))
    {
      return;
    }

    var diagnostic = Diagnostic.Create(
      RuleIds.EnvExitsUse,
      identifier.GetLocation());

    context.ReportDiagnostic(diagnostic);
  }
}
