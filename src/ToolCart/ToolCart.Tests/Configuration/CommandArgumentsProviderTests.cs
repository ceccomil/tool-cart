namespace ToolCart.Tests.Configuration;

[Collection("CommandArgumentsProvider Tests")]
public class CommandArgumentsProviderTests
{
  private static ConfigurationManager GetConfigurationManager(
    string[]? args)
  {
    IConfigurationBuilder confBuilder = new ConfigurationManager();
    confBuilder.Add(new CommandArgumentsProvider(
      args));

    return (confBuilder.Build() as ConfigurationManager)!;
  }

  [Fact]
  public void CommandArgumentsProvider_when_no_arguments()
  {
    // Arrange
    var conf = GetConfigurationManager(null);

    // Act
    var source = conf
      .Sources
      .Single(x => x is CommandArgumentsProvider)
      as CommandArgumentsProvider;

    // Assert
    source!
      .IsEmpty
      .Should()
      .BeTrue();
  }

  [Fact]
  public void CommandArgumentsProvider_when_args_are_stored()
  {
    // Arrange
    var args = new string[]
    {
      "--full-Name", "John",
      "--age", "30",
      "--city_of_birth", "New York",
      "--isActive"
    };

    var conf = GetConfigurationManager(args);

    // Act
    var person = $"{conf["Full-Name"]}, " +
      $"{conf["Age"]}, {conf["City_of_birth"]}" +
      $" - is active: {Convert.ToBoolean(conf["IsActive"])}";

    // Assert
    person
      .Should()
      .Be("John, 30, New York - is active: True");
  }

  [Fact]
  public void CommandArgumentsProvider_when_duplicate_args()
  {
    // Arrange
    var args = new string[]
    {
      "--NamE", "John",
      "--name", "Doe"
    };

    // Act
    Action act = () => GetConfigurationManager(args);

    // Assert
    act
      .Should()
      .Throw<InvalidOperationException>()
      .WithMessage("Duplicate command line argument: `--name`.");
  }

  [Theory]
  [InlineData("name", "John")]
  [InlineData("-simpleFlag", null)]
  [InlineData("--a", "A single char is not accepted")]
  public void CommandArgumentsProvider_when_invalid_args(
    string arg,
    string? value)
  {
    // Arrange
    List<string> args = [arg];

    if (value is not null)
    {
      args.Add(value);
    }

    // Act
    Action act = () => GetConfigurationManager(
      [.. args]);

    // Assert
    act
      .Should()
      .Throw<InvalidOperationException>()
      .WithMessage(
        $"Invalid command line argument: `{arg}`" +
        ". Every argument must start with `--` " +
        "followed by at least two characters.");
  }

  [Fact]
  public void CommandArgumentsProvider_when_invalid_chars()
  {
    // Arrange
    var rng = new Random();
    var forbidden = '-';

    while (forbidden == '-' || forbidden == '_')
    {
      forbidden = (char)rng
        .Next(char.MinValue, char.MaxValue);
    }

    var arg = $"--invalid{forbidden}Chars";

    // Act
    Action act = () => GetConfigurationManager([arg]);

    // Assert
    act
      .Should()
      .Throw<InvalidOperationException>()
      .WithMessage(
        $"Invalid character `{forbidden}` " +
        $"in argument `{arg}`. Only letters, " +
        "numbers, hyphens, and underscores " +
        "are allowed.");
  }
}
