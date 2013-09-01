using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;
using System.Text.RegularExpressions;
using System.Threading;
using System.Text;
using reExp.Utils;
using reExp.Controllers.regex;

namespace reExp.Controllers.main
{
    public class MainController : Controller
    {
        //
        // GET: /Main/ 
        public ActionResult Index(MainData data)
        {
            Compression.SetCompression();
            var list = Model.GetLangCounter();
            data.LangCounters = list.Where(f => f.Key.ToLower() != "unknown")
                                    .Where(f => !f.Key.ToLower().EndsWith("_api"))
                                    .ToDictionary(f => f.Key, f => f.Value);
            data.ApiLangCounters = list.Where(f => f.Key.ToLower() != "unknown_api")
                                       .Where(f => f.Key.ToLower().EndsWith("_api"))
                                       .ToDictionary(f => f.Key.Substring(0, f.Key.Length-4), f => f.Value);
            return View(data);
        }

        public ActionResult Faq()
        {
            Compression.SetCompression();
            return View();
        }
    }
}
