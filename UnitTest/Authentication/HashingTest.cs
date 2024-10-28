using Authentication.Common.Abstractions;
using Authentication.Utils;

namespace UnitTest.Authentication;

[TestFixture]
public class HashingTest
{
    private IHasher _hasher;

    [SetUp]
    public void Setup()
    {
        _hasher = new BasicHasher();
    }

    [Test]
    public void Hash_ShouldGenerateValidSHA256Hash()
    {
        // Arrange & Act & Assert
        Assert.That(_hasher.Hash("password"), Is.EqualTo("XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg="));
    }

    [Test]
    public void Hash_WithNullOrWhiteSpacePath_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Multiple(() =>
        {
            Assert.Throws<ArgumentException>(() => _hasher.Hash(null!));
            Assert.Throws<ArgumentException>(() => _hasher.Hash(""));
            Assert.Throws<ArgumentException>(() => _hasher.Hash("    "));
        });
    }
}