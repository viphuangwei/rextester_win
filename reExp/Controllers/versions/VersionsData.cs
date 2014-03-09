using reExp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reExp.Controllers.versions
{
    public class VersionsData
    {
        public bool IsLive
        {
            get;
            set;
        }
        public string CodeGuid
        {
            get;
            set;
        }
        public Author Author
        {
            get;
            set;
        }
        public DateTime CreationDate
        {
            get;
            set;
        }
        public List<Version> Versions
        {
            get;
            set;
        }
    }

    public class Version
    {
        public DateTime CreationDate
        {
            get;
            set;
        }

        public string Author
        {
            get;
            set;
        }

        public int? Wall_id
        {
            get;
            set;
        }

        public string Guid
        {
            get;
            set;
        }

        public bool LeftChecked
        {
            get;
            set;
        }

        public bool RightChecked
        {
            get;
            set;
        }
    }
}