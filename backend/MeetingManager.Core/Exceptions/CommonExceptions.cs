namespace MeetingManager.Core.Exceptions
{
    public class ValidationException : AppException
    {
        public ValidationException(string message) : base(message) { }
    }
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ConflictException : AppException
    {
        public ConflictException(string message) : base(message) { }
    }
}
