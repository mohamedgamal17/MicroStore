using Volo.Abp.DependencyInjection;

namespace MicroStore.Client.PublicWeb.Infrastructure
{
    public class UINotificationManager : IScopedDependency
    {

        private readonly Queue<Notification> _notifications;

        public UINotificationManager()
        {
            _notifications = new Queue<Notification>();
        }


        public void Notifiy(Notification notification) => _notifications.Enqueue(notification);


        public void Info(string title, string message)
            => Notifiy(new Notification
            {
                Title = title,
                Message = message,
                Type = NotificationType.Info
            });

        public void Success(string title, string message)
            => Notifiy(new Notification
            {
                Title = title,
                Message = message,
                Type = NotificationType.Success
            });   
        public void Warning(string title, string message)
            => Notifiy(new Notification
            {
                Title = title,
                Message = message,
                Type = NotificationType.Warning
            });   
        
        public void Error(string title, string message)
            => Notifiy(new Notification
            {
                Title = title,
                Message = message,
                Type = NotificationType.Error
            });


        public bool HasNotification()
        {
            return _notifications.Any();
        }

        public int Count() => _notifications.Count;

        public Notification GetNotification() => _notifications.Dequeue();
    }

    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        
    }

    public enum NotificationType
    {
        Info,
        Success,
        Warning,
        Error
    }
}
