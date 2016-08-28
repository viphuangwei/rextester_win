using reExp.Models;
using reExp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace reExp.Controllers.log
{
    public class LogDataController : Controller
    {
        [ValidateInput(false)]
        public ActionResult Index(LogModel data)
        {
            Compression.SetCompression();
            if (!SessionManager.IsUserInSession() || !SessionManager.IsAdmin)
            {
                data.Entries = new List<LogEntry>();
                return View(data);
            }
            if (data.id != 0)
            {
                var r = Model.GetLogEntry(data.id);
                data.Entries = new List<LogEntry>() { r };
            }
            else
            {
                data.Entries = Model.GetLog(data.lang, data.from, data.to, data.search);
            }
            return View(data);
        }
    }

    public class LogModel
    {
        public int id { get; set; }
        public int lang { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public string search { get; set; }
        public List<LogEntry> Entries { get; set; }
    }
}