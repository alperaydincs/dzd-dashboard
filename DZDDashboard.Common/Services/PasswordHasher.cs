using System.Security.Cryptography;

namespace DZDDashboard.Common.Services;

public static class PasswordHasher
{
    
    public static string Hash(string password, int iterations = 100_000, int saltSize = 16, int keySize = 32)
    {
        var salt = RandomNumberGenerator.GetBytes(saltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, keySize);
        return $"v1|{iterations}|{Convert.ToBase64String(salt)}|{Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string hashed, string password)
    {
        var parts = hashed.Split('|');
        if (parts.Length != 4 || parts[0] != "v1") return false;

        var iterations = int.Parse(parts[1]);
        var salt = Convert.FromBase64String(parts[2]);
        var expected = Convert.FromBase64String(parts[3]);

        var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
        return CryptographicOperations.FixedTimeEquals(expected, actual);
    }
}