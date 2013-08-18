using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Utils;
using reExp.Models;
using System.Web.Script.Serialization;

namespace reExp.Controllers.codewall
{
    public class CodeWallController : Controller
    {
        //
        // GET: /CodeWall/

        [HttpGet()]
        public ActionResult Index(CodeWallData data)
        {
            Compression.SetCompression();
            data.Codes = Model.GetWallsCode(data.Page, data.Sort);
            data.TotalRecords = Model.GetWallsTotal();
            data.IsSubscribed = Model.IsUserSubscribed(null);
            return View(data);
        }
        [HttpPost()]
        public string Subscribe(int? wall_id)
        {
            Compression.SetCompression();
            JavaScriptSerializer json = new JavaScriptSerializer();
            if (!SessionManager.IsUserInSession())
                return json.Serialize(new SubscriptionData() { Errors = false, NotLoggedIn = true, Subscribed = false });
            bool? s = Model.Subscribe(wall_id);
            return json.Serialize(new SubscriptionData() { Errors = s == null, NotLoggedIn = false, Subscribed = s});
        }

    }

    public class SubscriptionData
    {
        public bool Errors { get; set; }
        public bool? Subscribed { get; set; }
        public bool NotLoggedIn { get; set; }
    }
}
