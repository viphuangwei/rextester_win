using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;
using reExp.Utils;
using System.Web.Script.Serialization;

namespace reExp.Controllers.versions
{
    public class VersionsController : Controller
    {
        //
        // GET: /Versions/

        public ActionResult Index(VersionsData data)
        {
            Compression.SetCompression();
            var versions = Model.GetVersions(data.CodeGuid);
            data.Versions = new List<Version>();
            foreach (var v in versions)
                data.Versions.Add(new Version()
                    {
                        Author = v.Author,
                        CreationDate = v.DateCreated,
                        Guid = v.VersionGuid
                    });
            data.IsLive = Model.IsLive(data.CodeGuid);
            data.Author = Model.GetUserByGuid(data.CodeGuid);
            data.CreationDate = Model.GetCode(data.CodeGuid, false).Date;
            return View(data);
        }

        [HttpPost]
        public string GetDiffHtml(string CodeGuid, string LeftGuid, string RightGuid)
        {
            Compression.SetCompression();
            JavaScriptSerializer json = new JavaScriptSerializer();

            var left = Model.GetCode(LeftGuid, false);
            var right = Model.GetCode(RightGuid, false);

            Service.LinuxService ser = new Service.LinuxService();
            var res = ser.GetDiff(left.Program, right.Program);
            if (res.IsError)
                return json.Serialize(new JsonData() { Errors = true });
            else
            {
                if (!string.IsNullOrEmpty(res.Result))
                {
                    int startIndex = res.Result.IndexOf("<table");
                    if (startIndex != -1)
                        res.Result = res.Result.Substring(startIndex);
                    int endIndex = res.Result.LastIndexOf("</body");
                    if (endIndex != -1)
                        res.Result = res.Result.Substring(0, endIndex);
                }
                return json.Serialize(new JsonData() { Result = res.Result });
            }
        }

    }

    public class JsonData
    {
        public bool Errors
        {
            get;
            set;
        }

        public string Result
        {
            get;
            set;
        }
    }
}
