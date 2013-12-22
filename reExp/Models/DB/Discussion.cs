using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace reExp.Models.DB
{
    public partial class DB
    {
        public static List<Dictionary<string, object>> Comments_Get(string guid)
        {
            string query = @"select com.id,
                                    com.code_id,
                                    com.user_id,
                                    com.text,
                                    com.edited_date,
                                    com.date,
                                    user.Name as user_name,
                                    code.guid
                             from Comments com
                                  inner join Users user on com.user_id = user.id
                                  inner join Code code on com.code_id = code.id
                             where com.code_id = (select id from code where guid=@Guid)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> Comment_Get(int Comments_ID)
        {
            string query = @"select com.id,
                                    com.code_id,
                                    com.user_id,
                                    com.text,
                                    com.edited_date,
                                    com.date,
                                    user.Name as user_name
                             from Comments com
                                  inner join Users user on com.user_id = user.id
                             where com.id = @CommentsId";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("CommentsId", Comments_ID));
            return ExecuteQuery(query, pars);
        }

        public static void Comments_Insert(int code_id, int user_id, string text)
        {
            string query = @"insert into Comments(code_id, user_id, text, date) values(@Code_id, @User_id, @Text, DATETIME('now'))";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Code_id", code_id));
            pars.Add(new SQLiteParameter("User_id", user_id));
            pars.Add(new SQLiteParameter("Text", text));
            ExecuteNonQuery(query, pars);
        }

        public static void Comments_Update(int comments_id, string text)
        {
            string query = @"update Comments set Text = @Text, edited_date = DATETIME('now') where id = @Id";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Text", text));
            pars.Add(new SQLiteParameter("Id", comments_id));
            ExecuteNonQuery(query, pars);
        }

        public static void Comments_Delete(int comments_id)
        {
            var query = @"delete from Comments where id = @Id";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Id", comments_id));
            ExecuteNonQuery(query, pars);
        }
    }
}