using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Controllers.rundotnet;
using reExp.Controllers.regex;
using System.Security.Cryptography;
using reExp.Utils;
using BookSleeve;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading.Tasks;
using LinqDb;
using System.IO;

namespace reExp.Models
{
    public partial class Model
    {
        public class RedisData
        {
            public string Program { get; set; }
            public string Input { get; set; }
            public string Args { get; set; }
            public int LanguageChoice { get; set; }

            public string WholeError { get; set; }
            public string WholeWarning { get; set; }
            public string WholeOutput { get; set; }
            public string RunStatus { get; set; }
        }
        static RedisConnection redis_conn = null;
        static object redis_lock = new object();
        public static RedisConnection RedisConnection
        {
            get
            {
                lock (redis_lock)
                {
                    if (redis_conn == null)
                    {
                        redis_conn = new RedisConnection("localhost");
                    }
                    if (redis_conn.State != RedisConnectionBase.ConnectionState.Open)
                    {
                        redis_conn.Open().Wait();
                    }
                }
                return redis_conn;
            }
        }
        public static int redis_db = 0;
        public static void InsertRundotnetDataToRedis(RundotnetData data)
        {
            try
            {
                if (data.LanguageChoice == LanguagesEnum.Octave)
                    return;

                var conn = RedisConnection;
                JavaScriptSerializer json = new JavaScriptSerializer();
                string key = json.Serialize(new RedisData() 
                    {
                        Program = data.Program,
                        Input = data.Input,
                        Args = data.CompilerArgs,
                        LanguageChoice = (int)data.LanguageChoice
                    });
                key = Utils.EncryptionUtils.CreateMD5Hash(key);
                string val = json.Serialize(new RedisData() 
                    { 
                        Program = data.Program, 
                        Input = data.Input, 
                        Args = data.CompilerArgs, 
                        LanguageChoice = (int)data.LanguageChoice,
                        WholeError = data.WholeError,
                        WholeWarning = data.WholeWarning,
                        WholeOutput = data.Output,
                        RunStatus = data.RunStats
                    });
                val = GlobalUtils.Utils.Compress(val);
                conn.Strings.Set(redis_db, new Dictionary<string, byte[]>() { { key, Encoding.UTF8.GetBytes(val) } }).Wait();
                conn.Keys.Expire(redis_db, key, 24 * 60 * 60);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "redis insert error");
            }
        }

