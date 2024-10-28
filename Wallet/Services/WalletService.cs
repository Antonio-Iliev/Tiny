using Wallet.Common.Abstracts;
using Wallet.Common.Exceptions;
using Wallet.Models;

namespace Wallet.Services;

public class WalletService : IWalletService
{
    private readonly Dictionary<Guid, BalanceModel> _userWallets = new();

    public void CreateWallet(Guid userId)
    {
        // Initialize a new wallet with zero balance for the specified user.
        _userWallets[userId] = new BalanceModel();
    }

    public WalletResponseModel Deposit(Guid userId, decimal amount)
    {
        // Ensure deposit amount is positive
        ValidateAmount(amount);

        var balance = GetUserWallet(userId);
        balance.Amount += amount;

        return new WalletResponseModel(balance.Amount);
    }

    public WalletResponseModel Withdraw(Guid userId, decimal amount)
    {
        // Ensure withdrawal amount is positive
        ValidateAmount(amount);

        var balance = GetUserWallet(userId);
        if (balance.Amount - amount < 0m)
        {
            return new WalletResponseModel($"Can not withdrawal. The balance is {balance}");
        }

        balance.Amount -= amount;
        return new WalletResponseModel(balance.Amount);
    }

    public WalletResponseModel UpdateWallet(Guid userId, decimal amount)
    {
        var balance = GetUserWallet(userId);

        balance.Amount += amount;
        return new WalletResponseModel(balance.Amount);
    }

    public decimal GetWalletBalance(Guid userId)
    {
        var balance = GetUserWallet(userId);
        return balance.Amount;
    }

    private BalanceModel GetUserWallet(Guid userId)
    {
        if (!_userWallets.TryGetValue(userId, out var balance))
        {
            throw new WalletNotAvailableException($"Wallet for player id '{userId}' was not created.");
        }

        return balance;
    }

    private void ValidateAmount(decimal amount)
    {
        if (amount <= 0m)
        {
            throw new InsufficientValueException($"The amount must be a positive number. Passed value is {amount}.");
        }
    }
}