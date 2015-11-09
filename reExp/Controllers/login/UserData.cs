using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Models;
namespace reExp.Controllers.login
{
    public class UserData
    {
        public string UserName
        {
            get;
            set;
        }
        public List<SavedItem> Items
        {
            get;
            set;
        }

        public bool IsError
        {
            get;
            set;
        }
        public string Error
        {
            get;
            set;
        }
        public int TotalRecords
        {
            get;
            set;
        }
        public int CurrentPage
        {
            get;
            set;
        }
        public int? Wall_ID
        {
            get;
            set;
        }
        public string Query
        {
            get;
            set;
        }
    }
}