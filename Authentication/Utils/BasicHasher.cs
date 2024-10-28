using System.Security.Cryptography;
using System.Text;
using Authentication.Common.Abstractions;

namespace Authentication.Utils;

public class BasicHasher : IHasher
{
    string IHasher.Hash(string input)
    {
        // Validate input to ensure it's not empty.
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Input cannot be empty.");
        }

        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}