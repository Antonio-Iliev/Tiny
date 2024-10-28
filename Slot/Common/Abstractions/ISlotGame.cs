using Slot.Common.Exceptions;
using Slot.Models;

namespace Slot.Common.Abstractions;

public interface ISlotGame
{
    /// <summary>
    /// Calculates the win amount based on a given bet, with different probabilities for losing, small wins, and big wins.
    /// </summary>
    /// <param name="betAmount">The amount of the bet placed by the player.</param>
    /// <returns>A <see cref="GameResponseModel"/> indicating whether the player won and the winning amount.</returns>
    /// <exception cref="InsufficientBetAmountException">Thrown if the bet amount is zero or negative.</exception>
    GameResponseModel CalculateWinAmount(decimal betAmount);
}