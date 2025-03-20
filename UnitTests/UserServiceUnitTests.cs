using Xunit;
using BCrypt.Net;

public class UserServiceUnitTests
{
    // Ensures hashing the same password produces different hashes due to salting
    [Fact]
    public void Hash_DifferentResultsForSameInput()
    {
        string password = "mypassword";
        string hash1 = BCrypt.Net.BCrypt.HashPassword(password);
        string hash2 = BCrypt.Net.BCrypt.HashPassword(password);
        Assert.NotEqual(hash1, hash2);
    }

    // Ensures correct password matches its hash
    [Fact]
    public void Verify_CorrectPassword_ReturnsTrue()
    {
        string password = "mypassword";
        string hash = BCrypt.Net.BCrypt.HashPassword(password);
        bool isValid = BCrypt.Net.BCrypt.Verify(password, hash);
        Assert.True(isValid);
    }

    // Ensures incorrect password does not match the stored hash
    [Fact]
    public void Verify_IncorrectPassword_ReturnsFalse()
    {
        string correctPassword = "mypassword";
        string incorrectPassword = "wrongpassword";
        string hash = BCrypt.Net.BCrypt.HashPassword(correctPassword);
        bool isValid = BCrypt.Net.BCrypt.Verify(incorrectPassword, hash);
        Assert.False(isValid);
    }
}
