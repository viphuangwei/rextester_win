using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace reExp.Models.DB
{
    public partial class DB
    {
        public static List<Dictionary<string, object>> Regex_Get(string guid)
        {
            string query = @"select * from Regex where guid = @Guid COLLATE NOCASE";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }

        public static void Regex_Insert(string regex, string text, string output, string options, string guid, int? userId)
        {
            string query = @"insert into Regex(regex, text, output, options, guid, user_id) values(@Regex, @Text, @Output, @Options, @Guid, @UserId)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Regex", regex));
            pars.Add(new SQLiteParameter("Text", text));
            pars.Add(new SQLiteParameter("Output", output));
            pars.Add(new SQLiteParameter("Options", options));
            pars.Add(new SQLiteParameter("Guid", guid));
            if (userId == null)
                pars.Add(new SQLiteParameter("UserId", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("UserId", userId));
            ExecuteNonQuery(query, pars);

            if (userId != null)
            {
                query = @"insert into UsersCode(regex_id, user_id, date) select id, @UserID, DATETIME('now') from Regex where guid = @Guid";
                pars = new List<SQLiteParameter>();
                pars.Add(new SQLiteParameter("Guid", guid));
                pars.Add(new SQLiteParameter("UserID", userId));
                ExecuteNonQuery(query, pars);
            }
        }

        public static List<Dictionary<string, object>> Regex_Replace_Get(string guid)
        {
            string query = @"select * from RegexReplace where guid = @Guid COLLATE NOCASE";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }

        public static void Regex_Replace_Insert(string regex, string replacement, string text, string output, string options, string guid, int? userId)
        {
            string query = @"insert into RegexReplace(regex, replacement, text, output, options, guid, user_id) values(@Regex, @Replacement, @Text, @Output, @Options, @Guid, @UserId)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Regex", regex));
            pars.Add(new SQLiteParameter("Replacement", replacement));
            pars.Add(new SQLiteParameter("Text", text));
            pars.Add(new SQLiteParameter("Output", output));
            pars.Add(new SQLiteParameter("Options", options));
            pars.Add(new SQLiteParameter("Guid", guid));
            if (userId == null)
                pars.Add(new SQLiteParameter("UserId", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("UserId", userId));
            ExecuteNonQuery(query, pars);

            if (userId != null)
            {
                query = @"insert into UsersCode(regexreplace_id, user_id, date) select id, @UserID, DATETIME('now') from RegexReplace where guid = @Guid";
                pars = new List<SQLiteParameter>();
                pars.Add(new SQLiteParameter("Guid", guid));
                pars.Add(new SQLiteParameter("UserID", userId));
                ExecuteNonQuery(query, pars);
            }
        }
    }
}