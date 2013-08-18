using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using reExp.Models;
using reExp.Utils;

namespace reExp.Controllers.users
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        public ActionResult Index(UsersWallsData data)
        {
            Compression.SetCompression();
            data.Walls = Model.GetUsersWalls(data.Page);
            data.TotalRecords = Model.GetUserWallsTotal();
            return View(data);
        }

        public ViewResult GetUserWallsCode(UserWallData data)
        {
            Compression.SetCompression();
            data.Name = Model.GetUserWallName(data.Wall_ID);
            data.Codes = Model.GetUsersWallCodes(data.Wall_ID, data.Page, data.Sort);
            data.TotalRecords = Model.GetUserWallCodesTotal(data.Wall_ID);
            data.IsOwner = Model.IsWallsOwner(data.Wall_ID);
            int wall_id;
            if(Int32.TryParse(data.Wall_ID, out wall_id))
                data.IsSubscribed = Model.IsUserSubscribed(wall_id);
            return View("UserWall", data);
        }

        public string RemoveItem(int id)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var res = !Model.DeleteUserWallItem(id);
            return json.Serialize(new JsonData() { Errors = res });
        }

    }

    public class JsonData
    {
        public bool Errors { get; set; }
        public string Error { get; set; }
    }

    public class UserWallData
    {
        public string Wall_ID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public List<Code> Codes
        {
            get;
            set;
        }

        public int Page
        {
            get;
            set;
        }

        public int Sort
        {
            get;
            set;
        }

        public int TotalRecords
        {
            get;
            set;
        }

        public bool IsSubscribed
        {
            get;
            set;
        }

        public bool IsOwner
        {
            get;
            set;
        }
    }

    public class UsersWallsData
    {
        public List<UserWall> Walls
        {
            get;
            set;
        }

        public int Page
        {
            get;
            set;
        }


        public int TotalRecords
        {
            get;
            set;
        }
    }


}
