using System;
using System.Security.Cryptography;
using MyJournal.Services.Extensibility;

namespace MyJournal.Services
{
    public class PasswordHasher : IPasswordHasher
    {

        private const int SaltByteSize = 24;
        private const int HashByteSize = 24;
        private const int HashingIterationsCount = 501;

        public Tuple<string, string> GetHash(string password, byte[] salt = null)
        {
            var usedSalt = salt ?? GenerateSalt();
            using (Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(password, usedSalt))
            {
                hashGenerator.IterationCount = HashingIterationsCount;
                var passwordHash = hashGenerator.GetBytes(HashByteSize);
                return new Tuple<string, string>(
                    Convert.ToBase64String(passwordHash),
                    Convert.ToBase64String(usedSalt));
            }
        }

        public bool CheckPassword(string password, string hash, string salt)
        {
            try
            {
                var saltBytes = Convert.FromBase64String(salt);
                var receivedPasswordHash = GetHash(password, saltBytes);
                return receivedPasswordHash.Item1 == hash;
            }
            catch
            {
                return false;
            }
        }

        private byte[] GenerateSalt()
        {
            using (RNGCryptoServiceProvider saltGenerator = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltByteSize];
                saltGenerator.GetBytes(salt);
                return salt;
            }
        }
    }
}
