namespace webApi.Application.Services
{
    public class NotificationService : INotificationService
    {
        
        private static readonly List<Notification> Notifications = new();

        public List<Notification> GetAllNotifications => Notifications;
        

        public void Notify(string message, string type, ErrorType errorType)
        {

            var statusCode = errorType switch
            {
                ErrorType.Success => 200,
                ErrorType.Error => 500,
                ErrorType.Info => 202,
                ErrorType.NotFound => 404,
                ErrorType.Conflict => 409,
                ErrorType.ExternalServerError => 500
            };

            Notifications.Add(new Notification(message, type,statusCode));
        }

        public void Destroy()
        {
            Notifications.Clear();
        }

        public bool HasAnyNotification() => Notifications.Any();
    }

    public enum ErrorType
    {
        Success,
        Error,
        Info,
        Conflict,
        NotFound,
        ExternalServerError
    }
}


