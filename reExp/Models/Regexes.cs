using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Controllers.regex;
using reExp.Utils;
using System.Threading.Tasks;

namespace reExp.Models
{
    public partial class Model
    {
        public static string SaveRegex(RegexData data)
        {
            var guid = Utils.Utils.GetGuid();

            try
            {
                string options = "";
                foreach (var o in data.Options)
                    options += o ? "1" : "0";
                DB.DB.Regex_Insert(data.Pattern, data.Text, data.SavedOutput, options, guid, SessionManager.UserId);

                if (SessionManager.IsUserInSession())
                {
                    int uid = (int)SessionManager.UserId;
                    Task.Run(() =>
                    {
                        Search.PutUserItem(new UsersItem()
                        {
                            Date = DateTime.Now,
                            Guid = guid,
                            ID = "regex_" + guid,
                            UserId = uid,
                            Regex = data.Pattern,
                            Text = data.Text
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

        public static Regexpr GetRegex(string guid)
        {
            try
            {
                var res = DB.DB.Regex_Get(guid);
                if (res.Count != 0)
                {
                    List<bool> options = new List<bool>();
                    foreach (var ch in (string)res[0]["options"])
                        options.Add(ch == '1' ? true : false);
                    return new Regexpr()
                    {
                        Pattern = (string)res[0]["regex"],
                        Text = (string)res[0]["text"],
                        Output = (string)(res[0]["output"] == DBNull.Value ? "" : res[0]["output"]),
                        Options = options
                    };
                }
                else
                    return new Regexpr();
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new Regexpr();
            }
        }

        public static string SaveRegexReplace(RegexData data)
        {
            var guid = Utils.Utils.GetGuid();

            try
            {
                string options = "";
                foreach (var o in data.Options)
                    options += o ? "1" : "0";
                DB.DB.Regex_Replace_Insert(data.Pattern, data.Substitution, data.Text, data.SavedOutput, options, guid, SessionManager.UserId);

                if (SessionManager.IsUserInSession())
                {
                    int uid = (int)SessionManager.UserId;
                    Task.Run(() =>
                    {
                        Search.PutUserItem(new UsersItem()
                        {
                            Date = DateTime.Now,
                            Guid = guid,
                            ID = "regex_r_" + guid,
                            UserId = uid,
                            Regex = data.Pattern,
                            Replace = data.Substitution,
                            Text = data.Text
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

        public static RegexReplace GetRegexReplace(string guid)
        {
            try
            {
                var res = DB.DB.Regex_Replace_Get(guid);
                if (res.Count != 0)
                {
                    List<bool> options = new List<bool>();
                    foreach (var ch in (string)res[0]["options"])
                        options.Add(ch == '1' ? true : false);
                    return new RegexReplace()
                    {
                        Pattern = (string)res[0]["regex"],
                        Replacement = (string)res[0]["replacement"],
                        Text = (string)res[0]["text"],
                        Output = (string)(res[0]["output"] == DBNull.Value ? "" : res[0]["output"]),
                        Options = options
                    };
                }
                else
                    return new RegexReplace();
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new RegexReplace();
            }
        }
    }

    public class Regexpr
    {
        public string Pattern
        {
            get;
            set;
        }
        public string Text
        {
            get;
            set;
        }
        public string Output
        {
            get;
            set;
        }
        public List<bool> Options
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public string Guid
        {
            get;
            set;
        }
    }

    public class RegexReplace
    {
        public string Pattern
        {
            get;
            set;
        }
        public string Replacement
        {
            get;
            set;
        }
        public string Text
        {
            get;
            set;
        }
        public string Output
        {
            get;
            set;
        }
        public List<bool> Options
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public string Guid
        {
            get;
            set;
        }
    }
}