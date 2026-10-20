using MeetingManager.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MeetingManager.Infrastructure.Services
{
    public class Sha256TokenHasher : ITokenHasher
    {
        public string Hash(string token)
            => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        public bool Verify(string token, string hash)
        {
            var hashedToken = Hash(token);
            return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(hashedToken), Encoding.UTF8.GetBytes(hash));
        }
    }
}
