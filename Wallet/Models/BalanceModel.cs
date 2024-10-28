using System.Globalization;

namespace Wallet.Models;

public class BalanceModel
{
    public decimal Amount { get; set; }

    public override string ToString()
    {
        return Amount.ToString(CultureInfo.InvariantCulture);
    }
}