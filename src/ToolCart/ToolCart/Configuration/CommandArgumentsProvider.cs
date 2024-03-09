namespace ToolCart.Configuration;

internal sealed class CommandArgumentsProvider
  : ConfigurationProvider, IConfigurationSource
{
  private const string ALLOWED_CHARS =
    "abcdefghijklmnopqrstuvwxyz" +
    "0123456789" +
    "-_";

  private readonly string[] _args;

  public bool IsEmpty => Data.Count == 0;

  public CommandArgumentsProvider(
    string[]? args = default)
  {
    args ??= [];
    _args = args;
  }

  private Dictionary<string, string?> GetArguments()
  {
    var dict = new Dictionary<string, string?>();

    for (var i = 0; i < _args.Length; i++)
    {
      var source = _args[i];
      var arg = NormalizeArgument(source);

      var value = "true";

      if (i + 1 < _args.Length &&
        !_args[i + 1].StartsWith("--"))
      {
        value = _args[i + 1];
        i++;
      }

      if (dict.Keys.Any(x => x.Equals(
        arg, StringComparison.OrdinalIgnoreCase)))
      {
        throw new InvalidOperationException(
          $"Duplicate command line argument: `{source}`.");
      }

      dict.Add(
        arg,
        value);
    }

    return dict;
  }

  private static string NormalizeArgument(string arg)
  {
    if (!arg.StartsWith("--") || arg.Length <= 3)
    {
      throw new InvalidOperationException(
        $"Invalid command line argument: `{arg}`. " +
        "Every argument must start with `--` followed" +
        " by at least two characters.");
    }

    var sb = new StringBuilder(arg);

    sb.Remove(0, 2);

    sb[0] = char.ToUpperInvariant(sb[0]);

    for (var i = 1; i < sb.Length; i++)
    {
      if (!ALLOWED_CHARS.Contains(
        sb[i],
        StringComparison.OrdinalIgnoreCase))
      {
        throw new InvalidOperationException(
          $"Invalid character `{sb[i]}` in argument " +
          $"`{arg}`. Only letters, numbers, hyphens," +
          " and underscores are allowed.");
      }
    }

    return sb.ToString();
  }

  public IConfigurationProvider Build(
    IConfigurationBuilder builder) => this;

  public override void Load()
  {
    Data = GetArguments();
  }
}
