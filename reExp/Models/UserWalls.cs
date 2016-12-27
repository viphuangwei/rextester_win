using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Utils;

namespace reExp.Models
{
    public partial class Model
    {
        public static List<UserWall> GetUsersWalls(int page)
        {
            try
            {
                List<UserWall> walls = new List<UserWall>();
                var res = DB.DB.GetUsersWalls(page);
                foreach (var wall in res)
                {
                    walls.Add(new UserWall()
                    {
                        ID = Convert.ToInt32(wall["id"]),
                        Name = (string)wall["name"]
                    });
                }
                return walls;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, e, "error");
                return new List<UserWall>();
            }
        }

        public static int GetUserWallsTotal()
        {
            try
            {
                return Convert.ToInt32(DB.DB.GetUserWallsTotal()[0]["total"]);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, e, "error");
                return 0;
            }
        }


        public static string GetUserWallName(string wall_id)
        {
            try
            {
                int id = Convert.ToInt32(wall_id);
                List<Code> wallsCode = new List<Code>();
                var res = DB.DB.GetWallsName(id);
                return (string)res[0]["name"];
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, e, "error");
                return "";
            }
        }

        public static bool IsWallsOwner(string wall_id)
        {
            try
            {
                if (!SessionManager.IsUserInSession())
                    return false;
                int id = Convert.ToInt32(wall_id);
                var res = DB.DB.GetUserWallUserID(id);
                return Convert.ToInt32(res[0]["user_id"]) == SessionManager.UserId;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, e, "error");
                return false;
            }
        }

        public static bool DeleteUserWallItem(int id)
        {
            if (!SessionManager.IsUserInSession())
                return false;
            return DB.DB.DeleteUserWallItem(id);
        }

        public static List<Code> GetUsersWallCodes(string wall_id, int page, int sort)
        {
            try
            {
                int id = Convert.ToInt32(wall_id);
                List<Code> wallsCode = new List<Code>();
                var res = DB.DB.GetUserWallsCode(id, page, sort);
                foreach (var wcode in res)
                {
                    wallsCode.Add(new Code()
                    {
                        Title = (wcode["title"] == DBNull.Value ? null : (string)wcode["title"]),
                        Program = (string)wcode["code"],
                        Lang = (LanguagesEnum)Convert.ToInt32(wcode["lang"]),
                        Editor = (EditorsEnum)Convert.ToInt32(wcode["editor"]),
                        Guid = (string)wcode["guid"],
                        Date = (DateTime)wcode["date"],
                        Status = (wcode["status"] == DBNull.Value ? GlobalConst.RundotnetStatus.Unknown : Convert.ToInt32(wcode["status"]).ToRundotnetStatus()),
                        Votes = (wcode["votes"] == DBNull.Value ? null : (int?)Convert.ToInt32(wcode["votes"])),
                        Views = (wcode["views"] == DBNull.Value ? null : (int?)Convert.ToInt32(wcode["views"])),
                        CodeOnWallID = Convert.ToInt32(wcode["code_on_wall_id"])
                    });
                }
                return wallsCode;
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, e, "error");
                return new List<Code>();
            }
        }

        public static int GetUserWallCodesTotal(string wall_id)
        {
            try
            {
                int id = Convert.ToInt32(wall_id);
                return Convert.ToInt32(DB.DB.GetUserWallCodesTotal(id)[0]["total"]);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, e, "error");
                return 0;
            }
        }


        public static int? GetUserWallId()
        {
            try
            {
                var res = DB.DB.GetUserWallId();
                if (res.Count == 0)
                    return null;
                return Convert.ToInt32(res[0]["id"]);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, e, "error");
                return 0;
            }
        }



    }

    public class UserWall
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
}