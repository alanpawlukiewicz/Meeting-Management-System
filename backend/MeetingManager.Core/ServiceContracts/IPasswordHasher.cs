using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface IPasswordHasher
    {
        public string Hash(string password);
        public bool Verify(string password, string hash);
    }
}
