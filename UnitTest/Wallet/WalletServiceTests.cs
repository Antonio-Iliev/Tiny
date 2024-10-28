using Wallet.Common.Exceptions;
using Wallet.Models;
using Wallet.Services;

namespace UnitTest.Wallet;

[TestFixture]
public class WalletServiceTests
{
    private WalletService _walletService;
    private Guid _userId;

    [SetUp]
    public void SetUp()
    {
        _walletService = new WalletService();
        _userId = Guid.NewGuid();
        _walletService.CreateWallet(_userId);
    }

    [Test]
    public void CreateWallet_ShouldInitializeWalletWithZeroBalance()
    {
        // Arrange
        var newUserId = Guid.NewGuid();
        _walletService.CreateWallet(newUserId);

        // Act
        var balance = _walletService.GetWalletBalance(newUserId);

        // Assert
        Assert.That(balance, Is.EqualTo(0m));
    }

    [Test]
    public void Deposit_ShouldReturnWalletResponseModel()
    {
        // Act
        var result = _walletService.Deposit(_userId, 22m);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<WalletResponseModel>());
        });
    }

    [Test]
    public void Deposit_ShouldIncreaseBalance()
    {
        // Arrange
        var newUserId = Guid.NewGuid();
        _walletService.CreateWallet(newUserId);
        const decimal depositAmount = 1234.5678m;

        // Act
        WalletResponseModel response = _walletService.Deposit(newUserId, depositAmount);

        // Assert
        Assert.That(response.CurrentBalance, Is.EqualTo(depositAmount));
    }

    [Test]
    public void Deposit_WithNegativeAmount_ShouldThrowInsufficientValueException()
    {
        // Arrange
        const decimal depositAmount = -0.0001m;

        // Act & Assert
        Assert.Throws<InsufficientValueException>(() => _walletService.Deposit(_userId, depositAmount));
    }

    [Test]
    public void Deposit_WithZeroAmount_ShouldThrowInsufficientValueException()
    {
        // Arrange
        const decimal depositAmount = 0m;

        // Act & Assert
        Assert.Throws<InsufficientValueException>(() => _walletService.Deposit(_userId, depositAmount));
    }

    [Test]
    public void Deposit_WhenWalletNotExist_ShouldThrowWalletNotAvailableException()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<WalletNotAvailableException>(() => _walletService.Deposit(nonExistentUserId, 1m));
    }

    [Test]
    public void Withdrawal_ShouldReturnWalletResponseModel()
    {
        // Act
        var result = _walletService.Withdraw(_userId, 1m);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<WalletResponseModel>());
        });
    }

    [Test]
    public void Withdrawal_ShouldDecreaseBalance()
    {
        // Arrange
        const decimal initialBalance = 100m;
        const decimal withdrawalAmount = 40m;

        var newUserId = Guid.NewGuid();
        _walletService.CreateWallet(newUserId);
        _walletService.Deposit(newUserId, initialBalance);

        // Act
        var response = _walletService.Withdraw(newUserId, withdrawalAmount);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.CurrentBalance, Is.EqualTo(initialBalance - withdrawalAmount));
        });
    }

    [Test]
    public void Withdrawal_WithAmountExceedingBalance_ShouldReturnErrorResponse()
    {
        // Arrange
        const decimal initialBalance = 40m;
        const decimal withdrawalAmount = 41m;

        var newUserId = Guid.NewGuid();
        _walletService.CreateWallet(newUserId);
        _walletService.Deposit(newUserId, initialBalance);

        // Act
        var response = _walletService.Withdraw(newUserId, withdrawalAmount);

        // Assert
        Assert.That(response.IsSuccessful, Is.False);
    }

    [Test]
    public void Withdrawal_WithNegativeAmount_ShouldThrowInsufficientValueException()
    {
        // Arrange
        const decimal withdrawalAmount = -100.002m;

        // Act & Assert
        Assert.Throws<InsufficientValueException>(() => _walletService.Withdraw(_userId, withdrawalAmount));
    }

    [Test]
    public void Withdrawal_WithZeroAmount_ShouldThrowInsufficientValueException()
    {
        // Arrange
        const decimal withdrawalAmount = 0m;

        // Act & Assert
        Assert.Throws<InsufficientValueException>(() => _walletService.Deposit(_userId, withdrawalAmount));
    }

    [Test]
    public void Withdrawal_WhenWalletNotExist_ShouldThrowWalletNotAvailableException()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<WalletNotAvailableException>(() => _walletService.Withdraw(nonExistentUserId, 2m));
    }

    [Test]
    public void UpdateWallet_ShouldReturnWalletResponseModel()
    {
        // Act
        var result = _walletService.UpdateWallet(_userId, -3m);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<WalletResponseModel>());
        });
    }

    [Test]
    public void UpdateWallet_ShouldAdjustBalance()
    {
        // Arrange
        const decimal updateAmount = -25.5m;

        var newUserId = Guid.NewGuid();
        _walletService.CreateWallet(newUserId);

        // Act
        var response = _walletService.UpdateWallet(newUserId, updateAmount);

        // Assert
        Assert.That(response.CurrentBalance, Is.EqualTo(updateAmount));
    }

    [Test]
    public void UpdateWallet_WhenWalletNotExist_ShouldThrowWalletNotAvailableException()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<WalletNotAvailableException>(() => _walletService.UpdateWallet(nonExistentUserId, -12.5m));
    }

    [Test]
    public void GetWalletBalance_ShouldReturnCurrentBalance()
    {
        // Arrange
        const decimal initialBalance = 120m;

        var newUserId = Guid.NewGuid();
        _walletService.CreateWallet(newUserId);
        _walletService.Deposit(newUserId, initialBalance);

        // Act
        var balance = _walletService.GetWalletBalance(newUserId);

        // Assert
        Assert.That(balance, Is.EqualTo(initialBalance));
    }

    [Test]
    public void GetWalletBalance_WithNonExistentWallet_ShouldThrowWalletNotAvailableException()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<WalletNotAvailableException>(() => _walletService.GetWalletBalance(nonExistentUserId));
    }
}