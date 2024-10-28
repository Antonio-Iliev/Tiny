using Authentication.Common.Exceptions;
using Authentication.Models;

namespace Authentication.Common.Abstractions;

public interface IAuthenticationService
{
    /// <summary>
    /// Initializes the Authentication service by loading user registration data from a specified file path.
    /// </summary>
    /// <param name="filePath">The path to the data file where user registration information is stored.</param>
    Task InitializeAsync(string filePath);

    /// <summary>
    /// Registers a new user with a specified username and password.
    /// </summary>
    /// <param name="username">The username for the new user.</param>
    /// <param name="password">The password for the new user.</param>
    /// <returns>The created <see cref="User"/> if registration is successful.</returns>
    /// <exception cref="ArgumentException">Thrown if the username or password input is invalid.</exception>
    /// <exception cref="UserNameDuplicationException">Thrown if the username already exists.</exception>
    Task<User> RegisterAsync(string username, string password);

    /// <summary>
    /// Authenticates a user.
    /// </summary>
    /// <param name="username">The username to log in with.</param>
    /// <param name="password">The password associated with the username.</param>
    /// <returns>The <see cref="User"/> if login is successful.</returns>
    /// <exception cref="NotRegisteredUserException">Thrown if the user is not registered.</exception>
    /// <exception cref="InvalidPasswordException">Thrown if the password is incorrect.</exception>
    User Login(string username, string password);
}