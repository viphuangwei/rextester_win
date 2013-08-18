using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using reExp.Models;
using reExp.Utils;

namespace reExp.Controllers.discussion
{
    public class DiscussionController : Controller
    {
        //
        // GET: /Discussion/
        [HttpGet]
        public ActionResult Index(DiscussionData data)
        {
            Compression.SetCompression();
            bool increment = false;
            if (!Utils.Utils.IsDisqus)
                increment = true;
            var code = Model.GetCode(data.Guid, increment);
            if (!code.IsOnAWall)
                data.Title = "Discussion not available";
            else
                data.Title = code.Title;
            data.Votes = code.Votes;
            data.VoteUp = code.Voted;
            data.ShowComments = code.IsOnAWall;
            data.Code = code.Program;
            data.Date = code.Date;
            data.Language = code.Lang;
            return View(data);
        }

        [HttpPost]
        public string Vote(DiscussionData data)
        {
            Compression.SetCompression();
            JavaScriptSerializer json = new JavaScriptSerializer();

            if (!SessionManager.IsUserInSession())
            {
                return json.Serialize(new VoteData() { NotLoggedIn = true });
            }

            if (!Model.Vote(data.Guid, (bool)data.VoteUp))
                return json.Serialize(new VoteData() { AlreadyVoted = true });

            return json.Serialize(new VoteData());
        }

        [HttpPost]
        public string CancelVote(DiscussionData data)
        {
            Compression.SetCompression();
            JavaScriptSerializer json = new JavaScriptSerializer();

            if (!SessionManager.IsUserInSession())
            {
                return json.Serialize(new VoteData() { NotLoggedIn = true });
            }

            if (!Model.CancelVote(data.Guid))
            {
                return json.Serialize(new VoteData() { AlreadyVoted = true });
            }
            return json.Serialize(new VoteData());
        }

    }
}
