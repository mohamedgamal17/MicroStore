using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;

namespace MicroStore.AspNetCore.UI
{
    public class UINotificationManager : IScopedDependency
    {
        const string NOTIFICATION_KEY = "MicroStoreUINotification";


        private readonly ITempDataDictionary _tempData;


        public UINotificationManager(IHttpContextAccessor httpContextAccessor , ITempDataDictionaryFactory tempDataDictionaryFactory )
        {
            _tempData = tempDataDictionaryFactory.GetTempData(httpContextAccessor.HttpContext);
        }


        public void Notifiy(Notification notification)
        {
            var notificationList =  _tempData.ContainsKey(NOTIFICATION_KEY) 
                   ? JsonConvert.DeserializeObject<List<Notification>>(_tempData[NOTIFICATION_KEY]!.ToString()!)!  :
                   new List<Notification>();

            notificationList.Add(notification);

            _tempData[NOTIFICATION_KEY] = JsonConvert.SerializeObject(notificationList);
        }


        public void Info(string message)
            => Notifiy(new Notification
            {
                Message = message,
                Type = NotificationType.Info
            });

        public void Success(string message)
            => Notifiy(new Notification
            {
                Message = message,
                Type = NotificationType.Success
            });
        public void Warning(string message)
            => Notifiy(new Notification
            {
                Message = message,
                Type = NotificationType.Warning
            });

        public void Error( string message)
            => Notifiy(new Notification
            {
                Message = message,
                Type = NotificationType.Error
            });


        public bool HasNotifications()
        {
            return GetNotifications().Any();
        }

        public int Count() => GetNotifications().Count;


        public List<Notification> GetNotifications()
        {
            return _tempData.ContainsKey(NOTIFICATION_KEY)
                       ? JsonConvert.DeserializeObject<List<Notification>>(_tempData[NOTIFICATION_KEY]!.ToString()!)! :
                       new List<Notification>();
        }
    }

    [Serializable]
    public class Notification
    {
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
