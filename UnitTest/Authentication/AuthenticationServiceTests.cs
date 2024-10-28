using Authentication.Common.Abstractions;
using Authentication.Common.Exceptions;
using Authentication.Models;
using Authentication.Services;
using Moq;

namespace UnitTest.Authentication;

[TestFixture]
public class AuthenticationServiceTests
{
    private const string Username = "ValidUser";
    private const string DuplicateUser = "DuplicateUser";
    private const string ExistingUser = "ExistingUser";
    private const string Password = "ValidPassword123!";
    private const string PasswordHash = "hashedPassword";

    private AuthenticationService _authService;
    private Mock<IDataProvider> _dataProviderMock;
    private Mock<IHasher> _hasherMock;

    private string _testFilePath;

    [SetUp]
    public async Task SetUp()
    {
        // Set the file path.
        _testFilePath = Path.Combine(Path.GetTempPath(), "test-data.json");

        // Create and setup the DataProvider.
        _dataProviderMock = new Mock<IDataProvider>();
        _dataProviderMock
            .Setup(dp => dp.LoadData<RegistrationData>(It.IsAny<string>()))
            .ReturnsAsync(new RegistrationData
                {
                    UsersMap =
                    [
                        new DataModel(Guid.NewGuid(), DuplicateUser, PasswordHash),
                        new DataModel(Guid.NewGuid(), ExistingUser, PasswordHash)
                    ]
                }
            );

        // Create and setup the Hasher.
        _hasherMock = new Mock<IHasher>();
        _hasherMock.Setup(h => h.Hash(Password)).Returns(PasswordHash);

        // Create and setup the AuthenticationService.
        _authService = new AuthenticationService(_dataProviderMock.Object, _hasherMock.Object);
        await _authService.InitializeAsync(_testFilePath);
    }

    [Test]
    public async Task RegisterAsync_WithValidCredentials_ShouldAddUser()
    {
        // Arrange
        const string newUsername = "NewUser";

        // Act
        var user = await _authService.RegisterAsync(newUsername, Password);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(user.Name, Is.EqualTo(newUsername));
            _dataProviderMock.Verify(dp => dp.SaveData(_testFilePath, It.IsAny<RegistrationData>()), Times.Once);
        });
    }

    [Test]
    public void RegisterAsync_WithInvalidCredentials_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync(null, null));
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync(Username, null));
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync(null, Password));
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync("", ""));
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync(Username, ""));
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync("", Password));
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync("1", "1"));
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync(Username, "1"));
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync("1", Password));
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync("invalid#User", "12"));
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync("invalid#User", Password));
            Assert.ThrowsAsync<ArgumentException>(() => _authService.RegisterAsync(Username, "12"));
        });
    }

    [Test]
    public void RegisterAsync_WithDuplicateUsername_ShouldThrowUserNameDuplicationException()
    {
        // Arrange & Act & Assert
        Assert.ThrowsAsync<UserNameDuplicationException>(() => _authService.RegisterAsync(DuplicateUser, Password));
    }

    [Test]
    public void Login_WithValidCredentials_ShouldReturnUser()
    {
        // Arrange & Act
        var user = _authService.Login(ExistingUser, Password);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(user.Name, Is.EqualTo(ExistingUser));
        });
    }

    [Test]
    public void Login_WithInvalidCredentials_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Multiple(() =>
        {
            Assert.Throws<ArgumentException>(() => _authService.Login(null, null));
            Assert.Throws<ArgumentException>(() => _authService.Login(Username, null));
            Assert.Throws<ArgumentException>(() => _authService.Login(null, Password));
            Assert.Throws<ArgumentException>(() => _authService.Login("", ""));
            Assert.Throws<ArgumentException>(() => _authService.Login(Username, ""));
            Assert.Throws<ArgumentException>(() => _authService.Login("", Password));
            Assert.Throws<ArgumentException>(() => _authService.Login("1", "1"));
            Assert.Throws<ArgumentException>(() => _authService.Login(Username, "1"));
            Assert.Throws<ArgumentException>(() => _authService.Login("1", Password));
            Assert.Throws<ArgumentException>(() => _authService.Login("invalid#User", "12"));
            Assert.Throws<ArgumentException>(() => _authService.Login("invalid#User", Password));
            Assert.Throws<ArgumentException>(() => _authService.Login(Username, "12"));
        });
    }

    [Test]
    public void Login_WithNonExistentUser_ShouldThrowNotRegisteredUserException()
    {
        // Arrange
        const string nonExistentUser = "nonExistentUser";

        // Act & Assert
        Assert.Throws<NotRegisteredUserException>(() => _authService.Login(nonExistentUser, Password));
    }

    [Test]
    public void Login_WithIncorrectPassword_ShouldThrowInvalidPasswordException()
    {
        // Arrange
        const string incorrectPassword = "WrongPassword456!";

        // Act & Assert
        Assert.Throws<InvalidPasswordException>(() => _authService.Login(ExistingUser, incorrectPassword));
    }
}