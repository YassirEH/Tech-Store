namespace webApi.Application.Services
{
    public interface INotificationService
    {
        void Notify(string type, string message, ErrorType errorType);
        void Destroy();
        bool HasAnyNotification();

        List<Notification> GetAllNotifications { get; }
    }
}
