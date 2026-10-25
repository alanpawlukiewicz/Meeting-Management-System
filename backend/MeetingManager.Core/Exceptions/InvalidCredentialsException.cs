namespace MeetingManager.Core.Exceptions
{
    public class InvalidCredentialsException : AppException
    {
        public InvalidCredentialsException(string message) : base(message) { }
    }
}
