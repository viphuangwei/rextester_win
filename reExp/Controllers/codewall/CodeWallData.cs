using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Models;

namespace reExp.Controllers.codewall
{
    public class CodeWallData
    {
        public List<Code> Codes
        {
            get;
            set;
        }

        public int Page
        {
            get;
            set;
        }

        public int Sort
        {
            get;
            set;
        }

        public int TotalRecords
        {
            get;
            set;
        }

        public bool IsSubscribed
        {
            get;
            set;
        }
    }
}