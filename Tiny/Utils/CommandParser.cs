using Tiny.Common.Exceptions;
using Tiny.Models;

namespace Tiny.Utils;

public class CommandParser
{
    /// <summary>
    /// Parses a user input into a CommandModel, containing the command and any associated arguments.
    /// </summary>
    /// <param name="input">The command input string from the user.</param>
    /// <returns>A CommandModel with the parsed command and arguments.</returns>
    /// <exception cref="InvalidUserInputException">Thrown when input is null, empty.</exception>
    public CommandModel Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new InvalidUserInputException("The input command can not be empty.");

        // Split input into parts by spaces.
        var parts = input.Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        // Extract the command and converting it to lowercase for consistency.
        var command = parts[0].ToLower();

        return parts.Length == 1 ? new CommandModel(command) : new CommandModel(command, parts.Skip(1).ToArray());
    }
}