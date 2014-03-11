using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;
using reExp.Utils;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace reExp.Controllers.login
{
    public class LoginController : Controller
    {
        public ViewResult Notifications(NotificationsData data)
        {
            Compression.SetCompression();
            data.Notifications = Model.GetNotifications();
            data.Subscriptions = Model.GetSubscriptions();
            data.Wall_ID = Model.GetUserWallId();
            return View("Notifications", data);
        }

        public string GetNotificationsCount()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(new { Total = Model.GetNotificationsCount() });
        }

        //
        // GET: /Login/
        [ValidateInput(false)]
        public ActionResult Index(LoginData data)
        {
            Compression.SetCompression();
            data.IsError = false;
            data.Error = null;

            if ((string.IsNullOrEmpty(data.Name) || string.IsNullOrEmpty(data.Password)) &&
               (string.IsNullOrEmpty(data.RegName) || string.IsNullOrEmpty(data.RegPassword)))
            return View(data);

            

            if (!string.IsNullOrEmpty(data.Name) && !string.IsNullOrEmpty(data.Password))
            {
                var res = Model.LoginUser(data.Name, data.Password);
                if (res.NoSuchUser)
                {
                    data.IsError = true;
                    data.Error = "No such user name.";
                    return View(data);
                }
                if (res.BadPassword)
                {
                    data.IsError = true;
                    data.Error = "Incorrect password.";
                    return View(data);
                }
                if (!string.IsNullOrEmpty(res.Error))
                {
                    data.IsError = true;
                    data.Error = res.Error;
                    return View(data);
                }

                if (!string.IsNullOrEmpty(data.redirectInfo))
                    return this.Redirect(Utils.Utils.BaseUrl + data.redirectInfo);
                else
                    return this.RedirectToAction("UsersStuff");
            }

            if (!string.IsNullOrEmpty(data.RegName) && !string.IsNullOrEmpty(data.RegPassword))
            {
                if (data.RegName.Length > 100 || data.RegPassword.Length > 100)
                {
                    data.IsError = true;
                    data.Error = "User name and password should be shorter than 100 characters.";
                    return View(data);
                }
                var res = Model.RegisterUser(data.RegName, data.RegPassword);
                if (res.NameTaken)
                {
                    data.IsError = true;
                    data.Error = "This name is already taken.";
                    return View(data);
                }
                if (!string.IsNullOrEmpty(res.Error))
                {
                    data.IsError = true;
                    data.Error = res.Error;
                    return View(data);
                }

                if (!string.IsNullOrEmpty(data.redirectInfo))
                    return this.Redirect(Utils.Utils.BaseUrl + data.redirectInfo);
                else
                    return this.RedirectToAction("UsersStuff");
            }

            return View(data);
        }

        public ActionResult UsersStuff(int page=0, string Query = null)
        {
            Compression.SetCompression();
            UserData data = new UserData();
            if(!SessionManager.IsUserInSession())
            {
                data.Error = "Not logged in.";
                data.IsError = true;
                return View(data);
            }
            data.UserName = SessionManager.UserName;
            int userId = (int)SessionManager.UserId;
            if (string.IsNullOrEmpty(Query))
            {
                data.Items = Model.GetUsersItems(userId, page);
                data.CurrentPage = page;
                data.TotalRecords = Model.GetUsersTotal(userId);
            }
            else
            {
                data.Query = Query;
                data.Items = Search.MakeSearch(Query, (int)SessionManager.UserId)
                                   .Select(f => new SavedItem() 
                                   { 
                                       Date = f.Date,
                                       Guid = f.Guid,
                                       Program = f.Code,
                                       Regex = f.Regex,
                                       Title = f.Title,
                                       Type = f.ID.StartsWith("code") ? 1 : (f.ID.StartsWith("regex_r") ? 3 : 2),
                                       Lang = f.Lang.ToLanguageEnum(),
                                       IsLive = f.IsLive ?? false
                                   })
                                   .ToList();
                data.CurrentPage = 0;
                data.TotalRecords = 10;
            }

            data.Wall_ID = Model.GetUserWallId();
            return View(data);
        }
        
        public string RemoveItem(int id)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            var res = !Model.DeleteUserItem(id);

            return json.Serialize(new JsonData() { Errors = res });
        }

        public ActionResult Logout()
        {
            SessionManager.LogOut();
            return this.RedirectToAction("Index");
        }


        public ActionResult oauth2callback(GoogleResult result)
        {
            try
            {
                if (!string.IsNullOrEmpty(result.error))
                {
                    return View("Index", new LoginData() {IsError = true, Error = string.Format("Error occured ({0}). Try again later.", result.error), redirectInfo=result.state });
                }

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://accounts.google.com/o/oauth2/token");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                using (TextWriter tw = new StreamWriter(request.GetRequestStream()))
                {
                    tw.Write(string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type={4}",
                             result.code, GlobalUtils.TopSecret.Google_client_id, GlobalUtils.TopSecret.Google_client_secret, GlobalUtils.TopSecret.Google_callback_url, "authorization_code"));
                }
                string json = null;
                using (TextReader tr = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    json = tr.ReadToEnd();
                }

                JavaScriptSerializer js = new JavaScriptSerializer();
                Dictionary<string, object> res = (Dictionary<string, object>)js.Deserialize(json, typeof(Dictionary<string, object>));

                GoogleResult gr = new GoogleResult()
                {
                    access_token = (string)res["access_token"],
                    id_token = (string)res["id_token"],
                    expires_in = (int)res["expires_in"],
                    token_type = (string)res["token_type"]
                };

                request = (HttpWebRequest)HttpWebRequest.Create(string.Format("https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}", gr.access_token));
                request.Method = "GET";
                using (TextReader tr = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    json = tr.ReadToEnd();
                }
                res = (Dictionary<string, object>)js.Deserialize(json, typeof(Dictionary<string, object>));
                gr.email = (string)res["email"];

                var hash = Utils.EncryptionUtils.CreateMD5Hash(gr.email);
                var name = Model.IsGoogleAccountCreated(hash);

                if (!string.IsNullOrEmpty(name))
                {
                    SessionManager.SetAuthentication(name);
                    if (!string.IsNullOrEmpty(result.state))
                        return this.Redirect(Utils.Utils.BaseUrl + result.state);
                    else
                        return this.RedirectToAction("UsersStuff");
                }
                else
                {
                    return View("CreateGoogleUser", new GoogleUser() { EmailHash = hash, redirectInfo=result.state });
                }
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message, "google login error");
                return View("Index", new LoginData() { IsError=true, Error = "Error occured. Try again later.", redirectInfo=result.state });
            }
        }

        public ActionResult CreateGoogleUser(GoogleUser user)
        {
            if (string.IsNullOrEmpty(user.Name))
                return View(user);
            if (user.Name.Length > 100)
            {
                user.Error = "Name for display should be shorter than 100 characters.";
                return View(user);
            }
            var res = Model.RegisterGoogleUser(user.Name, user.EmailHash);
            if (!string.IsNullOrEmpty(res.Error))
            {
                user.Error = "Error occured. Try again later.";
                return View(user);
            }
            if (res.NameTaken)
            {
                user.Error = "This name is already taken.";
                return View(user);
            }

            SessionManager.SetAuthentication(user.Name);
            if (!string.IsNullOrEmpty(user.redirectInfo))
                return this.Redirect(Utils.Utils.BaseUrl + user.redirectInfo);
            else
                return this.RedirectToAction("UsersStuff");
        }
    }

    public class NotificationsData
    {
        public List<Notification> Notifications { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public int? Wall_ID { get; set; }
    }

    public class GoogleUser
    {
        public string EmailHash
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Error
        {
            get;
            set;
        }
        public string redirectInfo
        {
            get;
            set;
        }
    }
    public class GoogleResult
    {
        public string email
        {
            get;
            set;
        }
        public string access_token
        {
            get;
            set;
        }
        public string state
        {
            get;
            set;
        }
        public string refresh_token
        {
            get;
            set;
        }
        public string id_token
        {
            get;
            set;
        }
        public string code
        {
            get;
            set;
        }
        public int expires_in
        {
            get;
            set;
        }
        public string token_type
        {
            get;
            set;
        }
        public string error
        {
            get;
            set;
        }
    }

    public class JsonData
    {
        public bool Errors { get; set; }
        public string Error { get; set; }
    }
}
