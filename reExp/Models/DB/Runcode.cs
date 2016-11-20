using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using reExp.Utils;


namespace reExp.Models.DB
{
    public partial class DB
    {
        static string location = Utils.Utils.RootFolder + @"App_Data\db.s3db";
        static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(string.Format("Data Source={0};Version=3;", location));
        }

        public static List<Dictionary<string, object>> Get_User_profile(int? UserId)
        {
            string query = @"select * from Users where id = @UserId";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserId", UserId));
            return ExecuteQuery(query, pars);
        }

        public static void Save_User_profile(int? UserId, int language_choice, int editor_choice)
        {
            string query = @"update users 
                             set last_language = @Language, last_editor = @Editor
                             where id = @UserId";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserId", UserId));
            pars.Add(new SQLiteParameter("Language", language_choice));
            pars.Add(new SQLiteParameter("Editor", editor_choice));
            ExecuteNonQuery(query, pars);
        }


        public static void Increment_Lang_Counter(string data, string input, string compiler_args, string result, int lang, bool is_api)
        {
            string query = @"INSERT OR REPLACE INTO LangCounters (lang_id, lang_name, counter) 
                             VALUES (@LangID, @Lang, coalesce((SELECT counter FROM LangCounters WHERE lang_id = @LangID), 0)+1)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("LangID", lang + (is_api ? 1000 : 0)));
            pars.Add(new SQLiteParameter("Lang", ((LanguagesEnum)lang).ToLanguage()+ (is_api ? "_api" : "")));
            ExecuteNonQuery(query, pars);
        }

        public static List<Dictionary<string, object>> Get_Lang_Counter()
        {
            string query = @"select lang_name, counter
                             from LangCounters";
            var pars = new List<SQLiteParameter>();
            return ExecuteQuery(query, pars);
        }
        public static void Log_Code_Insert(string data, string input, string compiler_args, string result, int lang)
        {
            string query = @"insert into LogCode(data, result, compiler_args, input, lang) values(@Data, @Result, @Args, @Input, @Lang)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Data", data));
            pars.Add(new SQLiteParameter("Result", result));
            if (input == null)
                pars.Add(new SQLiteParameter("Input", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("Input", input));
            if (compiler_args == null)
                pars.Add(new SQLiteParameter("Args", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("Args", compiler_args));
            pars.Add(new SQLiteParameter("Lang", lang));

            ExecuteNonQuery(query, pars);
        }

        public static void Log_Info_Insert(string info, string type)
        {
            string query = @"insert into LogInfo(info, type) values(@Info, @Type)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Info", info));
            pars.Add(new SQLiteParameter("Type", type));
            ExecuteNonQuery(query, pars);
        }

        public static void Code_Update(string newguid, string code, string input, string output, string compiler_args, string oldguid, string error, string warning, bool show_warnings, int status, string stats, string title)
        {
            string query = string.Format(@"insert into Code(code, input, guid, lang, editor, date, output, compiler_args, error, status, user_id, stats, warning, show_warnings, title, views, votes)
                                           select code, input, '{0}', lang, editor, date, output, compiler_args, error, status, user_id, stats, warning, show_warnings, title, null, null
                                           from Code
                                           where guid = @Guid", newguid);
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", oldguid));
            ExecuteNonQuery(query, pars);

            query = @"insert into Versions(primary_code_id, snapshot_code_id, time_created)
                      values((select id from Code where guid = @OldGuid), (select id from Code where guid = @NewGuid), DATETIME('now'))";
            pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("OldGuid", oldguid));
            pars.Add(new SQLiteParameter("NewGuid", newguid));
            ExecuteNonQuery(query, pars);

            query = @"update Code
                      set code = @Code, input = @Input, output = @Output, compiler_args= @Args, error = @Error, warning = @Warning, show_warnings = @Show_warnings, status = @Status, stats = @Stats, user_id = @User_id, date = DATETIME('now'), title = @Title
                      where guid = @OldGuid";
            pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("OldGuid", oldguid));
            pars.Add(new SQLiteParameter("Code", code));
            pars.Add(new SQLiteParameter("Input", input));
            pars.Add(new SQLiteParameter("Output", output));
            pars.Add(new SQLiteParameter("Args", compiler_args));
            pars.Add(new SQLiteParameter("Error", error));
            pars.Add(new SQLiteParameter("Warning", warning));
            pars.Add(new SQLiteParameter("Show_warnings", show_warnings));
            pars.Add(new SQLiteParameter("Status", status));
            pars.Add(new SQLiteParameter("Stats", stats));
            pars.Add(new SQLiteParameter("Title", title));
            if (SessionManager.UserId == null)
                pars.Add(new SQLiteParameter("User_id", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("User_id", SessionManager.UserId));
            ExecuteNonQuery(query, pars);

            if (SessionManager.UserId != null)
            {
                
                query = @"select count(*) as 'count' from UsersCode where user_id = @UserId and code_id = (select id from Code where guid = @OldGuid)";
                pars = new List<SQLiteParameter>();
                pars.Add(new SQLiteParameter("OldGuid", oldguid));
                pars.Add(new SQLiteParameter("UserId", SessionManager.UserId));
                var res = ExecuteQuery(query, pars);
                if (Convert.ToInt32(res[0]["count"]) == 0)
                {
                    query = @"insert into UsersCode(code_id, user_id, type, date)
                              values((select id from Code where guid = @OldGuid), @UserID, (select case when count(*) = 0 then 1 else 2 end from wall where code_id = (select id from Code where guid = @OldGuid)), DATETIME('now'))";
                    pars = new List<SQLiteParameter>();
                    pars.Add(new SQLiteParameter("OldGuid", oldguid));
                    pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                    ExecuteNonQuery(query, pars);
                }
                else
                {
                    query = @"update UsersCode
                              set date = DATETIME('now')
                              where user_id = @UserId and code_id = (select id from Code where guid = @OldGuid)";
                    pars = new List<SQLiteParameter>();
                    pars.Add(new SQLiteParameter("OldGuid", oldguid));
                    pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                    ExecuteNonQuery(query, pars);
                }
            }
        }

        public static List<Dictionary<string, object>> IsLive(string guid)
        {
            string query = @"select count(*) as count from livecode lc inner join code c on lc.code_id = c.id where c.guid = @Guid";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }

        public static void Live_Code_Update(string newguid, string code, string input, string output, string compiler_args, string oldguid, string error, string warning, bool show_warnings, int status, string stats, string title)
        {
            string query = string.Format(@"insert into Code(code, input, guid, lang, editor, date, output, compiler_args, error, status, user_id, stats, warning, show_warnings, title, views, votes)
                                           select code, input, '{0}', lang, editor, DATETIME('now'), output, compiler_args, error, status, {1}, stats, warning, show_warnings, title, null, null
                                           from Code
                                           where guid = @Guid", newguid, SessionManager.UserId == null ? "null" : SessionManager.UserId.ToString());
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", oldguid));
            ExecuteNonQuery(query, pars);

            query = @"update Code
                      set code = @Code, input = @Input, output = @Output, compiler_args = @Args, error = @Error, warning = @Warning, show_warnings = @Show_warnings, status = @Status, stats = @Stats, title = @Title
                      where guid = @NewGuid";
            pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("NewGuid", newguid));
            pars.Add(new SQLiteParameter("Code", code));
            pars.Add(new SQLiteParameter("Input", input));
            pars.Add(new SQLiteParameter("Output", output));
            pars.Add(new SQLiteParameter("Args", compiler_args));
            pars.Add(new SQLiteParameter("Error", error));
            pars.Add(new SQLiteParameter("Warning", warning));
            pars.Add(new SQLiteParameter("Show_warnings", show_warnings));
            pars.Add(new SQLiteParameter("Status", status));
            pars.Add(new SQLiteParameter("Stats", stats));
            pars.Add(new SQLiteParameter("Title", title));
            ExecuteNonQuery(query, pars);

            query = @"insert into Versions(primary_code_id, snapshot_code_id, time_created)
                      values((select id from Code where guid = @OldGuid), (select id from Code where guid = @NewGuid), DATETIME('now'))";
            pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("OldGuid", oldguid));
            pars.Add(new SQLiteParameter("NewGuid", newguid));
            ExecuteNonQuery(query, pars);


            query = @"update Code
                      set code = @Code
                      where guid = @Guid";
            pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", oldguid));
            pars.Add(new SQLiteParameter("Code", code));
            ExecuteNonQuery(query, pars);

            if (SessionManager.UserId != null)
            {
                query = @"select count(*) as 'count' from UsersCode where user_id = @UserId and code_id = (select id from Code where guid = @OldGuid)";
                pars = new List<SQLiteParameter>();
                pars.Add(new SQLiteParameter("OldGuid", oldguid));
                pars.Add(new SQLiteParameter("UserId", SessionManager.UserId));
                var res = ExecuteQuery(query, pars);
                if (Convert.ToInt32(res[0]["count"]) == 0)
                {
                    query = @"insert into UsersCode(code_id, user_id, type, date)
                              values((select id from Code where guid = @OldGuid), @UserID, 3, DATETIME('now'))";
                    pars = new List<SQLiteParameter>();
                    pars.Add(new SQLiteParameter("OldGuid", oldguid));
                    pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                    ExecuteNonQuery(query, pars);
                }
                else
                {
                    query = @"update UsersCode
                              set date = DATETIME('now')
                              where user_id = @UserId and code_id = (select id from Code where guid = @OldGuid)";
                    pars = new List<SQLiteParameter>();
                    pars.Add(new SQLiteParameter("OldGuid", oldguid));
                    pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                    ExecuteNonQuery(query, pars);
                }
            }
        }

        public static void Code_Insert(string title, string code, string input, string output, string compiler_args, int lang, int editor, string guid, int? userId, string error, string warning, bool show_warnings, int status, string stats, bool wall, bool live, bool personal)
        {
            string query = @"insert into Code(title, code, input, output, compiler_args, lang, editor, guid, user_id, error, warning, show_warnings, status, stats, votes, views) values(@Title, @Code, @Input, @Output, @Args, @Lang, @Editor, @Guid, @UserId, @Error, @Warning, @ShowWarnings, @Status, @Stats, @Votes, @Views)";
            var pars = new List<SQLiteParameter>();
            if (title == null)
                pars.Add(new SQLiteParameter("Title", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("Title", title));
            pars.Add(new SQLiteParameter("Code", code));            
            if(input == null)
                pars.Add(new SQLiteParameter("Input", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("Input", input));

            pars.Add(new SQLiteParameter("Output", output));
            pars.Add(new SQLiteParameter("Args", compiler_args));
            pars.Add(new SQLiteParameter("Lang", lang));
            pars.Add(new SQLiteParameter("Editor", editor));
            pars.Add(new SQLiteParameter("Guid", guid));
            pars.Add(new SQLiteParameter("Error", error));
            pars.Add(new SQLiteParameter("Warning", warning));
            pars.Add(new SQLiteParameter("ShowWarnings", show_warnings));
            pars.Add(new SQLiteParameter("Status", status));
            pars.Add(new SQLiteParameter("Stats", stats));
            if(userId == null)
                pars.Add(new SQLiteParameter("UserId", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("UserId", userId));
            if (wall || personal)
            {
                int a = 0;
                pars.Add(new SQLiteParameter("Votes", a));
                pars.Add(new SQLiteParameter("Views", a));
            }
            else
            {
                pars.Add(new SQLiteParameter("Votes", DBNull.Value));
                pars.Add(new SQLiteParameter("Views", DBNull.Value));
            }
            ExecuteNonQuery(query, pars);

            if (wall)
            {
                query = @"insert into wall(code_id) select id from code where guid = @Guid";
                pars = new List<SQLiteParameter>();
                pars.Add(new SQLiteParameter("Guid", guid));
                ExecuteNonQuery(query, pars);
            }

            if (personal)
            {
                query = @"select count(*) as total from UserWalls where user_id = @UserID";
                pars = new List<SQLiteParameter>();
                pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                var res = ExecuteQuery(query, pars);
                int total = Convert.ToInt32(res[0]["total"]);
                if (total == 0)
                {
                    var user_name = SessionManager.UserName;
                    var index = user_name.IndexOf('@');
                    if(index >= 0 && index != 0)
                        user_name = user_name.Substring(0, index);
                    if(string.IsNullOrEmpty(user_name))
                        user_name = "_";
                    query = @"insert into UserWalls(name, user_id) values(@Name, @UserID)";
                    pars = new List<SQLiteParameter>();
                    pars.Add(new SQLiteParameter("Name", user_name));
                    pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                    ExecuteNonQuery(query, pars);
                }
                query = @"insert into codeonwalls(userwalls_id, code_id, date_created) select w.id, c.id, DATETIME('now') from code c left outer join userwalls w on w.user_id = @UserID where c.guid = @Guid";
                pars = new List<SQLiteParameter>();
                pars.Add(new SQLiteParameter("Guid", guid));
                pars.Add(new SQLiteParameter("UserID", SessionManager.UserId));
                ExecuteNonQuery(query, pars);
            }

            if (userId != null)
            {
                query = @"insert into UsersCode(code_id, user_id, type, date) select id, @UserID, @Type, DATETIME('now') from code where guid = @Guid";
                pars = new List<SQLiteParameter>();
                pars.Add(new SQLiteParameter("Guid", guid));
                pars.Add(new SQLiteParameter("UserID", userId));
                string type = "1";
                if(wall)
                    type = "2";
                if(live)
                    type = "3";
                if (personal)
                    type = "4";
                pars.Add(new SQLiteParameter("Type", type));
                ExecuteNonQuery(query, pars);
            }
        }

        public static void Live_Code_Insert(string guid, string version_token)
        {
            string query = @"insert into LiveCode(code_id, version_token) values((select id from Code where guid = @Guid), @Version_token)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            pars.Add(new SQLiteParameter("Version_token", version_token));
            ExecuteNonQuery(query, pars);
        }

        public static List<Dictionary<string, object>> Live_Code_Get(string guid)
        {
            string query = @"select * from Code c inner join LiveCode lc on lc.code_id = c.id where c.guid = @Guid";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }

        public static void Delete_Code_Get(string guid)
        {
            string query = @"select lc.id from Code c inner join LiveCode lc on lc.code_id = c.id where c.guid = @Guid";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            var res = ExecuteQuery(query, pars);

            int? id = res[0]["id"] == DBNull.Value ? null : (int?)Convert.ToInt32(res[0]["id"]);

            if (id == null)
            {
                return;
            }

            query = @"delete from LiveCode where id = @Id";
            pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Id", id));
            ExecuteNonQuery(query, pars);
        }

        public static List<Dictionary<string, object>> Code_Get(string guid)
        {
            string query = @"select c.*, v.id as 'version_id', p.guid as 'primary_guid'
                             from Code c
                                  left outer join Versions v on c.id = v.snapshot_code_id
                                  left outer join Code p on p.id = v.primary_code_id
                             where c.guid = @Guid";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }

        public static void IncrementViews(string guid)
        {
            string query = @"update Code set Views = Views + 1 where guid = @Guid";
            List<SQLiteParameter> pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            ExecuteNonQuery(query, pars);
        }
        public static List<Dictionary<string, object>> CodeWithVotes_Get(string guid)
        {
            string query = string.Format(
                            @"select code.*, votes.voted, v.id as 'version_id', p.guid as 'primary_guid'
                                from Code code
                                    left outer join Votes votes on votes.code_id = code.id and votes.user_id = {0} 
                                    left outer join Versions v on code.id = v.snapshot_code_id
                                    left outer join Code p on p.id = v.primary_code_id
                                where code.guid = @Guid", SessionManager.UserId);
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetVersions(string guid)
        {
            string query = @"select vc.date as 'date', u.name as 'author', vc.guid as 'version_guid', uw.id as wall_id, vc.title as 'title'
                             from versions v
                                  inner join code pc on v.primary_code_id = pc.id
                                  inner join code vc on v.snapshot_code_id = vc.id
                                  left outer join users u on vc.user_id = u.id
                                  left outer join userwalls uw on u.id = uw.user_id
                             where pc.guid = @Guid";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }

        public static void Vote(string guid, bool up)
        {
            string query = string.Format(@"insert into Votes(code_id, user_id, voted) select id, {0}, {1} from code where guid = @Guid", SessionManager.UserId, up ? 1 : 0);
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            ExecuteNonQuery(query, pars);

            if (up)
                query = @"update Code set votes = votes+1 where guid = @Guid";
            else
                query = @"update Code set votes = votes-1 where guid = @Guid";

            ExecuteNonQuery(query, pars);
        }

        public static void CancelVote(string guid, bool up)
        {
            string query = string.Format(@"delete from Votes where code_id = (select id from Code where guid=@Guid) and user_id = {0}", SessionManager.UserId);
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            ExecuteNonQuery(query, pars);

            if (up)
                query = @"update Code set votes = votes-1 where guid = @Guid";
            else
                query = @"update Code set votes = votes+1 where guid = @Guid";

            ExecuteNonQuery(query, pars);
        }

        public static List<Dictionary<string, object>> HowVoted(string guid)
        {
            string query = string.Format(@"select voted from Votes where code_id = (select id from Code where guid = @Guid) and user_id = {0}", SessionManager.UserId);
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }


        static void ExecuteNonQuery(string query, List<SQLiteParameter> pars)
        {
            using (var Conn = GetConnection())
            {
                Conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, Conn))
                {
                    foreach (var par in pars)
                        command.Parameters.Add(par);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}