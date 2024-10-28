using Wallet.Common.Exceptions;
using Wallet.Models;

namespace Wallet.Common.Abstracts;

public interface IWalletService
{
    /// <summary>
    /// Creates a new wallet for a user with a specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    void CreateWallet(Guid userId);

    /// <summary>
    /// Deposits a specified amount into the user's wallet.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="amount">The amount to deposit.</param>
    /// <returns>A <see cref="WalletResponseModel"/> containing the updated balance after the deposit.</returns>
    /// <exception cref="InsufficientValueException">Thrown if the deposit amount is zero or negative.</exception>
    WalletResponseModel Deposit(Guid userId, decimal amount);

    /// <summary>
    /// Withdraws a specified amount from the user's wallet, if sufficient balance exists.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="amount">The amount to withdraw.</param>
    /// <returns>A <see cref="WalletResponseModel"/> containing the updated balance or an error message if funds are insufficient.</returns>
    /// <exception cref="InsufficientValueException">Thrown if the withdrawal amount is zero or negative.</exception>
    WalletResponseModel Withdraw(Guid userId, decimal amount);

    /// <summary>
    /// Updates the user's wallet by adding or deducting a specified amount.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="amount">The amount to adjust (positive for addition, negative for deduction).</param>
    /// <returns>A <see cref="WalletResponseModel"/> containing the updated balance after adjustment.</returns>
    WalletResponseModel UpdateWallet(Guid userId, decimal amount);

    /// <summary>
    /// Retrieves the current balance of a user's wallet.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The current wallet balance.</returns>
    decimal GetWalletBalance(Guid userId);
}