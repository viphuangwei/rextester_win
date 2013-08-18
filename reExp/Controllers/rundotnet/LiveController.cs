using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Utils;
using System.Web.Script.Serialization;
using reExp.Models;

namespace reExp.Controllers.rundotnet
{
    public class LiveController : Controller
    {
        [HttpPost]
        [ValidateInput(false)]
        public string UserStats(RundotnetData data)
        {
            Compression.SetCompression();
            JavaScriptSerializer json = new JavaScriptSerializer();

            int count = Model.LiveUsersCount(data.LiveGuid, data.LiveUserToken);
            return json.Serialize(new LiveDataViewModel() { Users_count = count });
        }
    }
    class LiveDataViewModel
    {
        public string Code
        {
            get;
            set;
        }
        public string Version_token
        {
            get;
            set;
        }
        public int Users_count
        {
            get;
            set;
        }
    }
}
