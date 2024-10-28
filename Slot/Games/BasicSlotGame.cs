using Slot.Common.Abstractions;
using Slot.Common.Exceptions;
using Slot.Models;

namespace Slot.Games;

public class BasicSlotGame : ISlotGame
{
    private readonly Random _random = new();

    public GameResponseModel CalculateWinAmount(decimal betAmount)
    {
        // Validate bet to ensure a positive value
        ValidateAmount(betAmount);

        // Generate a random number between 0.0 and 1.0
        var probabilityFactor = _random.NextDouble();

        // 50% chance to lose (0.0 to 0.5)
        if (probabilityFactor < 0.5)
        {
            // Lose the bet
            return new GameResponseModel(false, 0m);
        }

        decimal winMultiplier;
        // 40% chance to win between x1 and x2 the bet (0.5 to 0.9)
        if (probabilityFactor < 0.9)
        {
            // Win between x1 and x2 the bet
            winMultiplier = 1m + (decimal)_random.NextDouble();
        }
        // 10% chance to win between x2 and x10 the bet (0.9 to 1.0)
        else
        {
            // Win between x2 and x10 the bet
            winMultiplier = 2m + (decimal)(_random.NextDouble() * 8);
        }

        return new GameResponseModel(true, betAmount * winMultiplier);
    }

    private void ValidateAmount(decimal amount)
    {
        if (amount <= 0m)
        {
            throw new InsufficientBetAmountException(
                $"The amount must be a positive number greater than zero. Passed value is {amount}.");
        }
    }
}