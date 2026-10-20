using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Exceptions
{
    public class InvalidCredentialsException : AppException
    {
        public InvalidCredentialsException(string message) : base(message) { }
    }
}
