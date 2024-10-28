using System.Text.Json;
using Authentication.Utils;

namespace UnitTest.Authentication;

[TestFixture]
public class DataProviderTests
{
    private DataProvider _dataProvider;
    private string _testFilePath;

    private record TestData(string Test1, string Test2);

    [SetUp]
    public void SetUp()
    {
        _dataProvider = new DataProvider();
        _testFilePath = Path.Combine(Path.GetTempPath(), "test-data.json");
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Test]
    public async Task LoadData_ShouldReturnDeserializedObject()
    {
        // Arrange
        var testData = new TestData("Persisted information", "Player level -> 123");
        var jsonString = JsonSerializer.Serialize(testData);
        await File.WriteAllTextAsync(_testFilePath, jsonString);

        // Act
        var result = await _dataProvider.LoadData<TestData>(_testFilePath);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<TestData>());
            Assert.That(result, Is.EqualTo(testData));
        });
    }

    [Test]
    public void LoadData_WithNullOrWhiteSpacePath_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ArgumentException>(() => _dataProvider.LoadData<object>(null!));
            Assert.ThrowsAsync<ArgumentException>(() => _dataProvider.LoadData<object>(""));
            Assert.ThrowsAsync<ArgumentException>(() => _dataProvider.LoadData<object>("    "));
        });
    }

    [Test]
    public void LoadData_WithInvalidPath_ShouldThrowFileNotFoundException()
    {
        // Arrange & Act & Assert
        Assert.ThrowsAsync<FileNotFoundException>(() => _dataProvider.LoadData<object>("Invalid path"));
    }

    [Test]
    public async Task LoadData_WithInvalidJson_ShouldThrowJsonException()
    {
        // Arrange
        await File.WriteAllTextAsync(_testFilePath, "Invalid Json");

        // Act & Assert
        Assert.ThrowsAsync<JsonException>(() => _dataProvider.LoadData<TestData>(_testFilePath));
    }


    [Test]
    public async Task SaveData_ShouldWriteDataToFile()
    {
        // Arrange
        var testData = new TestData("Some test data", "Another test data");

        // Act
        await _dataProvider.SaveData(_testFilePath, testData);

        // Assert
        Assert.That(File.Exists(_testFilePath), Is.True);

        var jsonString = await File.ReadAllTextAsync(_testFilePath);
        var deserializedData = JsonSerializer.Deserialize<TestData>(jsonString);

        Assert.That(deserializedData, Is.EqualTo(testData));
    }

    [Test]
    public void SaveData_WithNullOrWhiteSpacePath_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ArgumentException>(() => _dataProvider.SaveData(null!, new object()));
            Assert.ThrowsAsync<ArgumentException>(() => _dataProvider.SaveData("", new object()));
            Assert.ThrowsAsync<ArgumentException>(() => _dataProvider.SaveData("    ", new object()));
        });
    }
}