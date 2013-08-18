using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Utils;
using System.Web.Script.Serialization;

namespace reExp.Controllers.diff
{
    public class DiffController : Controller
    {
        //
        // GET: /Diff/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Diff(string left, string right)
        {
            int maxLength = 200000;
            Compression.SetCompression();
            JavaScriptSerializer json = new JavaScriptSerializer();

            if (!string.IsNullOrEmpty(left) && left.Length > maxLength)
            {
                return json.Serialize(new JsonData() { IsError = true, Errors = string.Format("Left input is too long (max is {0} characters).\n", maxLength) });
            }

            if (!string.IsNullOrEmpty(right) && right.Length > maxLength)
            {
                return json.Serialize(new JsonData() { IsError = true, Errors = string.Format("Right input is too long (max is {0} characters).\n", maxLength) });
            }

            Service.LinuxService ser = new Service.LinuxService();
            var res = ser.GetDiff(left, right);
            if (res.IsError)
                return json.Serialize(new JsonData() { IsError = true });
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

        public class JsonData
        {
            public string Result
            {
                get;
                set;
            }

            public bool IsError
            {
                get;
                set;
            }

            public string Errors
            {
                get;
                set;
            }
        }

    }
}
