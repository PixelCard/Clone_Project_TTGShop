using System;
using System.Security.Cryptography;
using System.Text;

public static class HashingHelper
{
    /// <summary>
    /// Hashes a password using SHA-512 with an optional salt.
    /// </summary>
    public static string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        string passwordWithSalt = password;
        byte[] passwordBytes = Encoding.UTF8.GetBytes(passwordWithSalt);

        using (SHA512 sha512 = SHA512.Create())
        {
            byte[] hashBytes = sha512.ComputeHash(passwordBytes);
            StringBuilder hashString = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                hashString.Append(b.ToString("x2"));
            }

            return hashString.ToString();
        }
    }

    /// <summary>
    /// Compares a plain-text password with a stored hash using the provided salt.
    /// </summary>
    /// <param name="password">The plain-text password to compare.</param>
    /// <param name="storedHash">The stored hash to compare against.</param>
    /// <param name="salt">The salt used for hashing the stored password.</param>
    /// <returns>True if the password matches the stored hash; otherwise, false.</returns>
    public static bool VerifyPassword(string password, string storedHash)
    {
        // Hash the input password with the same salt
        string hashedPassword = HashPassword(password);

        // Compare the hashes
        return hashedPassword == storedHash;
    }
}
