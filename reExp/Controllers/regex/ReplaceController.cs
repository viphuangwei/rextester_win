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
    public class ReplaceController : Controller
    {
        //
        // GET: /Replace/
        [ValidateInput(false)]
        public ActionResult Index(RegexData data, string savedNr = null)
        {
            Compression.SetCompression();
            data.Describtions = (new OptionDescribtion()).GetDescribtions();

            //retrieve saved regex replace 
            savedNr = savedNr ?? HttpContext.Request.QueryString["code"];
            if (!string.IsNullOrEmpty(savedNr))
            {
                var regexReplace = Model.GetRegexReplace(savedNr);
                data.Pattern = regexReplace.Pattern;
                data.Substitution = regexReplace.Replacement;
                data.Text = regexReplace.Text;
                if (regexReplace.Options != null)
                    data.Options = regexReplace.Options;
                data.Result = regexReplace.Output;
                return View(data);
            }

            return View(data);
        }

        [HttpPost]
        [ValidateInput(false)]
        public string TakeText(RegexData data)
        {
            Compression.SetCompression();
            data.IsReplace = true;
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
            if (string.IsNullOrEmpty(data.Substitution))
                data.Substitution = string.Empty;
            if (string.IsNullOrEmpty(data.Text))
                data.Text = string.Empty;
            string guid = Model.SaveRegexReplace(data);

            SavedItem item = new SavedItem();
            if (!string.IsNullOrEmpty(guid))
                item.Url = Utils.Utils.GetUrl(Utils.Utils.PagesEnum.Replace) + "/" + guid;
            else
                item.Url = "";

            return json.Serialize(item);
        }

    }
}
