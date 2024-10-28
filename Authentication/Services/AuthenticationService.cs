using System.Text.RegularExpressions;
using Authentication.Common;
using Authentication.Common.Abstractions;
using Authentication.Common.Exceptions;
using Authentication.Models;

namespace Authentication.Services;

public class AuthenticationService(IDataProvider dataProvider, IHasher hasher) : IAuthenticationService
{
    private string? _dataPath;
    private RegistrationData _data = new();

    public async Task InitializeAsync(string filePath)
    {
        _dataPath = filePath;
        _data = await dataProvider.LoadData<RegistrationData>(_dataPath);
    }

    public async Task<User> RegisterAsync(string? username, string? password)
    {
        // Validate input credentials to ensure they meet system requirements.
        if (!Validate(username, password))
        {
            throw new ArgumentException(
                $"Registration input is incorrect.Username input '{username}', password input '{password}'");
        }

        // Ensure the username is unique by checking existing data.
        if (GetUserData(username!) != null)
        {
            throw new UserNameDuplicationException($"The user name '{username}' already exists.");
        }

        var user = new User(Guid.NewGuid(), username!);
        var passwordHash = hasher.Hash(password!);

        // Add and persists the new registered user.
        _data.UsersMap.Add(new DataModel(user.Id, user.Name, passwordHash));
        await dataProvider.SaveData(_dataPath!, _data);

        return user;
    }


    public User Login(string? username, string? password)
    {
        // Validate input credentials to ensure they meet system requirements.
        if (!Validate(username, password))
        {
            throw new ArgumentException(
                $"Registration input is incorrect.Username input '{username}', password input '{password}'");
        }

        // Get the user data.
        var dataModel = GetUserData(username!);
        if (dataModel == null)
        {
            throw new NotRegisteredUserException($"The user '{username}' is not registered.");
        }

        // Verify the provided password.
        if (dataModel.PasswordHash != hasher.Hash(password!))
        {
            throw new InvalidPasswordException("Wrong username or password.");
        }

        return new User(dataModel.Id, dataModel.Username);
    }

    private bool Validate(string? username, string? password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        var rgxUsername = new Regex(Constants.UsernameValidationCriteria);
        var rexPassword = new Regex(Constants.PasswordValidationCriteria);

        return rgxUsername.IsMatch(username) && rexPassword.IsMatch(password);
    }

    private DataModel? GetUserData(string username)
    {
        return _data.UsersMap.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }
}