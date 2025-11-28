#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Shared.Helpers;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class Tags
{
  /// <summary>
  /// Valid tags that can be used in WriteMixed text.
  /// </summary>
  public static string[] KnownTags { get; } =
  [
    "_i_", "_ai_", "_w_", "_aw_",
    "_e_", "_ae_", "_q_", "_aq_",
    "_u_", "_au_", "_d_"
  ];

  public static readonly HashSet<string> KnownTagNames =
  [
    .. KnownTags.Select(x => x.Trim('_'))
  ];
}
