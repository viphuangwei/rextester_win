using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reExp.Controllers.main
{
    public class MainData
    {
        public Dictionary<string, long> LangCounters
        {
            get;
            set;
        }
        public Dictionary<string, long> ApiLangCounters
        {
            get;
            set;
        }
    }
}