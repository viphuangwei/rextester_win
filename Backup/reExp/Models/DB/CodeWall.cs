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
        public static List<Dictionary<string, object>> GetWallsCode(int page, int sort)
        {
            string query = string.Format(
                            @"select c.*
                            from Wall w
                                 inner join Code c on w.code_id = c.id
                            order by {0}
                            limit @Limit
                            offset @Offset",
                            sort == 0 ? " c.date desc " : " c.votes desc, c.date desc ");
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Limit", GlobalConst.RecordsPerPage));
            pars.Add(new SQLiteParameter("Offset", page * GlobalConst.RecordsPerPage));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetWallsTotal()
        {
            string query = @"select count(*) as total from Wall";
            return ExecuteQuery(query, new List<SQLiteParameter>());
        }

        public static List<Dictionary<string, object>> IsUserSubscribed(int? wall_id)
        {
            if (wall_id != null)
            {
                string query = @"select case when count(*) = 0 then 0 else 1 end as subscribed from Subscriptions where user_id = @UserID and userwalls_id = @WallID";
                var pars = new List<SQLiteParameter>();
                pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                pars.Add(new SQLiteParameter("WallID", wall_id));
                return ExecuteQuery(query, pars);
            }
            else
            {
                string query = @"select case when count(*) = 0 then 0 else 1 end as subscribed from Subscriptions where user_id = @UserID and userwalls_id is null";
                var pars = new List<SQLiteParameter>();
                pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                return ExecuteQuery(query, pars);
            }
        }

        public static void Subscribe(int? wall_id, bool is_subscribed)
        {
            if (is_subscribed)
            {
                if (wall_id != null)
                {
                    string query = @"delete from subscriptions where user_id = @UserID and userwalls_id = @WallID";
                    var pars = new List<SQLiteParameter>();
                    pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                    pars.Add(new SQLiteParameter("WallID", wall_id));
                    ExecuteNonQuery(query, pars);
                }
                else
                {
                    string query = @"delete from subscriptions where user_id = @UserID and userwalls_id is null";
                    var pars = new List<SQLiteParameter>();
                    pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                    ExecuteNonQuery(query, pars);
                }
            }
            else
            {
                if (wall_id != null)
                {
                    string query = @"insert into subscriptions(userwalls_id, user_id) values(@WallID, @UserID)";
                    var pars = new List<SQLiteParameter>();
                    pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                    pars.Add(new SQLiteParameter("WallID", wall_id));
                    ExecuteNonQuery(query, pars);
                }
                else
                {
                    string query = @"insert into subscriptions(userwalls_id, user_id) values(null, @UserID)";
                    var pars = new List<SQLiteParameter>();
                    pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                    ExecuteNonQuery(query, pars);
                }
            }
        }

        static List<Dictionary<string, object>> ExecuteQuery(string query, List<SQLiteParameter> pars)
        {
            List<Dictionary<string, object>> res = new List<Dictionary<string, object>>();
            using (var Conn = GetConnection())
            {
                Conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, Conn))
                {
                    foreach (var par in pars)
                        command.Parameters.Add(par);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                res.Add(new Dictionary<string, object>());
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    res[res.Count - 1][reader.GetName(i)] = reader[i];
                                }
                            }
                        }
                    }
                }
            }
            return res;
        }
    }
}