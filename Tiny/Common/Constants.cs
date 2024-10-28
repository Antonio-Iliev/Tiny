namespace Tiny.Common;

internal static class Constants
{
    public const string DataPath = "data.json";

    #region Messages

    internal const string WelcomeMessage = "Welcome and have fun!";
    internal const string WelcomeUserMessage = "Hi, {0}. Nice to see you again.";
    internal const string WelcomeNewUserMessage = "Hello, {0}. Enjoy the game!";
    internal const string CallToActionMessage = "\nPlease, submit action:";
    internal const string ExitMessage = "Thank you for playing! Hope to see you again soon!";
    internal const string WinMessage = "Congrats - you won ${0:F2}! Your current balance is: ${1:F2}";
    internal const string LostMessage = "No luck this time! Your current balance is: ${0:F2}";

    internal const string WithdrawalMessage =
        "Your withdrawal of ${0} was successful. Your current balance is: ${1:F2}";

    internal const string DepositMessage = "Your deposit of ${0} was successful. Your current balance is: ${1:F2}";

    // Error Messages
    internal const string NotLoggedUserMessage = "You must login first.";

    internal const string InvalidCommandMessage =
        "The command '{0}' is not valid. Type 'help' to see available commands.";

    internal const string InsufficientBalancesMessage = "Insufficient balance. Deposit funds to continue playing.";
    internal const string MissingAmountMessage = "An amount is required to complete this action.";
    internal const string MissingUsernameOrPasswordMessage = "Both username and password are required to proceed.";
    internal const string AuthenticationGeneralErrorMessage = "An unexpected error occurred during authentication: {0}";
    internal const string WalletGeneralErrorMessage = "Oops! {0}";
    internal const string GameGeneralErrorMessage = "Game crashed! Please try again later. {0}";

    #endregion

    #region Commands

    internal const string SignupCommand = "signup";
    internal const string SigninCommand = "signin";
    internal const string DepositCommand = "deposit";
    internal const string WithdrawCommand = "withdraw";
    internal const string BetCommand = "bet";
    internal const string ExitCommand = "exit";
    internal const string HelpCommand = "help";

    #endregion

    internal const string CommandDescriptions = """

                                                -------------------
                                                Available Commands:

                                                1. **signup** [username] [password]
                                                   Registers a new user with the specified username and password. 
                                                   Example: signup myusername MyPassword123!

                                                2. **signin** [username] [password]
                                                   Logs in an existing user using the specified username and password.
                                                   Example: signup myusername MyPassword123!

                                                3. **deposit** [amount]
                                                   Adds the specified amount of money to the wallet.
                                                   You must be logged in to perform this action. Make sure to specify a valid amount.
                                                   Example: deposit 50.00

                                                4. **withdraw** [amount]
                                                   Withdraw the specified amount of money from the wallet.
                                                   This command requires the user to be logged in and should not exceed the available balance.
                                                   Example: withdraw 20.00

                                                5. **bet** [amount]
                                                   Places a bet of the specified amount in the slot game.
                                                   The user must be logged in and have sufficient balance to place the bet.
                                                   Example: bet 10.00
                                                   
                                                6. **exit**
                                                   Terminates the applicatio. Use this command to safely log out and close the application.
                                                   Example: exit
                                                   -------------------
                                                   
                                                """;
}