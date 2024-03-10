namespace ToolCart.Tests.Configuration;

[Collection("Extensions Tests")]
public class ExtensionsTests
{
  private readonly IHostApplicationBuilder _hostBuilder = Substitute
    .For<IHostApplicationBuilder>();

  [Fact]
  public void AddArgsConfiguration_when_no_arguments()
  {
    // Arrange
    IConfigurationManager confBuilder = Substitute
      .For<IConfigurationManager>();

    _hostBuilder
      .Configuration
      .Returns(confBuilder);

    // Act
    _hostBuilder
      .AddArgsConfiguration();

    // Assert
    confBuilder
      .Received(1)
      .Add(Arg.Is<CommandArgumentsProvider>(
        x => x.IsEmpty));
  }

  [Fact]
  public void AddArgsConfiguration_when_arguments()
  {
    // Arrange
    IConfigurationManager confBuilder = Substitute
      .For<IConfigurationManager>();

    _hostBuilder
      .Configuration
      .Returns(confBuilder);

    var args = new string[]
    {
      "--name", "John",
      "--isActive"
    };

    // Act
    _hostBuilder
      .AddArgsConfiguration(args);

    // Assert
    confBuilder
      .Received(1)
      .Add(Arg.Is<CommandArgumentsProvider>(
        x => !x.IsEmpty));
  }

  [Fact]
  public void AddAppSettingsConfiguration()
  {
    // Arrange
    IConfigurationBuilder confBuilder = new ConfigurationManager();

    // Act
    confBuilder
      .AddAppSettingsConfiguration();

    var toolName = confBuilder
      .Build()["ToolName"];

    // Assert
    toolName
      .Should()
      .Be("XUnit Test Project");
  }
}
