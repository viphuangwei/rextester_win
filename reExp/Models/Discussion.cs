using reExp.Controllers.discussion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reExp.Models
{
    public partial class Model
    {
        public static List<Comment> GetComments(string guid)
        {
            try
            {
                List<Comment> coms = new List<Comment>();
                var res = DB.DB.Comments_Get(guid);
                return res.Select(c => new Comment
                            {
                                Id = Convert.ToInt32(c["id"]),
                                Code_Id = Convert.ToInt32(c["code_id"]),
                                User_Id = Convert.ToInt32(c["user_id"]),
                                User_Name = (string)c["user_name"],
                                Text = (string)c["text"],
                                Edited_Date = c["edited_date"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(c["edited_date"]),
                                Date = Convert.ToDateTime(c["date"]),
                                Guid = (string)c["guid"]
                            })
                            .ToList();
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new List<Comment>();
            }
        }

        public static Comment GetComment(int Comment_ID)
        {
            try
            {
                var res = DB.DB.Comment_Get(Comment_ID);
                return res.Select(c => new Comment
                {
                    Id = Convert.ToInt32(c["id"]),
                    Code_Id = Convert.ToInt32(c["code_id"]),
                    User_Id = Convert.ToInt32(c["user_id"]),
                    User_Name = (string)c["user_name"],
                    Text = (string)c["text"],
                    Edited_Date = c["edited_date"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(c["edited_date"]),
                    Date = Convert.ToDateTime(c["date"])
                }).FirstOrDefault();
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return null;
            }
        }

        public static void SaveComment(Comment comment)
        {
            try
            {
                DB.DB.Comments_Insert(comment.Code_Id, comment.User_Id, comment.Text);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
            }
        }


        public static int? Comment_Last_Id(int code_id)
        {
            try
            {
                var res = DB.DB.Comment_Last_Id(code_id);
                return Convert.ToInt32(res[0]["id"]);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return 0;
            }
        }

        public static void UpdateComment(Comment comment)
        {
            try
            {
                DB.DB.Comments_Update(comment.Id, comment.Text);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
            }
        }
        public static void DeleteComment(Comment comment)
        {
            try
            {
                DB.DB.Comments_Delete(comment.Id);
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
            }
        }

        public static List<RelatedEntry> GeRelated(int code_id)
        {
            try
            {
                var res = DB.DB.GetRelated(code_id);
                return res.Select(c => new RelatedEntry()
                {
                    Guid = (string)c["guid"],
                    Title = (string)c["title"]
                })
                .ToList();
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "error");
                return new List<RelatedEntry>();
            }
        }
    }

    public class Comment
    {
        public int Id { get; set; }
        public int Code_Id { get; set; }
        public string Guid { get; set; }
        public int User_Id { get; set; }
        public string User_Name { get; set; }
        public string Text { get; set; }
        public DateTime? Edited_Date { get; set; }
        public DateTime Date { get; set; }
    }
}