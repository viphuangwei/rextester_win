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
            data.LangCounters = Model.GetLangCounter();
            return View(data);
        }

        public ActionResult Faq()
        {
            Compression.SetCompression();
            return View();
        }
    }
}
