using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Utils;

namespace reExp.Models
{
    public partial class Model
    {
        public static List<Notification> GetNotifications()
        {
            try
            {
                if (!SessionManager.IsUserInSession())
                    return new List<Notification>();

                List<Notification> items = new List<Notification>();
                var res = DB.DB.GetNotifications();
                foreach (var item in res)
                {
                    items.Add(new Notification()
                    {
                        ID = item["id"] == DBNull.Value ? null : (int?)Convert.ToInt32(item["id"]),
                        Name = item["name"] == DBNull.Value ? "-" : (string)item["name"],
                        Many = item["total"] == DBNull.Value ?  false : (Convert.ToInt32(item["total"]) > 1),
                        Type = item["type"] == DBNull.Value ? NotificationType.Subscription : (NotificationType)Convert.ToInt32(item["type"]),
                        DiscussionAddress = item["address"] == DBNull.Value ? null : (string)item["address"]
                    });
                }
                DB.DB.UpdateSubscriptionsCheckedDate();
                return items;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new List<Notification>();
            }
        }

        public static int GetNotificationsCount()
        {
            try
            {
                if (!SessionManager.IsUserInSession())
                    return 0;

                List<Notification> items = new List<Notification>();
                var res = DB.DB.GetNotificationsCount();
                return Convert.ToInt32(res[0]["total"]);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return 0;
            }
        }

        public static List<Subscription> GetSubscriptions()
        {
            try
            {
                if (!SessionManager.IsUserInSession())
                    return new List<Subscription>();

                List<Subscription> items = new List<Subscription>();
                var res = DB.DB.GetSubscriptions();
                foreach (var item in res)
                {
                    items.Add(new Subscription()
                    {
                        ID = item["id"] == DBNull.Value ? null : (int?)Convert.ToInt32(item["id"]),
                        Name = (string)item["name"]
                    });
                }
                return items;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new List<Subscription>();
            }
        }
    }

    public class Notification
    {
        public NotificationType Type { get; set; }
        public string Name { get; set; }
        public int? ID { get; set; }
        public bool Many { get; set; }
        public string DiscussionAddress { get; set; }
    }

    public enum NotificationType : int
    {
        Subscription = 1,
        Comment = 2
    }
    public class Subscription
    {
        public string Name { get; set; }
        public int? ID { get; set; }
    }
}