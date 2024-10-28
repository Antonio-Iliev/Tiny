using Tiny.Common.Exceptions;
using Tiny.Utils;

namespace UnitTest.Application;

[TestFixture]
public class CommandParserTests
{
    private CommandParser _commandParser;

    [SetUp]
    public void SetUp()
    {
        _commandParser = new CommandParser();
    }

    [Test]
    public void Parse_WithSingleWordCommand_ShouldReturnCommandModelWithNoArguments()
    {
        // Arrange 
        const string input = "exit";

        // Act
        var result = _commandParser.Parse(input);

        // Assert 
        Assert.Multiple(() =>
        {
            Assert.That(result.Command, Is.EqualTo(input));
            Assert.IsEmpty(result.Arguments);
        });
    }

    [Test]
    public void Parse_WithCommandAndSingleArgument_ShouldReturnCommandModelWithSingleArgument()
    {
        // Arrange 
        const string command = "deposit";
        const string value = "10";
        const string input = $"{command} {value}";

        // Act
        var result = _commandParser.Parse(input);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Command, Is.EqualTo(command));
            Assert.That(result.Arguments, Has.Length.EqualTo(1));
            Assert.That(result.Arguments[0], Is.EqualTo(value));
        });
    }

    [Test]
    public void Parse_WithCommandAndMultipleArguments_ShouldReturnCommandModelWithArguments()
    {
        // Arrange
        const string command = "register";
        var values = new[]
        {
            "User123",
            "Password123",
            "Hint",
            "MyBestPasswordHint",
        };
        var input = $"{command} {string.Join(" ", values)}";

        // Act
        var result = _commandParser.Parse(input);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Command, Is.EqualTo(command));
            Assert.That(result.Arguments, Has.One.Length.EqualTo(values.Length));
            for (var i = 0; i < values.Length; i++)
            {
                Assert.That(result.Arguments[i], Is.EqualTo(values[i]));
            }
        });
    }

    [Test]
    public void Parse_WithNullOrWhiteSpaceInput_ShouldThrowInvalidUserInputException()
    {
        // Arrange & Act & Assert
        Assert.Multiple(() =>
        {
            Assert.Throws<InvalidUserInputException>(() => _commandParser.Parse(null));
            Assert.Throws<InvalidUserInputException>(() => _commandParser.Parse(""));
            Assert.Throws<InvalidUserInputException>(() => _commandParser.Parse("        "));
        });
    }

    [Test]
    public void Parse_WithExtraWhitespace_ShouldTrimAndParseCorrectly()
    {
        // Arrange
        const string command = "bet";
        const string value = "20";
        const string input = $"   {command}      {value}   ";

        // Act
        var result = _commandParser.Parse(input);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Command, Is.EqualTo(command));
            Assert.That(result.Arguments, Has.Length.EqualTo(1));
            Assert.That(result.Arguments[0], Is.EqualTo(value));
        });
    }

    [Test]
    public void Parse_WithMixedCaseCommand_ShouldReturnCommandInLowercase()
    {
        // Arrange
        const string command = "WiThDraw";
        const string value = "HUGE_AMOUNT";
        const string input = $"{command} {value}";

        // Act
        var result = _commandParser.Parse(input);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Command, Is.EqualTo(command.ToLower()));
            Assert.That(result.Arguments, Has.Length.EqualTo(1));
            Assert.That(result.Arguments[0], Is.EqualTo(value));
        });
    }
}