        public static RedisData GetRundotnetDataFromRedis(RundotnetData data)
        {
            try
            {
                if (data.LanguageChoice == LanguagesEnum.Octave)
                    return null;
                
                var conn = RedisConnection;
                JavaScriptSerializer json = new JavaScriptSerializer();
                string key = json.Serialize(new RedisData()
                {
                    Program = data.Program,
                    Input = data.LanguageChoice == LanguagesEnum.Prolog ? data.Input + "\nhalt." : data.Input,
                    Args = data.CompilerArgs,
                    LanguageChoice = (int)data.LanguageChoice
                });
                key = Utils.EncryptionUtils.CreateMD5Hash(key);
                var bytes = conn.Strings.Get(redis_db, key).Result;
                if (bytes == null)
                    return null;
                string val = Encoding.UTF8.GetString(bytes);
                return json.Deserialize<RedisData>(GlobalUtils.Utils.Decompress(val));
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "redis get error");
                return null;
            }
        }

        public static void IncrementLangCounter(string data, string input, string compiler_args, string result, int lang, bool is_api)
        {
            try
            {
                DB.DB.Increment_Lang_Counter(data, input, compiler_args, result, lang, is_api);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
            }
        }
        public static Dictionary<string, long> GetLangCounter()
        {
            try
            {
                var res = DB.DB.Get_Lang_Counter();
                return res.ToDictionary(f => (string)(f["lang_name"]), f => Convert.ToInt64(f["counter"])); 
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new Dictionary<string, long>(); ;
            }  
        }
        public static void LogCodeData(string data, string input, string compiler_args, string result, int lang)
        {
            DB.DB.Log_Code_Insert(data, input, compiler_args, result, lang);
        }

        public static void LogInfo(string info, string type)
        {
            DB.DB.Log_Info_Insert(info, type);
        }
      
        public static string SaveCode(RundotnetData data, bool wall = false, bool live = false, bool personal = false)
        {
            var guid = Utils.Utils.GetGuid();
            
            try
            {
                DB.DB.Code_Insert(data.Title, data.Program, data.Input, data.SavedOutput, data.CompilerArgs, Convert.ToInt32(data.LanguageChoice), Convert.ToInt32(data.EditorChoice), guid, SessionManager.UserId, data.WholeError, data.WholeWarning, data.ShowWarnings, (int)data.Status, data.StatsToSave, wall, live, personal);

                if (SessionManager.IsUserInSession())
                {
                    int uid = (int)SessionManager.UserId;
                    Task.Run(() =>
                        {
                            Search.PutUserItem(new UsersItem()
                            {
                                Code = data.Program,
                                Date = DateTime.Now,
                                Guid = guid,
                                ID = "code_"+guid,
                                Lang = data.LanguageChoice.ToLanguage(),
                                UserId = uid,
                                Title = data.Title,
                                IsLive = live == true ? 1 : 0
                            });
                        });
                }
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return "";    
            }
            return guid;
        }
        public static void UpdateLiveCache(string code, string chat, string guid)
        {
            var c = Model.GetCode(guid);
            Search.PutUserItem(new UsersItem()
            {
                Code = code,
                Date = c.Date,
                Guid = guid,
                ID = "code_" + guid,
                Lang = c.Lang.ToLanguage(),
                UserId = (int)c.UserId,
                Text = chat,
                IsLive = 1,
                Title = c.Title
            });
        }

        public static void UpdateCode(RundotnetData data)
        {
            var guid = Utils.Utils.GetGuid();
            try
            {
                if (data.IsLive)
                {
                    DB.DB.Live_Code_Update(guid, data.Program, data.Input, data.SavedOutput, data.CompilerArgs, data.CodeGuid, data.WholeError, data.WholeWarning, data.ShowWarnings, (int)data.Status, data.StatsToSave, data.Title);
                }
                else
                {
                    DB.DB.Code_Update(guid, data.Program, data.Input, data.SavedOutput, data.CompilerArgs, data.CodeGuid, data.WholeError, data.WholeWarning, data.ShowWarnings, (int)data.Status, data.StatsToSave, data.Title);
                    if (SessionManager.IsUserInSession())
                    {
                        int uid = (int)SessionManager.UserId;
                        Task.Run(() =>
                        {
                            Search.PutUserItem(new UsersItem()
                            {
                                Code = data.Program,
                                Date = DateTime.Now,
                                Guid = data.CodeGuid,
                                ID = "code_" + data.CodeGuid,
                                Lang = data.LanguageChoice.ToLanguage(),
                                UserId = uid,
                                Title = data.Title
                            });
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
            }
        }

        public static bool IsLive(string guid)
        { 
            try
            {
                var res = DB.DB.IsLive(guid);
                return Convert.ToInt32(res[0]["count"]) != 0;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return false;
            }
        }

        public static List<Version> GetVersions(string CodeGuid)
        {
            try
            {
                List<Version> versions = new List<Version>();
                var res = DB.DB.GetVersions(CodeGuid);
                foreach (var ver in res)
                {
                    versions.Add(new Version()
                    {
                        DateCreated = (DateTime)ver["date"],
                        Author = ver["author"] == DBNull.Value ? null : (string)(ver["author"]),
                        VersionGuid = (string)ver["version_guid"],
                        Wall_Id = ver["wall_id"] == DBNull.Value ? null : (int?)Convert.ToInt32((ver["wall_id"])),
                        Title = (string)(ver["title"] == DBNull.Value ? "" : ver["title"]),
                    });
                }
                return versions;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new List<Version>();
            }
        }

        public static void MakeLiveCode(string guid, string version_token)
        {
            try
            {
                DB.DB.Live_Code_Insert(guid, version_token);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
            }
        }

        public static Code GetLiveCode(string guid)
        {
            try
            {
                var res = DB.DB.Live_Code_Get(guid);
                if (res.Count != 0)
                    return new Code()
                    {
                        Program = (string)res[0]["code"],
                        Lang = (LanguagesEnum)Convert.ToInt32(res[0]["lang"]),
                        CompilerArgs = (string)(res[0]["compiler_args"] == DBNull.Value ? "" : res[0]["compiler_args"]),
                        Editor = (EditorsEnum)Convert.ToInt32(res[0]["editor"]),
                        Input = res[0]["input"] == DBNull.Value ? "" : (string)res[0]["input"],
                        Guid = (string)res[0]["guid"],
                        ShowWarnings = (res[0]["show_warnings"] == DBNull.Value ? false : (bool)res[0]["show_warnings"]),
                        VersionToken = (string)res[0]["version_token"],
                        UserId = res[0]["user_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(res[0]["user_id"]),
                        Title = (string)(res[0]["title"] == DBNull.Value ? "" : res[0]["title"]),
                    };
                else
                    return null;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return null;
            }
        }

        public static void DeleteLiveCode(string guid)
        {
            try
            {
                DB.DB.Delete_Code_Get(guid);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
            }
        }

        public static Code GetCode(string guid, bool incrementViews = true)
        {
            try
            {
                Code code = null;
                var res = DB.DB.Code_Get(guid);
                if (res.Count != 0)
                {
                    code = new Code()
                    {
                        Title = (string)(res[0]["title"] == DBNull.Value ? "" : res[0]["title"]),
                        Program = (string)res[0]["code"],
                        Output = (string)(res[0]["output"] == DBNull.Value ? "" : res[0]["output"]),
                        CompilerArgs = (string)(res[0]["compiler_args"] == DBNull.Value ? "" : res[0]["compiler_args"]),
                        Input = (string)(res[0]["input"] == DBNull.Value ? null : res[0]["input"]),
                        Lang = (LanguagesEnum)Convert.ToInt32(res[0]["lang"]),
                        Editor = (EditorsEnum)Convert.ToInt32(res[0]["editor"]),
                        Guid = (string)res[0]["guid"],
                        WholeError = (string)(res[0]["error"] == DBNull.Value ? "" : res[0]["error"]),
                        Warnings = (string)(res[0]["warning"] == DBNull.Value ? "" : res[0]["warning"]),
                        Status = (res[0]["status"] == DBNull.Value ? GlobalConst.RundotnetStatus.Unknown : Convert.ToInt32(res[0]["status"]).ToRundotnetStatus()),
                        Stats = (string)(res[0]["stats"] == DBNull.Value ? "" : res[0]["stats"]),
                        ShowWarnings = (res[0]["show_warnings"] == DBNull.Value ? false : (bool)res[0]["show_warnings"]),
                        Votes = (res[0]["votes"] == DBNull.Value ? null : (int?)Convert.ToInt32(res[0]["votes"])),
                        Views = (res[0]["views"] == DBNull.Value ? null : (int?)Convert.ToInt32(res[0]["views"])),
                        IsPrimaryVersion = (res[0]["version_id"] == DBNull.Value ? true : false),
                        PrimaryGuid = (res[0]["primary_guid"] == DBNull.Value ? null : (string)res[0]["primary_guid"]),
                        Date = (DateTime)res[0]["date"],
                        ID = Convert.ToInt32(res[0]["id"]),
                        UserId = res[0]["user_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(res[0]["user_id"])
                    };
                    if (code.Votes != null && SessionManager.IsUserInSession())                     
                        code = GetCodeWithInfo(guid);

                    if(code.Views != null && incrementViews)
                        DB.DB.IncrementViews(guid);

                    return code;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return null;
            }
        }

        public static Code GetCodeWithInfo(string guid)
        {
            try
            {
                var res = DB.DB.CodeWithVotes_Get(guid);
                if (res.Count != 0)
                    return new Code()
                    {
                        Title = (string)(res[0]["title"] == DBNull.Value ? "" : res[0]["title"]),
                        Program = (string)res[0]["code"],
                        Output = (string)(res[0]["output"] == DBNull.Value ? "" : res[0]["output"]),
                        CompilerArgs = (string)(res[0]["compiler_args"] == DBNull.Value ? "" : res[0]["compiler_args"]),
                        Input = (string)(res[0]["input"] == DBNull.Value ? null : res[0]["input"]),
                        Lang = (LanguagesEnum)Convert.ToInt32(res[0]["lang"]),
                        Editor = (EditorsEnum)Convert.ToInt32(res[0]["editor"]),
                        Guid = (string)res[0]["guid"],
                        WholeError = (string)(res[0]["error"] == DBNull.Value ? "" : res[0]["error"]),
                        Warnings = (string)(res[0]["warning"] == DBNull.Value ? "" : res[0]["warning"]),
                        Status = (res[0]["status"] == DBNull.Value ? GlobalConst.RundotnetStatus.Unknown : Convert.ToInt32(res[0]["status"]).ToRundotnetStatus()),
                        Stats = (string)(res[0]["stats"] == DBNull.Value ? "" : res[0]["stats"]),
                        ShowWarnings = (res[0]["show_warnings"] == DBNull.Value ? false : (bool)res[0]["show_warnings"]),
                        Votes = (res[0]["votes"] == DBNull.Value ? null : (int?)Convert.ToInt32(res[0]["votes"])),
                        Voted = (res[0]["voted"] == DBNull.Value ? null : (bool?)Convert.ToBoolean(res[0]["voted"])),
                        Views = (res[0]["views"] == DBNull.Value ? null : (int?)Convert.ToInt32(res[0]["views"])),
                        IsPrimaryVersion = (res[0]["version_id"] == DBNull.Value ? true : false),
                        PrimaryGuid = (res[0]["primary_guid"] == DBNull.Value ? null : (string)res[0]["primary_guid"]),
                        Date = (DateTime)res[0]["date"],
                        ID = Convert.ToInt32(res[0]["id"])
                    };
                else
                    return null;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return null;
            }
        }

        public static bool Vote(string guid, bool up)
        {
            try
            {
                if (HowOneVoted(guid) == null)
                {
                    DB.DB.Vote(guid, up);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return false;
            }
        }

        public static bool? HowOneVoted(string guid)
        {
            try
            {
                var res = DB.DB.HowVoted(guid);
                if (res.Count != 0)
                    return Convert.ToBoolean(res[0]["voted"]);
                else
                    return null;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return null;
            }
        }

        public static bool CancelVote(string guid)
        {
            try
            {
                bool? up = HowOneVoted(guid);
                if (up != null)
                {
                    DB.DB.CancelVote(guid, (bool)up);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return false;
            }
        }

        public static UserProfile GetUserProfile()
        {
            if (SessionManager.IsUserInSession())
            {
                try
                {
                    var res = DB.DB.Get_User_profile(SessionManager.UserId);
                    if (res.Count != 0)
                        return new UserProfile()
                        {
                            LanguageChoice = res[0]["last_language"] == DBNull.Value ? LanguagesEnum.CSharp : (LanguagesEnum)Convert.ToInt32(res[0]["last_language"]),
                            EditorChoice = res[0]["last_editor"] == DBNull.Value ? EditorsEnum.Codemirror : (EditorsEnum)Convert.ToInt32(res[0]["last_editor"])
                        };
                    else
                        return new UserProfile() { EditorChoice = EditorsEnum.Codemirror, LanguageChoice = LanguagesEnum.CSharp };
                }
                catch (Exception e)
                {
                    Utils.Log.LogInfo(e.Message, "error");
                    return new UserProfile() { EditorChoice = EditorsEnum.Codemirror, LanguageChoice = LanguagesEnum.CSharp };
                }
            }
            else
            {
                return new UserProfile() { EditorChoice = EditorsEnum.Codemirror, LanguageChoice = LanguagesEnum.CSharp };
            }
        }

        public static void SaveUserProfile(LanguagesEnum language_choice, EditorsEnum editor_choice)
        {
            if (SessionManager.IsUserInSession())
            {
                try
                {
                    DB.DB.Save_User_profile(SessionManager.UserId, (int)language_choice, (int)editor_choice);
                }
                catch (Exception e)
                {
                    Utils.Log.LogInfo(e.Message, "error");
                }
            }
        }

        public static LogEntry GetLogEntry(int id)
        {
            return Utils.Utils.db.Table<LogEntry>().Where(f => f.Id == id).SelectEntity().FirstOrDefault();
        }
        public static List<LogEntry> GetLog(int lang, DateTime? from, DateTime? to, string search, int api, out int total)
        {
            var res = Utils.Utils.db.Table<LogEntry>();
            if (lang != 0)
            {
                res.Where(f => f.Lang == lang);
            }

            WorkOnDates(res, from, to);

            if (!string.IsNullOrEmpty(search))
            {
                res.Search(f => f.Data, search).Or().Search(f => f.Result, search).Or().Search(f => f.Input, search);
            }
            if (api == 1)
            {
                res.Where(f => f.Is_api == 1);
            }
            if (api == 2)
            {
                res.Where(f => f.Is_api == 0);
            }
            
            return res.OrderByDescending(f => f.Id).Take(50).SelectEntity(out total);
        }
        static void WorkOnDates(ILinqDbQueryable<LogEntry> res, DateTime? from, DateTime? to)
        {
            if (from != null || to != null)
            {
                if (from == null)
                {
                    var first = Utils.Utils.db.Table<LogEntry>().OrderBy(f => f.Time).Take(1).Select(f => new { Time = f.Time }).FirstOrDefault();
                    from = first == null ? DateTime.Now : first.Time;
                }
                if (to == null)
                {
                    var last = Utils.Utils.db.Table<LogEntry>().OrderByDescending(f => f.Time).Take(1).Select(f => new { Time = f.Time }).FirstOrDefault();
                    to = last == null ? DateTime.Now : last.Time;
                }
                if (((DateTime)to - (DateTime)from).TotalDays < 1)
                {
                    res.Between(f => f.Time, (DateTime)from, (DateTime)to, BetweenBoundaries.BothInclusive);
                }
                else
                {
                    var next = from.Value.AddDays(1);
                    res.Between(f => f.Time, (DateTime)from, new DateTime(next.Year, next.Month, next.Day), BetweenBoundaries.BothInclusive);
                    DateTime d;
                    for (d = new DateTime(next.Year, next.Month, next.Day); d <= to.Value.AddDays(-1); d = d.AddDays(1))
                    {
                        res.Or().Search(f => f.Day_string, d.ToString("yyyyMMdd"));
                    }
                    d = d.AddDays(-1);
                    res.Or().Between(f => f.Time, d, (DateTime)to, BetweenBoundaries.BothInclusive);
                }
            }
        }
        public static void LogRun(string data, string input, string compiler_args, string result, int lang, bool is_api, string log_path)
        {
            try
            {
                try
                {
                    var entry = new LogEntry()
                    {
                        Data = data,
                        Result = result,
                        Input = input,
                        Compiler_args = compiler_args,
                        Lang = lang,
                        Is_api = is_api ? 1 : 0,
                        Time = DateTime.Now,
                        Day_string = DateTime.Now.ToString("yyyyMMdd")
                    };
                    Utils.Utils.db.Table<LogEntry>().Save(entry);
                }
                catch (Exception e)
                {
                    File.WriteAllText(Path.Combine(log_path, "err.txt"), e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine + data+"|"+input+"|"+compiler_args+"|"+result+"|"+lang+"|"+is_api);
                }
                Model.IncrementLangCounter(data, input, compiler_args, result, lang, is_api);
            }
            catch (Exception)
            { }
        }
    }

    
    public class UserProfile
    {
        public LanguagesEnum LanguageChoice
        {
            get;
            set;
        }
        public EditorsEnum EditorChoice
        {
            get;
            set;
        }
    }
    
    public class Code
    {
        public int? UserId
        {
            get;
            set;
        }
        public int ID
        {
            get;
            set;
        }
        public int Wall_ID
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Program
        {
            get;
            set;
        }
        public string Output
        {
            get;
            set;
        }
        public string CompilerArgs
        {
            get;
            set;
        }
        public string Input
        {
            get;
            set;
        }
        public LanguagesEnum Lang
        {
            get;
            set;
        }
        public EditorsEnum Editor
        {
            get;
            set;
        }
        public string Guid
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public string WholeError
        {
            get;
            set;
        }
        public string Warnings
        {
            get;
            set;
        }
        public GlobalConst.RundotnetStatus Status
        {
            get;
            set;
        }
        public string Stats
        {
            get;
            set;
        }
        public bool ShowWarnings
        {
            get;
            set;
        }
        public string VersionToken
        {
            get;
            set;
        }
        public int? Votes
        {
            get;
            set;
        }
        public bool IsOnAWall
        {
            get
            {
                return Votes != null;
            }
        }
        public bool? Voted
        {
            get;
            set;
        }
        public int? Views
        {
            get;
            set;
        }
        public bool IsPrimaryVersion
        {
            get;
            set;
        }
        public string PrimaryGuid
        {
            get;
            set;
        }

        public int? CodeOnWallID
        {
            get;
            set;
        }
    }


    public class Version
    {
        public DateTime DateCreated
        {
            get;
            set;
        }

        public string Author
        {
            get;
            set;
        }

        public string VersionGuid
        {
            get;
            set;
        }

        public int? Wall_Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

    }

    public class LogEntry
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string Result { get; set; }
        public string Input { get; set; }
        public string Compiler_args { get; set; }
        public int Lang { get; set; }
        public int Is_api { get; set; }
        public DateTime Time { get; set; }
        public string Day_string { get; set; }
    }



}