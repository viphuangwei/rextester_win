using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Utils;

namespace reExp.Models
{
    public partial class Model
    {
        public static List<SavedItem> GetUsersItems(int userId, int page)
        {
            try
            {
                List<SavedItem> items = new List<SavedItem>();
                var res = DB.DB.GetUsersItems(userId, page);
                foreach (var item in res)
                {
                    items.Add(new SavedItem()
                    {
                        ID = Convert.ToInt32(item["id"]),
                        Type = Convert.ToInt32(item["type"]),
                        Title = (item["title"] == DBNull.Value ? "" : (string)(item["title"])),
                        Program = (item["code"] == DBNull.Value ? "" : (string)(item["code"])),
                        Status = (item["status"] == DBNull.Value ? GlobalConst.RundotnetStatus.Unknown : Convert.ToInt32(item["status"]).ToRundotnetStatus()),
                        Lang = (LanguagesEnum)(item["lang"] == DBNull.Value ? (int)LanguagesEnum.Unknown : Convert.ToInt32(item["lang"])),
                        Guid = (string)item["guid"],
                        Date = Convert.ToDateTime(item["date"]),
                        Regex = (item["regex"] == DBNull.Value ? "" : (string)(item["regex"])),
                        IsWall = (item["is_wall"] == DBNull.Value ? false : true),
                        IsPersonalWall = (item["is_personal_wall"] == DBNull.Value ? false : true),
                        IsLive = (item["is_live"] == DBNull.Value ? false : true)
                    });
                }
                return items;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new List<SavedItem>();
            }
        }

        public static bool DeleteUserItem(int id)
        {
            return DB.DB.DeleteUserItem(id);
        }
        public static int GetUsersTotal(int userId)
        {
            try
            {
                return Convert.ToInt32(DB.DB.GetUsersTotal(userId)[0]["total"]);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return 0;
            }
        }

        public static Registering RegisterUser(string name, string password)
        {
            try
            {
                var res = DB.DB.GetUser(name);
                if (res.Count != 0)
                    return new Registering() { NameTaken = true };

                string hashedPass = Utils.EncryptionUtils.CreateMD5Hash(password);
                DB.DB.InsertUser(name, hashedPass, null);

                var login = LoginUser(name, password);
                if (login.BadPassword || login.NoSuchUser || !string.IsNullOrEmpty(login.Error))
                    return new Registering() { Error = "Registration successful but login is not." };

                return new Registering();
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new Registering() { Error = e.Message };
            }
        }

        public static Registering RegisterGoogleUser(string name, string emailHash)
        {
            try
            {
                var res = DB.DB.GetUser(name);
                if (res.Count != 0)
                    return new Registering() { NameTaken = true };

                DB.DB.InsertUser(name, null, emailHash);

                return new Registering();
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new Registering() { Error = e.Message };
            }
        }
        
        public static User LoginUser(string name, string password)
        {
            try
            {
                string hashedPass = Utils.EncryptionUtils.CreateMD5Hash(password);
                var res = DB.DB.GetUser(name);
                if (res.Count == 0)
                    return new User() { NoSuchUser = true };

                if ((string)res[0]["password"] != hashedPass)
                    return new User() { BadPassword = true };

                //set data to session
                SessionManager.SetAuthentication(name);
                return new User() { Name = name };
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new User() { Error = e.Message };
            }
        }
        
        public static string IsGoogleAccountCreated(string emailHash)
        {
            try
            {
                var res = DB.DB.GoogleEmailExists(emailHash);
                if (res.Count > 0)
                    return (string)res[0]["name"];
                else
                    return null;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return null;
            }
        }

        public static int? GetCurrentUserId(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var res = DB.DB.GetUser(name);
            if (res.Count == 0)
                return null;

            return Convert.ToInt32(res[0]["id"]);
        }

        public static string GetUserByGuid(string guid)
        {
            try
            {
                var res = DB.DB.GetUserByGuid(guid);
                if (res.Count == 0)
                    return null;
                return res[0]["name"] == DBNull.Value ? null : (string)res[0]["name"];
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return null;
            }
        }
    }

    public class SavedItem
    {
        public int ID
        {
            get;
            set;
        }
        public int Type
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
        public GlobalConst.RundotnetStatus Status
        {
            get;
            set;
        }
        public LanguagesEnum Lang
        {
            get;
            set;
        }
        public string Regex
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
        public bool IsLive
        {
            get;
            set;
        }
        public bool IsWall
        {
            get;
            set;
        }
        public bool IsPersonalWall
        {
            get;
            set;
        }
    }

    public class Registering
    {
        public bool NameTaken
        {
            get;
            set;
        }
        public string Error
        {
            get;
            set;
        }
    }

    public class User
    {
        public string Name
        {
            get;
            set;
        }
        public bool NoSuchUser
        {
            get;
            set;
        }
        public bool BadPassword
        {
            get;
            set;
        }
        public string Error
        {
            get;
            set;
        }
    }

}