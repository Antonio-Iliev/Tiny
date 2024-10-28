namespace Wallet.Models;

public record WalletResponseModel
{
    public WalletResponseModel(decimal currentBalance)
    {
        CurrentBalance = currentBalance;
        IsSuccessful = true;
    }

    public WalletResponseModel(string errorMessage)
    {
        ErrorMessage = errorMessage;
        IsSuccessful = false;
    }

    public decimal CurrentBalance { get; }
    public bool IsSuccessful { get; }
    public string? ErrorMessage { get; }
}