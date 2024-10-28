using System.Text.Json;
using Authentication.Common.Abstractions;

namespace Authentication.Utils;

public class DataProvider : IDataProvider
{
    public async Task<T> LoadData<T>(string? filePath)
    {
        // Validate that the file path is provided.
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("The file path can not be null.");
        }

        var jsonContent = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<T>(jsonContent) ??
               throw new JsonException($"Can not deserialize {jsonContent}");
    }

    public async Task SaveData<T>(string? filePath, T data)
    {
        // Validate that the file path is provided.
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("The file path can not be null.");
        }

        var jsonContent = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(filePath, jsonContent);
    }
}