namespace ToolCart.ConsoleHelpers;

internal static partial class StringExtensions
{
  private static readonly string[] knownTags =
  [
    "_i_", "_ai_", "_w_", "_aw_",
    "_e_", "_ae_", "_q_", "_aq_",
    "_u_", "_au_", "_d_"
  ];

  [GeneratedRegex("_[aiweqdu]{1,2}_")]
  private static partial Regex TagRgx();

  private static List<Match> ValidateTags(
    MatchCollection matches)
  {
    var tags = new List<Match>();

    foreach (var match in matches.Cast<Match>())
    {
      if (!knownTags.Contains(match.Value))
      {
        throw new NotSupportedException(
          $"Invalid tag in the tagged " +
          $"text! `{match.Value}`");
      }

      tags.Add(match);
    }

    return tags;
  }

  private static Theme GetFromTag(
    this string tag,
    Theme defaultTheme) => tag switch
    {
      "_i_" => ExtendedConsole.Info,
      "_ai_" => ExtendedConsole.AlertInfo,
      "_w_" => ExtendedConsole.Warning,
      "_aw_" => ExtendedConsole.AlertWarning,
      "_e_" => ExtendedConsole.Error,
      "_ae_" => ExtendedConsole.AlertError,
      "_q_" => ExtendedConsole.Question,
      "_aq_" => ExtendedConsole.AlertQuestion,
      "_u_" => ExtendedConsole.UserInput,
      "_au_" => ExtendedConsole.AlertUserInput,
      _ => defaultTheme
    };

  private static List<(StringBuilder Text, Theme Theme)> GetThemed(
    this List<Match> matches,
    StringBuilder source,
    Theme noTheme)
  {
    var themes = new List<(StringBuilder, Theme)>();

    var currentTheme = noTheme;
    var currentText = new StringBuilder();
    var cursor = 0;

    for (var i = 0; i < matches.Count; i++)
    {
      currentText.Append(source.ToString(
        cursor, matches[i].Index - cursor));

      if (currentText.Length > 0)
      {
        themes.Add((currentText, currentTheme));
      }

      currentText = new StringBuilder();
      currentTheme = matches[i]
        .Value
        .GetFromTag(noTheme);

      cursor = matches[i].Index
        + matches[i].Length;
    }

    currentText.Append(source.ToString(
      cursor, source.Length - cursor));

    if (currentText.Length > 0)
    {
      themes.Add((currentText, currentTheme));
    }

    return themes;
  }

  public static List<(StringBuilder Text, Theme Theme)> SplitIntoThemes(
    this string taggedText)
  {
    var rgx = TagRgx();

    var noTheme = new Theme(
      Console.ForegroundColor,
      Console.BackgroundColor);

    var source = new StringBuilder(
      taggedText);

    var matches = ValidateTags(
      rgx.Matches(taggedText));

    return matches.GetThemed(
      source,
      noTheme);
  }
}
