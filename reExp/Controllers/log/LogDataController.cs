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
                int total = 0;
                if (data.View == 0)
                {
                    data.Entries = Model.GetLog(data.lang, data.from, data.to, data.search, data.api, data.Date_range, out total);
                }
                else
                {
                    if (data.lang == 0)
                    {
                        data.Languages_runs = Model.GetLogStats(data.from, data.to, data.search, data.api, data.Date_range, out total);
                    }
                    else
                    {
                        data.Language_runs = Model.GetLangLogStats(data.lang, data.from, data.to, data.search, data.api, data.Date_range);
                    }
                }
                data.Total = total;
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
        public int Total { get; set; }
        public int api { get; set; }
        public int Date_range { get; set; }
        public int View { get; set; }
        public Dictionary<string, KeyValuePair<int, int>> Languages_runs { get; set;}
        public Dictionary<string, KeyValuePair<int, int>> Language_runs { get; set; }
    }
}