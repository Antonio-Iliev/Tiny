using Slot.Common.Abstractions;
using Slot.Common.Exceptions;
using Slot.Games;
using Slot.Models;

namespace UnitTest.SlotGames;

[TestFixture]
public class BasicSlotGameTests
{
    private ISlotGame _slotGame;

    [SetUp]
    public void SetUp()
    {
        _slotGame = new BasicSlotGame();
    }

    [Test]
    public void CalculateWinAmount_WithValidBet_ShouldReturnGameResponseModel()
    {
        // Arrange
        const decimal betAmount = 10m;

        // Act
        var result = _slotGame.CalculateWinAmount(betAmount);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<GameResponseModel>());
        });
    }


    [Test]
    public void CalculateWinAmount_WithNegativeBet_ShouldThrowInsufficientBetAmountException()
    {
        // Arrange
        const decimal betAmount = -5m;

        // Act & Assert
        Assert.Throws<InsufficientBetAmountException>(() => _slotGame.CalculateWinAmount(betAmount));
    }

    [Test]
    public void CalculateWinAmount_WithZeroBet_ShouldThrowInsufficientBetAmountException()
    {
        // Arrange
        const decimal betAmount = 0m;

        // Act & Assert
        Assert.Throws<InsufficientBetAmountException>(() => _slotGame.CalculateWinAmount(betAmount));
    }
}