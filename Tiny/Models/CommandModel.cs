namespace Tiny.Models;

public record CommandModel(string Command, params string[] Arguments);