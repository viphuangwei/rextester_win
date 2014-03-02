using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using reExp.Utils;

namespace reExp.Models.DB
{
    public partial class DB
    {
        public static List<Dictionary<string, object>> GetNotifications()
        {
            string query = @"select w.name, w.id, count(w.id) as total
                             from  Subscriptions sub
                                   inner join UserWalls w on sub.userwalls_id = w.id
                                   inner join CodeOnWalls code on w.id = code.userwalls_id
                             where sub.user_id = @UserID and sub.last_checked < code.date_created
                             group by w.id
                             
                             union all
                                
                             select 'code wall' as name, null as id, count(*) as total
                             from Subscriptions sub
                                  inner join wall w on sub.userwalls_id is null
                                  inner join code c on w.code_id = c.id
                             where sub.user_id = @UserID and sub.last_checked < w.date_created and ifnull(c.user_id, 0) <> @UserID
                             group by sub.user_id";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserId", SessionManager.UserId));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetNotificationsCount()
        {
            string query = @"select count(*) as 'total' from
                               (select 1
                                from  Subscriptions sub
                                    inner join UserWalls w on sub.userwalls_id = w.id
                                    inner join CodeOnWalls code on w.id = code.userwalls_id
                                where sub.user_id = @UserID and sub.last_checked < code.date_created
                                                    
                                union all
                                
                                select 1
                                from Subscriptions sub
                                    inner join wall w on sub.userwalls_id is null
                                    inner join code c on w.code_id = c.id
                                where sub.user_id = @UserID and sub.last_checked < w.date_created and ifnull(c.user_id, 0) <> @UserID)";

            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserId", SessionManager.UserId));
            return ExecuteQuery(query, pars);
        }

        public static void UpdateSubscriptionsCheckedDate()
        {
            string query = @"update Subscriptions set last_checked = DATETIME('now') where user_id = @UserID";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserId", SessionManager.UserId));
            ExecuteNonQuery(query, pars);            
        }

        public static List<Dictionary<string, object>> GetSubscriptions()
        {
            string query = @"select w.id, case when w.id is null then 'code wall' else w.name end as 'name'
                             from subscriptions sub
                                  left outer join userwalls w on sub.userwalls_id = w.id
                             where sub.user_id = @UserID";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserId", SessionManager.UserId));
            return ExecuteQuery(query, pars);
        }
    }
}