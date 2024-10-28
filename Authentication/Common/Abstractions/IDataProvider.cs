using System.Text.Json;

namespace Authentication.Common.Abstractions;

public interface IDataProvider
{
    /// <summary>
    /// Loads data from a specified JSON file.
    /// </summary>
    /// <typeparam name="T">The type of the data object to be deserialized.</typeparam>
    /// <param name="filePath">The path to the JSON file.</param>
    /// <returns>The deserialized data object of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if the file path is null or empty.</exception>
    /// <exception cref="JsonException">Thrown if deserialization fails.</exception>
    Task<T> LoadData<T>(string filePath);

    /// <summary>
    /// Saves data to a specified JSON file.
    /// </summary>
    /// <typeparam name="T">The type of the data object to be serialized.</typeparam>
    /// <param name="filePath">The path to the file where data will be saved.</param>
    /// <param name="data">The data object to be serialized and saved.</param>
    /// <exception cref="ArgumentException">Thrown if the file path is null or empty.</exception>
    Task SaveData<T>(string filePath, T data);
}