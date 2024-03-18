namespace ToolCart.Tests;

[Collection("StringExtensions Tests")]
public class StringExtensionsTests
{
  [Theory]
  [InlineData(
    "_q_This is an _ai_info message!",
    "This is an info message!",
    2)]
  [InlineData(
    "This is a _aq_question with _i_info and _aw_warnings_q_!",
    "This is a question with info and warnings!",
    5)]
  [InlineData(
    "This is a _aq_question_d_ with _i_info_d_ and _aw_warnings_d_!",
    "This is a question with info and warnings!",
    7)]
  [InlineData(
    "TestDiDisposable is _au_disposed_d_!",
    "TestDiDisposable is disposed!",
    3)]
  public void SplitIntoThemes_WhenCalled_ReturnsThemes(
    string taggedText,
    string expected,
    int splits)
  {
    // Act
    var list = taggedText.SplitIntoThemes();
    var result = string.Join(
      "",
      list.Select(x => x.Text.ToString()));

    // Assert
    list
      .Count
      .Should()
      .Be(splits);

    result
      .Should()
      .Be(expected);
  }
}
