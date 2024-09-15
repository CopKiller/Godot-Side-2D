using Side2D.Cryptography;
using Xunit.Abstractions;

namespace Side2D.Tests;

public class CryptographyTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CryptographyTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test1()
    {
        string password;
        string hashedPassword;
        
        // Test password hashing and verification
        password = "password";
        hashedPassword = PasswordHelper.HashPassword(password);
        
        Assert.True(PasswordHelper.VerifyPassword(password, hashedPassword));
        Assert.False(PasswordHelper.VerifyPassword("wrongpassword", hashedPassword));
        
        // Test password length
        password = "PasswordLengthTest1234567890234234r5rfdsvgeswgewrsfsferwfwerfa";
        hashedPassword = PasswordHelper.HashPassword(password);
        _testOutputHelper.WriteLine(hashedPassword.Length.ToString());
        Assert.True(hashedPassword.Length == 60);
    }
}