using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Utils;

namespace reExp.Models
{
    public partial class Model
    {
        public static List<Code> GetWallsCode(int page, int sort)
        {
            try
            {
                List<Code> wallsCode = new List<Code>();
                var res = DB.DB.GetWallsCode(page, sort);
                foreach (var wcode in res)
                {
                    wallsCode.Add(new Code()
                    {
                        Wall_ID = Convert.ToInt32(wcode["wall_id"]),
                        Title = (wcode["title"] == DBNull.Value ? null : (string)wcode["title"]),
                        Program = (string)wcode["code"],
                        Lang = (LanguagesEnum)Convert.ToInt32(wcode["lang"]),
                        Editor = (EditorsEnum)Convert.ToInt32(wcode["editor"]),
                        Guid = (string)wcode["guid"],
                        Date = (DateTime)wcode["date"],
                        Status = (wcode["status"] == DBNull.Value ? GlobalConst.RundotnetStatus.Unknown : Convert.ToInt32(wcode["status"]).ToRundotnetStatus()),
                        Votes = (wcode["votes"] == DBNull.Value ? null : (int?)Convert.ToInt32(wcode["votes"])),
                        Views = (wcode["views"] == DBNull.Value ? null : (int?)Convert.ToInt32(wcode["views"])),
                    });
                }
                return wallsCode;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new List<Code>();
            }
        }

        public static int GetWallsTotal()
        {
            try
            {
                return Convert.ToInt32(DB.DB.GetWallsTotal()[0]["total"]);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return 0;
            }
        }

        public static bool IsUserSubscribed(int? wall_id)
        {
            try
            {
                if (!SessionManager.IsUserInSession())
                    return false;
                else
                    return Convert.ToBoolean(DB.DB.IsUserSubscribed(wall_id)[0]["subscribed"]);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return false;
            }
        }

        public static bool? Subscribe(int? wall_id)
        {
            try
            {
                bool is_subscribed = IsUserSubscribed(wall_id);
                DB.DB.Subscribe(wall_id, is_subscribed);
                return !is_subscribed;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return null;
            }
        }

        public static bool DeleteCodeWallItem(int id)
        {
            try
            {
                DB.DB.DeleteCodeWallItem(id);
                return true;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error while removing codewall item");
                return false;
            }
        }
    }
}