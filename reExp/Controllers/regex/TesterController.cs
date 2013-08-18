using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;
using reExp.Utils;
using System.Web.Script.Serialization;

namespace reExp.Controllers.regex
{
    public class TesterController : Controller
    {
        //
        // GET: /Tester/

        [ValidateInput(false)]
        public ActionResult Index(RegexData data, string savedNr = null)
        {
            Compression.SetCompression();
            data.Describtions = (new OptionDescribtion()).GetDescribtions();
            data.IsReplace = false;

            //retrieve saved regex
            savedNr = savedNr ?? HttpContext.Request.QueryString["code"];
            if (!string.IsNullOrEmpty(savedNr))
            {
                var regex = Model.GetRegex(savedNr);
                data.Pattern = regex.Pattern;
                data.Text = regex.Text;
                if (regex.Options != null)
                    data.Options = regex.Options;
                data.Result = regex.Output;
                return View(data);
            }
            
            return View(data);
        }

        [HttpPost]
        [ValidateInput(false)]
        public string TakeText(RegexData data)
        {
            Compression.SetCompression();
            data.IsReplace = false;
            return Logic.TakeText(data);
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Save(RegexData data)
        {
            Compression.SetCompression();
            JavaScriptSerializer json = new JavaScriptSerializer();

            if (string.IsNullOrEmpty(data.Pattern))
                data.Pattern = string.Empty;
            if (string.IsNullOrEmpty(data.Text))
                data.Text = string.Empty;
            string guid = Model.SaveRegex(data);

            SavedItem item = new SavedItem();
            if (!string.IsNullOrEmpty(guid))
                item.Url = Utils.Utils.GetUrl(Utils.Utils.PagesEnum.Tester)+"/"+guid;
            else
                item.Url = "";

            return json.Serialize(item);            
        }
    }
}
