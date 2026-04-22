using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace Laba10_1
{
    public static class PasswordHelper
    {
        private const int Iterations = 3;
        private const int MemorySizeKb = 65536;
        private const int Parallelism = 1;

        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Пароль не может быть пустым.");

            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = Parallelism;
                argon2.Iterations = Iterations;
                argon2.MemorySize = MemorySizeKb;

                byte[] hash = argon2.GetBytes(32);
                byte[] combined = new byte[salt.Length + hash.Length];
                Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
                Buffer.BlockCopy(hash, 0, combined, salt.Length, hash.Length);

                return Convert.ToBase64String(combined);
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrEmpty(storedHash))
                return false;

            byte[] combined = Convert.FromBase64String(storedHash);
            if (combined.Length < 16 + 32)
                return false;

            byte[] salt = combined.Take(16).ToArray();
            byte[] originalHash = combined.Skip(16).Take(32).ToArray();

            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = Parallelism;
                argon2.Iterations = Iterations;
                argon2.MemorySize = MemorySizeKb;

                byte[] newHash = argon2.GetBytes(32);
                return FixedTimeEquals(originalHash, newHash);
            }
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }

            return result == 0;
        }
    }
}