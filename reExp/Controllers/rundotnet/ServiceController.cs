using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using reExp.Utils;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using reExp.Controllers.rundotnet.autocomplete;

namespace reExp.Controllers.rundotnet
{
    public class ServiceController : Controller
    {
        //
        // GET: /Service/
        [ValidateInput(false)]
        public string Codecompletion(string code, int position, int language, int line, int ch)
        {
            Compression.SetCompression();
            JavaScriptSerializer json = new JavaScriptSerializer();

            if (string.IsNullOrEmpty(code))
                return json.Serialize(new List<string>());

            if (language == (int)LanguagesEnum.CSharp)
            {
                return CsharpComplete.Complete(code, position, line, ch);
            }
            else if (language == (int)LanguagesEnum.Java)
            {
                return JavaComplete.Complete(code, position, line, ch);
            }
            else if (language == (int)LanguagesEnum.CPP || language == (int)LanguagesEnum.CPPClang || language == (int)LanguagesEnum.VCPP)
            {
                return VcppComplete.Complete(code, position, line, ch);
            }
            else if (language == (int)LanguagesEnum.Python)
            {
                return PythonComplete.Complete(code, position, line, ch);
            }
            else
            {
                return json.Serialize(new List<string>());
            }            
        }

    }
}
