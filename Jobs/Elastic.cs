using GlobalUtils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Jobs
{
    public class Elastic
    {
        public static void PutUserItem(UsersItem item)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = Int32.MaxValue;
            string data = ser.Serialize(item);
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.UploadString(TopSecret.ElasticUrl + item.UserId + "/" + item.ID, "PUT", data);
            }
        }
        public void Do()
        {
            {
                var s = @"select u.id as user_id, u.name, c.code, c.guid, c.title, c.lang, c.date, uc.type
                          from userscode uc
                                inner join code c on uc.code_id = c.id
                                inner join users u on c.user_id = u.id";

                List<UsersItem> wallsCode = new List<UsersItem>();
                var res = DB.ExecuteQuery(s, new List<SQLiteParameter>());
                foreach (var wcode in res)
                {

                    wallsCode.Add(new UsersItem()
                    {
                        Title = (wcode["title"] == DBNull.Value ? null : (string)wcode["title"]),
                        Code = (string)wcode["code"],
                        Lang = Helpers.ToLanguage((Helpers.LanguagesEnum)Convert.ToInt32(wcode["lang"])),
                        Guid = (string)wcode["guid"],
                        UserId = Convert.ToInt32(wcode["user_id"]),
                        ID = "code_" + (string)wcode["guid"],
                        Date = Convert.ToDateTime(wcode["date"]),
                        IsLive = (wcode["type"]+"") == "3"
                    });
                }

                foreach (var r in wallsCode)
                {
                    PutUserItem(r);
                }
            }

            {
                var s = @"select u.id as user_id, u.name, r.regex, r.text, r.guid, r.date
                      from regex r
                            inner join users u on r.user_id = u.id";

                List<UsersItem> wallsCode = new List<UsersItem>();
                var res = DB.ExecuteQuery(s, new List<SQLiteParameter>());
                foreach (var wcode in res)
                {
                    wallsCode.Add(new UsersItem()
                    {
                        Regex = (wcode["regex"] == DBNull.Value ? null : (string)wcode["regex"]),
                        Text = (wcode["text"] == DBNull.Value ? null : (string)wcode["text"]),
                        Guid = (string)wcode["guid"],
                        UserId = Convert.ToInt32(wcode["user_id"]),
                        ID = "regex_" + (string)wcode["guid"],
                        Date = Convert.ToDateTime(wcode["date"])
                    });
                }
                
                foreach (var r in wallsCode)
                {
                    PutUserItem(r);
                }

            }

            {
                var s = @"select u.id as user_id, u.name, r.regex, r.replacement, r.text, r.guid, r.date
                          from regexreplace r
                                inner join users u on r.user_id = u.id";

                List<UsersItem> wallsCode = new List<UsersItem>();
                var res = DB.ExecuteQuery(s, new List<SQLiteParameter>());
                foreach (var wcode in res)
                {
                    wallsCode.Add(new UsersItem()
                    {
                        Regex = (wcode["regex"] == DBNull.Value ? null : (string)wcode["regex"]),
                        Replace = (wcode["replacement"] == DBNull.Value ? null : (string)wcode["replacement"]),
                        Text = (wcode["text"] == DBNull.Value ? null : (string)wcode["text"]),
                        Guid = (string)wcode["guid"],
                        UserId = Convert.ToInt32(wcode["user_id"]),
                        ID = "regex_r_" + (string)wcode["guid"],
                        Date = Convert.ToDateTime(wcode["date"])
                    });
                }

                foreach (var r in wallsCode)
                {
                    PutUserItem(r);
                }

            }
        }
    }

    public class UsersItem
    {
        public string ID { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Lang { get; set; }
        public string Guid { get; set; }
        public string Regex { get; set; }
        public string Replace { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool? IsLive { get; set; }
    }
}
