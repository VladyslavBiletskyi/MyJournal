using System;

namespace MyJournal.Services.Extensibility
{
    internal interface IPasswordHasher
    {
        Tuple<string, string> GetHash(string password, byte[] salt = null);

        bool CheckPassword(string password, string hash, string salt);
    }
}
