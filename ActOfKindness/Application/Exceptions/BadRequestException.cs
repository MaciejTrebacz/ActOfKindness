namespace Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public string Method { get; set; }
        public string? UserId { get; set; }
        public string? UserRole { get; set; }
        public string UserMessage { get; set; }

        public BadRequestException(string message = "", string userMessage = "", string method = null, string? userId = null, string? userRole = null) : base(message)
        {
            Method = method;
            UserId = userId;
            UserRole = userRole;
            UserMessage = userMessage;
        }
    }
}
