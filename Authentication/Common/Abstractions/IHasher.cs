namespace Authentication.Common.Abstractions;

public interface IHasher
{
    /// <summary>
    /// Computes a simple hash for a given input using SHA256.
    /// </summary>
    /// <param name="input">The input string to be hashed.</param>
    /// <returns>A string representing the SHA256 hash of the input.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input is null or empty.</exception>
    string Hash(string input);
}