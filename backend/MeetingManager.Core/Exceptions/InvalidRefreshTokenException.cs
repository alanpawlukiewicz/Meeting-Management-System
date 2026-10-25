namespace MeetingManager.Core.Exceptions
{
    public class InvalidRefreshTokenException : AppException
    {
        public InvalidRefreshTokenException(string message) : base(message) { }
    }
}
