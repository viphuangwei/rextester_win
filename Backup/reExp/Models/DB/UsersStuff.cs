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
        public static List<Dictionary<string, object>> GetUsersItems(int userId, int page)
        {
            string query = @"select users_code.ID, 
                                case when code_id is not null then 1 
                                     when regex_id is not null then 2 
                                     when regexreplace_id is not null then 3 
                                end as 'type',
                                c.title,
                                c.code,
                                c.status,
                                c.lang,
                                case when code_id is not null then c.guid 
                                     when regex_id is not null then r.guid 
                                     when regexreplace_id is not null then rr.guid 
                                end as 'guid',
                                users_code.date,
                                case when code_id is not null then null 
                                     when regex_id is not null then r.regex
                                     when regexreplace_id is not null then rr.regex
                                end as 'regex',
                                case when type = 2 then 1 else null end as 'is_wall',
                                case when type = 3 then 1 else null end as 'is_live',
                                case when type = 4 then 1 else null end as 'is_personal_wall'
                            from UsersCode users_code
                                left outer join Code c on c.id = users_code.code_id
                                left outer join Regex r on r.id = users_code.regex_id
                                left outer join RegexReplace rr on rr.id = users_code.regexreplace_id
                            where users_code.user_id = @UserId                           
                            order by users_code.date desc 
                            limit @Limit
                            offset @Offset";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserId", userId));
            pars.Add(new SQLiteParameter("Limit", GlobalConst.RecordsPerPage));
            pars.Add(new SQLiteParameter("Offset", page * GlobalConst.RecordsPerPage));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetUsersTotal(int userId)
        {
            string query = @"select count(*) as 'total'
                            from UsersCode users_code
                                left outer join Code c on c.id = users_code.code_id
                                left outer join Regex r on r.id = users_code.regex_id
                                left outer join RegexReplace rr on rr.id = users_code.regexreplace_id
                            where users_code.user_id = @UserId";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserId", userId));
            return ExecuteQuery(query, pars);
        }


        public static bool DeleteUserItem(int id)
        {
            if (SessionManager.UserId == null)
                return false;

            string query = @"select user_id from UsersCode where id = @Id";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("@Id", id));
            var res = ExecuteQuery(query, pars);
            if (Convert.ToInt32(res[0]["user_id"]) != SessionManager.UserId)
                return false;

            query = @"delete from UsersCode where id = @Id";
            pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("@Id", id));
            ExecuteNonQuery(query, pars);
            
            return true;
        }

        public static List<Dictionary<string, object>> GoogleEmailExists(string email)
        {
            string query = @"select * from Users where email = @Email and name is not null";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Email", email));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetUser(string name)
        {
            string query = @"select * from Users where name = @Name";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Name", name));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetUserByGuid(string guid)
        {
            string query = @"select u.name,
                                    uw.id as wall_id 
                             from Code c 
                                  inner join Users u on c.user_id = u.id 
                                  left outer join UserWalls uw on u.id = uw.user_id
                             where c.guid = @Guid";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }

        public static void InsertUser(string name, string passwordHash, string emailHash)
        {
            string query = @"insert into Users(name, password, email) values(@Name, @Password, @Email)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Name", name));
            if (passwordHash == null)
                pars.Add(new SQLiteParameter("Password", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("Password", passwordHash));
            if (emailHash == null)
                pars.Add(new SQLiteParameter("Email", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("Email", emailHash));
            ExecuteNonQuery(query, pars);
        }

    }

   

}