using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Mail;
using System.Net;
using reExp.Utils;

namespace reExp.Controllers.feedback
{
    public class FeedbackController : Controller
    {
        //private static object _lock = new object();

        // GET: /Feedback/
        [ValidateInput(false)]
        public ActionResult Index(Feedback feedback)
        {
            try
            {
                if (string.IsNullOrEmpty(feedback.Message) && string.IsNullOrEmpty(feedback.UserAnswer))
                {
                    feedback.IsResult = false;
                    /*ResetFeedback(feedback);*/
                    return View(feedback);
                }
                int maxLength = 30000;
                feedback.IsResult = true;
                /*
                byte[] b = EncryptionUtils.EncodeDecode(EncryptionUtils.FromUserString(feedback.InfoString));
                string infoString = System.Text.Encoding.Unicode.GetString(b);
                string[] parts = infoString.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                DateTime infoDate = Convert.ToDateTime(parts[0]);
                if (infoDate + TimeSpan.FromSeconds(5) > DateTime.Now)
                {
                    feedback.Succeeded = false;
                    feedback.ErrorMessage = "Form submitted in less than 5 seconds. Please try again slower.";
                    ResetFeedback(feedback);
                    return View(feedback);
                }
                else if (parts[1] != feedback.CaptchaRegex)
                {
                    feedback.Succeeded = false;
                    feedback.ErrorMessage = "Captcha regular expression has changed when it shouldn't.";
                    ResetFeedback(feedback);
                    return View(feedback);
                }
                else */if (string.IsNullOrEmpty(feedback.Message))
                {
                    feedback.Succeeded = false;
                    feedback.ErrorMessage = "Feedback shouldn't be empty.";
                    //ResetFeedback(feedback);
                    return View(feedback);
                }
                else if (feedback.Message.Length > maxLength)
                {
                    feedback.Succeeded = false;
                    feedback.ErrorMessage = string.Format("Feedback shouldn't be longer than {0} characters.", maxLength);
                    //ResetFeedback(feedback);
                    return View(feedback);
                }
                /*
                lock (_lock)
                {
                    if (InfoStrings.GetMe().ContainsInfoString(infoString))
                    {
                        feedback.Succeeded = false;
                        feedback.ErrorMessage = "An attempt to resubmit request detected. Please try again.";
                        ResetFeedback(feedback);
                        return View(feedback);
                    }
                    else
                    {
                        InfoStrings.GetMe().AddInfoString(infoString);
                    }
                }

                Regex rg = new Regex(feedback.CaptchaRegex);
                if (string.IsNullOrEmpty(feedback.UserAnswer) || !rg.IsMatch(feedback.UserAnswer))
                {
                    feedback.Succeeded = false;
                    feedback.ErrorMessage = "Supplied answer didn't match given regular expression. Please try again.";
                    ResetFeedback(feedback);
                    return View(feedback);
                }
                else
                {*/
                    Log.LogInfo(feedback.Message, "FEEDBACK");
                    feedback.Succeeded = true;
                    return View(feedback);
                /*}*/
            }
            catch (Exception e)
            {
                Log.LogInfo(e.Message, "FEEDBACK_ERROR");
                feedback.Succeeded = false;
                feedback.ErrorMessage = "Oops. Something went wrong by our fault. Please try again.";
                //ResetFeedback(feedback);
                return View(feedback);
            }
        }

        //private void ResetFeedback(Feedback feedback)
        //{
        //    string pattern = GenerateRegularExpression();
        //    feedback.CaptchaRegex = pattern;
        //    string info = DateTime.Now.ToString() + "|" + pattern;
        //    feedback.InfoString = EncryptionUtils.ToUserString(EncryptionUtils.EncodeDecode(System.Text.Encoding.Unicode.GetBytes(info)));
        //    feedback.UserAnswer = "";
        //}
        //private string GenerateRegularExpression()
        //{
        //    string regex = "";
        //    List<char> chars = new List<char>();
        //    for (int i = (int)'a'; i <= (int)'z'; i++)
        //        chars.Add((char)i);
        //    Random rg = new Random();
        //    for (int i = 0; i < rg.Next(1, 5); i++)
        //        regex += chars[rg.Next(0, chars.Count)];
        //    if (Happened(0.9, rg))
        //    {
        //        string group = "";
        //        if (Happened(0.5, rg))
        //            group += "^";
        //        int startIndex = rg.Next(chars.Count - 1);
        //        char start = chars[startIndex];
        //        char end = chars[rg.Next(startIndex + 1, rg.Next(startIndex + 2, chars.Count))];
        //        group += start + "-" + end;
        //        regex += "[" + group + "]";
        //    }
        //    if (Happened(0.5, rg))
        //        regex += "{" + rg.Next(2, 5) + "}";
        //    if (Happened(0.5, rg))
        //        regex += @"\s";
        //    if (Happened(0.5, rg))
        //        regex += @"\d";
        //    if (Happened(0.5, rg))
        //    {
        //        for (int i = 0; i < rg.Next(1, 5); i++)
        //            regex += chars[rg.Next(0, chars.Count)];
        //    }
        //    return regex;
        //}

        //private bool Happened(double probability, Random rg)
        //{
        //    if (rg.NextDouble() > probability)
        //        return false;
        //    else
        //        return true;
        //}
    }


    //class InfoStrings
    //    {
    //        public List<string> strings = new List<string>();
    //        static InfoStrings me = null;

    //        private InfoStrings()
    //        { }
    //        public static InfoStrings GetMe()
    //        {
    //            if (me == null)
    //                me = new InfoStrings();

    //            return me;
    //        }
    //        public bool ContainsInfoString(string InfoString)
    //        {
    //            return strings.Contains(InfoString);
    //        }

    //        public void AddInfoString(string InfoString)
    //        {
    //            int max = 500;
    //            if (strings.Count > max)
    //                strings[new Random().Next(0, strings.Count)] = InfoString;
    //            else
    //                strings.Add(InfoString);
    //        }
    //    }
}
