namespace ToolCart.UsageAnalyzer.Helpers;

internal static class SyntaxNodeExtensions
{
  public static bool IsInNameOfOrTypeOf(this SyntaxNode node)
  {
    var inNameOf = node
      .AncestorsAndSelf()
      .OfType<InvocationExpressionSyntax>()
      .Any(inv =>
        inv.Expression is IdentifierNameSyntax id &&
        id.Identifier.Text == "nameof");

    if (inNameOf)
    {
      return true;
    }

    return node
      .AncestorsAndSelf()
      .OfType<TypeOfExpressionSyntax>()
      .Any();
  }
}
