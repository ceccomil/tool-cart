namespace ToolCart.UsageAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class WriteMixedTagsUsageAnalyzer : DiagnosticAnalyzer
{
  // Any `_xxx_` (1–3 letters), not escaped -> for "unescaped" detection.
  private static readonly Regex _tagLikeRegex =
    new(@"(?<!\\)_(?<name>[a-z]{1,3})_", RegexOptions.Compiled);

  public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
  [
    RuleIds.UnescapedTagRule
  ];

  public override void Initialize(AnalysisContext context)
  {
    context.EnableConcurrentExecution();

    context.ConfigureGeneratedCodeAnalysis(
      GeneratedCodeAnalysisFlags.None);

    context.RegisterSyntaxNodeAction(
      AnalyzeInvocation,
      SyntaxKind.InvocationExpression);
  }

  private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
  {
    var invocation = (InvocationExpressionSyntax)context.Node;

    var symbolInfo = context
      .SemanticModel
      .GetSymbolInfo(invocation, context.CancellationToken);

    var methodSymbol =
      symbolInfo.Symbol as IMethodSymbol
      ?? symbolInfo
        .CandidateSymbols
        .OfType<IMethodSymbol>()
        .FirstOrDefault();

    if (methodSymbol is null)
    {
      return;
    }

    // We only care about WriteMixed(...)
    if (!string.Equals(methodSymbol.Name, "WriteMixed", StringComparison.Ordinal))
    {
      return;
    }

    // First parameter must be string, otherwise bail
    if (methodSymbol.Parameters.Length == 0 ||
        methodSymbol.Parameters[0].Type.SpecialType != SpecialType.System_String)
    {
      return;
    }

    var args = invocation.ArgumentList.Arguments;
    if (args.Count == 0)
    {
      return;
    }

    var firstArg = args[0];

    var constValue = context.SemanticModel.GetConstantValue(
      firstArg.Expression,
      context.CancellationToken);

    if (!constValue.HasValue ||
        constValue.Value is not string text)
    {
      // Not a compile-time constant string; skip
      return;
    }

    AnalyzeWriteMixedText(
      context,
      firstArg.Expression,
      text);
  }

  private static void AnalyzeWriteMixedText(
    SyntaxNodeAnalysisContext context,
    ExpressionSyntax argumentExpression,
    string text)
  {
    var location = argumentExpression.GetLocation();

    // 1. Unescaped `_xxx_` tags that are not known → UnescapedTagRule
    foreach (Match match in _tagLikeRegex.Matches(text))
    {
      var name = match.Groups["name"].Value;

      if (KnownTagNames.Contains(name))
      {
        continue;
      }

      var props = ImmutableDictionary<string, string?>.Empty
        .Add("kind", "unescaped")
        .Add("index", match.Index.ToString(CultureInfo.InvariantCulture))
        .Add("length", match.Length.ToString(CultureInfo.InvariantCulture));

      ReportUnescapedTag(
        context,
        location, 
        props, 
        match.Value);
    }
  }

  private static void ReportUnescapedTag(
    SyntaxNodeAnalysisContext context,
    Location location,
    ImmutableDictionary<string, string?> props,
    string tag)
  {
    var diag = Diagnostic.Create(
      RuleIds.UnescapedTagRule,
      location,
      props,
      tag);

    context.ReportDiagnostic(diag);
  }
}
