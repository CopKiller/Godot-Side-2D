namespace Side2D.Cryptography;

using BCrypt.Net;

public static class PasswordHelper
{
    // Hash uma senha com um salt gerado
    public static string HashPassword(string password)
    {
        return BCrypt.HashPassword(password);
    }

    // Verifica se a senha fornecida corresponde ao hash
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Verify(password, hashedPassword);
    }
}
