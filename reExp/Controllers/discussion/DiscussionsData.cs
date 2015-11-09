using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Utils;
using reExp.Models;

namespace reExp.Controllers.discussion
{
    public class DiscussionData
    {
        public string Guid { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public int? UserId { get; set; }

        public LanguagesEnum Language { get; set; }
        public bool ShowComments { get; set; }
        public int? Votes { get; set; }
        public bool? VoteUp { get; set; }
        public List<Comment> Comments { get; set; }
        public string NewComment { get; set; }
        public List<RelatedEntry> Related { get; set; }
    }

    class VoteData
    {
        public bool NotLoggedIn { get; set; }
        public bool AlreadyVoted { get; set; }
    }

    public class EditData
    {
        public bool IsEdit { get; set; }
        public int? Comment_ID { get; set; }
        public string Text { get; set; }
        public string Guid { get; set; }
    }

    public class RelatedEntry
    {
        public string Guid { get; set; }
        public string Title { get; set; }
    }
}