using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Exceptions
{
    public class InvalidRefreshTokenException : AppException
    {
        public InvalidRefreshTokenException(string message) : base(message) { }
    }
}
