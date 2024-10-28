using System.Text.Json;
using Authentication.Common.Abstractions;
using Authentication.Common.Exceptions;
using Authentication.Models;
using Slot.Common.Abstractions;
using Tiny.Common;
using Tiny.Models;
using Tiny.Utils;
using Wallet.Common.Abstracts;
using Wallet.Common.Exceptions;

namespace Tiny;

public class Application(
    CommandParser commandParser,
    IAuthenticationService authenticationService,
    IWalletService walletService,
    ISlotGame game)
{
    private bool _running;

    // Stores the currently logged-in user
    private User? _user;

    private bool IsUserLogin => _user != null;

    /// <summary>
    /// Starts the application.
    /// </summary>
    public async Task Run()
    {
        Console.WriteLine(Constants.WelcomeMessage);

        _running = true;
        while (_running)
        {
            Console.WriteLine(Constants.CallToActionMessage);

            var input = Console.ReadLine();
            var commandModel = commandParser.Parse(input);
            await Process(commandModel);
        }
    }

    /// <summary>
    /// Processes the parsed command and executes the corresponding action.
    /// </summary>
    /// <param name="commandModel">The CommandModel containing the command and its arguments.</param>
    private async Task Process(CommandModel commandModel)
    {
        switch (commandModel.Command)
        {
            case Constants.SignupCommand:
                await Signup(commandModel.Arguments);
                break;

            case Constants.SigninCommand:
                Signin(commandModel.Arguments);
                break;

            case Constants.DepositCommand:
                Deposit(commandModel.Arguments);
                break;

            case Constants.WithdrawCommand:
                Withdraw(commandModel.Arguments);
                break;

            case Constants.BetCommand:
                Play(commandModel.Arguments);
                break;

            case Constants.ExitCommand:
                Exit();
                break;

            case Constants.HelpCommand:
                Help();
                break;

            default:
                InvalidCommand(commandModel.Command);
                break;
        }
    }

    #region Commands Methods

    /// <summary>
    /// Handles the play command, allowing the user to place a bet.
    /// </summary>
    private void Play(string[] arguments)
    {
        if (!IsUserLogin)
        {
            Console.WriteLine(Constants.NotLoggedUserMessage);
            return;
        }

        if (arguments.Length == 1 && decimal.TryParse(arguments[0], out var betAmount))
            try
            {
                if (!HasAmountAvailable(betAmount))
                {
                    Console.WriteLine(Constants.InsufficientBalancesMessage);
                    return;
                }

                _ = walletService.UpdateWallet(_user!.Id, -betAmount);
                var gameResult = game.CalculateWinAmount(betAmount);

                if (gameResult.IsWining)
                {
                    var response = walletService.UpdateWallet(_user!.Id, gameResult.WinAmount);
                    Console.WriteLine(Constants.WinMessage, gameResult.WinAmount, response.CurrentBalance);
                }
                else
                {
                    var currentBalance = walletService.GetWalletBalance(_user!.Id);
                    Console.WriteLine(Constants.LostMessage, currentBalance);
                }
            }
            catch (WalletNotAvailableException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(Constants.GameGeneralErrorMessage, e.Message);
            }
        else
        {
            Console.WriteLine(Constants.MissingAmountMessage);
        }
    }

    /// <summary>
    /// Handles the withdrawal command for the user.
    /// </summary>
    private void Withdraw(string[] arguments)
    {
        if (!IsUserLogin)
        {
            Console.WriteLine(Constants.NotLoggedUserMessage);
            return;
        }

        if (arguments.Length == 1 && decimal.TryParse(arguments[0], out var withdrawAmount))
            try
            {
                var response = walletService.Withdraw(_user!.Id, withdrawAmount);
                if (response.IsSuccessful)
                {
                    Console.WriteLine(Constants.WithdrawalMessage, withdrawAmount, response.CurrentBalance);
                }
                else
                {
                    Console.WriteLine(response.ErrorMessage);
                }
            }
            catch (WalletNotAvailableException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InsufficientValueException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(Constants.WalletGeneralErrorMessage, e.Message);
            }
        else
        {
            Console.WriteLine(Constants.MissingAmountMessage);
        }
    }

    /// <summary>
    /// Handles the deposit command for the user.
    /// </summary>
    private void Deposit(string[] arguments)
    {
        if (!IsUserLogin)
        {
            Console.WriteLine(Constants.NotLoggedUserMessage);
            return;
        }

        if (arguments.Length == 1 && decimal.TryParse(arguments[0], out var depositAmount))
            try
            {
                var response = walletService.Deposit(_user!.Id, depositAmount);
                if (response.IsSuccessful)
                {
                    Console.WriteLine(Constants.DepositMessage, depositAmount, response.CurrentBalance);
                }
                else
                {
                    Console.WriteLine(response.ErrorMessage);
                }
            }
            catch (WalletNotAvailableException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InsufficientValueException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(Constants.WalletGeneralErrorMessage, e.Message);
            }
        else
        {
            Console.WriteLine(Constants.MissingAmountMessage);
        }
    }

    /// <summary>
    /// Handles the sign-in command for the user.
    /// </summary>
    private void Signin(string[] arguments)
    {
        if (arguments.Length == 2)
            try
            {
                var username = arguments[0];
                var password = arguments[1];
                _user = authenticationService.Login(username, password);
                walletService.CreateWallet(_user.Id);

                Console.WriteLine(Constants.WelcomeUserMessage);
            }
            catch (NotRegisteredUserException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InvalidPasswordException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(Constants.AuthenticationGeneralErrorMessage, e.Message);
            }
        else
        {
            Console.WriteLine(Constants.MissingUsernameOrPasswordMessage);
        }
    }

    /// <summary>
    /// Handles the sign-up command for the user.
    /// </summary>
    private async Task Signup(string[] arguments)
    {
        if (arguments.Length == 2)
            try
            {
                var username = arguments[0];
                var password = arguments[1];
                _user = await authenticationService.RegisterAsync(username, password);
                walletService.CreateWallet(_user.Id);

                Console.WriteLine(Constants.WelcomeNewUserMessage, _user.Name);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (UserNameDuplicationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(Constants.AuthenticationGeneralErrorMessage, e.Message);
            }
        else
        {
            Console.WriteLine(Constants.MissingUsernameOrPasswordMessage);
        }
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    private void Exit()
    {
        Console.WriteLine(Constants.ExitMessage);
        _running = false;
    }

    /// <summary>
    /// Displays help information for the available commands.
    /// </summary>
    private void Help()
    {
        Console.WriteLine(Constants.CommandDescriptions);
    }

    /// <summary>
    /// Handles invalid commands and displays an error message.
    /// </summary>
    private void InvalidCommand(string command)
    {
        Console.WriteLine(Constants.InvalidCommandMessage, command);
    }

    #endregion

    private bool HasAmountAvailable(decimal betAmount)
    {
        var currentBalance = walletService.GetWalletBalance(_user!.Id);
        return currentBalance - betAmount >= 0m;
    }
}