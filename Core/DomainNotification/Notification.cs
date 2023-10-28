using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace webApi.Application.Services
{
    public class Notification
    {
        public Guid Id { get; private set; }
        public string Message { get; private set; }
        public string Type { get; private set; }
        public int StatusCode { get; private set; }

        public Notification(string message, string type, int statusCode)
        {
            Id = Guid.NewGuid();
            Message = message;
            Type = type;
            StatusCode = statusCode;

        }
    }
}
