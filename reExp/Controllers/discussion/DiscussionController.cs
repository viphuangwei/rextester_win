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
        //[HttpGet]
        [ValidateInput(false)]
        public ActionResult Index(DiscussionData data)
        {
            Compression.SetCompression();
            var code = Model.GetCode(data.Guid, true);
            if (code == null)
            {
                throw new HttpException(404, "not found");
            }
            
            if (!string.IsNullOrEmpty(data.NewComment))
            {
                if (!SessionManager.IsUserInSession())
                {
                    return this.Redirect(Utils.Utils.BaseUrl + @"login");
                }
                Model.SaveComment(new Comment()
                    {
                        User_Id = (int)SessionManager.UserId,
                        Text = data.NewComment,
                        Code_Id = code.ID
                    });
                return this.Redirect(Utils.Utils.BaseUrl + @"discussion/" + code.Guid + "#comment_" + Model.Comment_Last_Id(code.ID));
            }


            if (!code.IsOnAWall)
                data.Title = "Discussion not available";
            else
                data.Title = code.Title;

            data.Related = Model.GeRelated(code.ID);            
            data.Votes = code.Votes;
            data.VoteUp = code.Voted;
            data.ShowComments = code.IsOnAWall;
            data.Code = code.Program;
            data.Date = code.Date;
            data.Language = code.Lang;

            data.Comments = Model.GetComments(data.Guid);

            var md = new MarkdownDeep.Markdown();

            md.ExtraMode = true;
            md.SafeMode = true;
            foreach (var c in data.Comments)
            {
                c.Text = md.Transform(c.Text);
            }
            return View(data);
        }

        [ValidateInput(false)]
        public ActionResult EditComment(EditData data)
        {
            Compression.SetCompression();
            var com = Model.GetComment((int)data.Comment_ID);
            if (!SessionManager.IsUserInSession() || SessionManager.UserId != com.User_Id)
            {
                return this.Redirect(Utils.Utils.BaseUrl + @"login");
            }
            if (data.IsEdit)
            {
                data.Text = com.Text;
                return View("EditComment", data);
            }
            else
            {
                var code = Model.GetCode(data.Guid);
                Model.UpdateComment(new Comment()
                {
                    Id = (int)data.Comment_ID,
                    User_Id = (int)SessionManager.UserId,
                    Text = data.Text
                });
                return this.Redirect(Utils.Utils.BaseUrl + @"discussion/" + code.Guid + "#comment_"+data.Comment_ID);
            }
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
