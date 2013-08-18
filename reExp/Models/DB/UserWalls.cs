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
        public static List<Dictionary<string, object>> GetUsersWalls(int page)
        {
            string query = @"select w.id, w.name, w.user_id
                            from UserWalls w
                            order by w.id desc
                            limit @Limit
                            offset @Offset";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Limit", GlobalConst.RecordsPerPage));
            pars.Add(new SQLiteParameter("Offset", page * GlobalConst.RecordsPerPage));
            return ExecuteQuery(query, pars);
        }


        public static List<Dictionary<string, object>> GetUserWallsTotal()
        {
            string query = @"select count(*) as total from UserWalls";
            return ExecuteQuery(query, new List<SQLiteParameter>());
        }


        public static List<Dictionary<string, object>> GetUserWallsCode(int id, int page, int sort)
        {
            string query = string.Format(@"select c.*, w.id as 'code_on_wall_id'
                                           from CodeOnWalls w
                                                   inner join Code c on w.code_id = c.id
                                           where w.userwalls_id = @Wall_ID
                                           order by {0}
                                           limit @Limit
                                           offset @Offset", 
                                           sort == 0 ? "c.date desc " : "c.votes desc, c.date desc");
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Wall_ID", id));
            pars.Add(new SQLiteParameter("Limit", GlobalConst.RecordsPerPage));
            pars.Add(new SQLiteParameter("Offset", page * GlobalConst.RecordsPerPage));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetUserWallCodesTotal(int id)
        {
            string query = @"select count(*) as total from CodeOnWalls c where c.userwalls_id = @WallsID";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("WallsID", id));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetWallsName(int id)
        {
            string query = @"select name from UserWalls where id = @WallsID";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("WallsID", id));
            return ExecuteQuery(query, pars);
        }


        public static List<Dictionary<string, object>> GetUserWallId()
        {
            string query = @"select id from UserWalls where user_id = @UserID";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetUserWallUserID(int id)
        {
            string query = @"select user_id from UserWalls where id = @WallsID";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("WallsID", id));
            return ExecuteQuery(query, pars);
        }

        public static bool DeleteUserWallItem(int id)
        {
            string query = @"select uw.user_id, w.code_id
                             from CodeOnWalls w
                                  inner join UserWalls uw on uw.id = w.userwalls_id
                             where w.id = @Id";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("@Id", id));
            var res = ExecuteQuery(query, pars);
            if (Convert.ToInt32(res[0]["user_id"]) != SessionManager.UserId)
                return false;

            int code_id = Convert.ToInt32(res[0]["code_id"]);

            query = @"delete from CodeOnWalls where id = @Id";
            pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("@Id", id));
            ExecuteNonQuery(query, pars);

            query = @"update userscode set type = 1 where code_id = @CodeID and user_id = @UserID and type = 4";
            pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("@CodeID", code_id));
            pars.Add(new SQLiteParameter("@UserID", SessionManager.UserId));
            ExecuteNonQuery(query, pars);
            return true;
        }
    }
}