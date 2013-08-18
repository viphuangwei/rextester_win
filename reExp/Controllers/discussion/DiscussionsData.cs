using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Utils;

namespace reExp.Controllers.discussion
{
    public class DiscussionData
    {
        public string Guid { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public LanguagesEnum Language { get; set; }
        public bool ShowComments { get; set; }
        public int? Votes { get; set; }
        public bool? VoteUp { get; set; }
    }

    class VoteData
    {
        public bool NotLoggedIn { get; set; }
        public bool AlreadyVoted { get; set; }
    }
}