using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface ITokenHasher
    {
        public string Hash(string token);
        public bool Verify(string token, string hash);
    }
}
