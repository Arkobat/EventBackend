using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Event.Utils;

public interface IPasswordHasher
{
    public string HashPassword(string password, string salt);
    public string GenerateSalt(int size = 16);
}

public class PasswordHasher : IPasswordHasher
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

    public string GenerateSalt(int size = 16)
    {
        var bytes = RandomNumberGenerator.GetBytes(size);
        var salt = Convert.ToBase64String(bytes);
        salt = salt.TrimEnd('=');
        return salt;
    }
}