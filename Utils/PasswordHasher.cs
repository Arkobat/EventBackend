using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Event.Utils;

public interface IPasswordHasher
{
    public string HashPassword(string password, string salt);
    public string GenerateSalt();
}

internal class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password, string salt)
    {
        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.UTF8.GetBytes(salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100_000,
            numBytesRequested: 256 / 8));
        return hashed;
    }

    public string GenerateSalt()
    {
        var salt = RandomNumberGenerator.GetBytes(128 / 8);
        return Encoding.UTF8.GetString(salt);
    }
}