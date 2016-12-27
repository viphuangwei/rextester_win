using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Net;
using reExp.Models;
using System.Threading;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Configuration;
using LinqDb;
using System.Web.Hosting;

namespace reExp.Utils
{
    public class Utils
    {
        private static readonly object globalLock = new object();
        private static readonly Random globalRandom = new Random();

        public static bool IsMobile
        {
            get
            {
                return System.Web.HttpContext.Current.Request.Browser.IsMobileDevice;
                //return true;
            }
        }
        public static bool IsIE
        {
            get
            {
                try
                {
                    return System.Web.HttpContext.Current.Request.Browser.Type.ToUpper().Contains("IE");
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool IsOpera
        {
            get
            {
                try
                {
                    return System.Web.HttpContext.Current.Request.Browser.Type.ToUpper().Contains("OPERA");
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool IsFirefox
        {
            get
            {
                try
                {
                    return System.Web.HttpContext.Current.Request.Browser.Type.ToUpper().Contains("FIREFOX");
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool IsChrome
        {
            get
            {
                try
                {
                    return System.Web.HttpContext.Current.Request.Browser.Type.ToUpper().Contains("CHROME");
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool IsBrowser
        {
            get
            {
                return IsChrome || IsFirefox || IsIE || IsOpera;
            }
        }


        public static Random GetTrulyRandom()
        {
            lock (globalLock)
            {
                return new Random(globalRandom.Next());
            }
        }
        public static string RootFolder
        {
            get;
            set;
        }

        public static string RandomString(bool guid = false)
        {
            string res = "";
            Random rg = Utils.GetTrulyRandom();
            for (int i = 0; i < rg.Next(3, 7); i++)
                res += (char)rg.Next((int)'A', (int)'Z' + 1);

            if (guid)
                res += rg.Next(1000, 100000);
            return res;
        }

        public static string RandomLetter()
        {
            Random rg = Utils.GetTrulyRandom();
            return ((char)rg.Next((int)'A', (int)'Z' + 1)).ToString();
        }

        public static string GetGuid()
        {
            return Utils.RandomString(true);
        }

        public static Db db { get; set; }
        public static Db search_db { get; set; }
        public static string BaseUrl = /*@"http://localhost:52512/";*/ @"http://rextester.com/";
        //public static string PathToFsc = @"C:\Program Files (x86)\Microsoft SDKs\F#\4.0\Framework\v4.0";
        public static string CurrentPath
        {
            get
            {
                return HttpContext.Current.Request.Url.AbsolutePath;
            }
        }

        public static string GetUrl(PagesEnum page)
        {
            switch (page)
            {
                case PagesEnum.Home:
                    return BaseUrl + "main";
                case PagesEnum.Tester:
                    return BaseUrl + "tester";
                case PagesEnum.Replace:
                    return BaseUrl + "replace";
                case PagesEnum.Reference:
                    return BaseUrl + "reference";
                case PagesEnum.Diff:
                    return BaseUrl + "diff";
                case PagesEnum.Rundotnet:
                    return BaseUrl;
                case PagesEnum.Codewall:
                    return BaseUrl + "codewall";
                case PagesEnum.Users:
                    return BaseUrl + "users";
                case PagesEnum.Feedback:
                    return BaseUrl + "feedback";
                case PagesEnum.UsersStuff:
                    return BaseUrl + "login/usersstuff";
                case PagesEnum.Notifications:
                    return BaseUrl + "login/notifications";
                case PagesEnum.Login:
                    return BaseUrl + "login";
                case PagesEnum.Logout:
                    return BaseUrl + "login/logout";
                case PagesEnum.Faq:
                    return BaseUrl + "main/faq";
                case PagesEnum.Log:
                    return BaseUrl + "logdata";
                default:
                    return BaseUrl;
            }
        }

        public static PagesEnum GetCurrentPage()
        {
            string pagePath = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
            string[] parts = pagePath.Split(new string[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
                string page = parts[parts.Length - 1].ToLower();

                if (pagePath.Trim('/').ToLower().Trim() == "tester")
                    return PagesEnum.Tester;
                if (pagePath.Trim('/').ToLower().Trim() == "replace")
                    return PagesEnum.Replace;
                if (pagePath.Trim('/').ToLower().Trim() == "reference")
                    return PagesEnum.Reference;
                if (pagePath.Trim('/').ToLower().Trim() == "diff")
                    return PagesEnum.Diff;
                if (pagePath.Trim('/').ToLower().Trim() == "codewall")
                    return PagesEnum.Codewall;
                if (pagePath.Trim('/').ToLower().Trim() == "feedback")
                    return PagesEnum.Feedback;
                if (pagePath.Trim('/').ToLower().Contains("login/usersstuff"))
                    return PagesEnum.UsersStuff;
                if (pagePath.Trim('/').ToLower().Contains("login/notifications"))
                    return PagesEnum.Notifications;
                if (pagePath.Trim('/').ToLower().Trim() == "users" || pagePath.Trim('/').ToLower().Trim().StartsWith("users/"))
                    return PagesEnum.Users;
                if (pagePath.Trim('/').ToLower().Trim() == "login")
                    return PagesEnum.Login;
                if (pagePath.Trim('/').ToLower().Trim() == "logout")
                    return PagesEnum.Logout;
                if (pagePath.Trim('/').ToLower().Trim() == "main/faq")
                    return PagesEnum.Home;
                if (pagePath.Trim('/').ToLower().Trim() == "main")
                    return PagesEnum.Home;
                if (pagePath.Trim('/').ToLower().Trim() == "logdata")
                    return PagesEnum.Log;
                //if (pagePath.Trim('/').ToLower().Trim() == "discussion")
                //    return PagesEnum.Rundotnet;
                //if (pagePath.Contains("rundotnet") || pagePath.Contains("runcode") || pagePath.Contains("versions") || new System.Text.RegularExpressions.Regex(@"/[a-z]+\d+.*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).IsMatch(pagePath))
                //    return PagesEnum.Rundotnet;

                return PagesEnum.Rundotnet;
            }
            else
                return PagesEnum.Rundotnet;
        }

        public enum PagesEnum
        {
            Home,
            Tester,
            Replace,
            Reference,
            Diff,
            Rundotnet,
            Codewall,
            Feedback,
            UsersStuff,
            Login,
            Logout,
            Faq,
            Users,
            Notifications,
            Log,
            Unknown
        }
    }

    public class Log
    {
        public static void LogInfo(string info, Exception ex, string type)
        {
            try
            {
                info += " ";
                if (ex != null)
                {
                    if (ex.InnerException != null)
                    {
                        info += ex.InnerException.Message + " " + ex.InnerException.StackTrace;
                    }
                    else
                    {
                        info += ex.Message + " " + ex.StackTrace;
                    }
                }
                LogJob job = new LogJob(info, type);
                HostingEnvironment.QueueBackgroundWorkItem(f => job.DoWork());
                //ThreadPool.QueueUserWorkItem(f => job.DoWork());
            }
            catch (Exception)
            { }
        }

        public static void LogCodeToDB(string data, string input, string compiler_args, string result, int lang, bool is_api, bool is_success)
        {
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(f => Model.LogRun(data, input, compiler_args, result, lang, is_api, ConfigurationManager.AppSettings["LogPath"], is_success));
                //ThreadPool.QueueUserWorkItem(f => Model.LogRun(data, input, compiler_args, result, lang, is_api, ConfigurationManager.AppSettings["LogPath"]));
            }
            catch (Exception)
            { }
        }
    }

    class LogJob
    {
        string Info
        {
            get;
            set;
        }

        string Type
        {
            get;
            set;
        }

        public LogJob(string info, string type)
        {
            this.Info = info;
            this.Type = type;
        }

        public void DoWork()
        {
            try
            {
                Model.LogInfo(Info, Type);
            }
            catch (Exception)
            { }
            try
            {
                var fromAddress = new MailAddress(GlobalUtils.TopSecret.FeedbackAdress);
                var toAddress = new MailAddress(GlobalUtils.TopSecret.FeedbackToAdress);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(GlobalUtils.TopSecret.EmailUser, GlobalUtils.TopSecret.EmailPass),
                    Timeout = 10000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "Feedback from rextester: " + Type,
                    Body = Info
                })
                {
                    smtp.Send(message);
                };
            }
            catch (Exception)
            { }
        }
    }

    public class CleanUp
    {
        public static void DeleteFile(string path)
        {
            try
            {
                DeleteFileJob job = new DeleteFileJob(path);
                System.Threading.Thread worker = new System.Threading.Thread(job.DoWork);
                worker.Start();
            }
            catch (Exception)
            { }
        }
    }

    class DeleteFileJob
    {
        string Path
        {
            get;
            set;
        }
        public DeleteFileJob(string path)
        {
            this.Path = path;
        }

        public void DoWork()
        {
            System.Threading.Thread.Sleep(5000);
            try
            {
                File.Delete(Path);
            }
            catch (Exception)
            {
                //reExp.Utils.Log.LogInfo("Can't delete " + path + ". " + e.Message, "RunDotNet");
            }
        }
    }




    class EncryptionUtils
    {
        public static byte[] EncodeDecode(byte[] infoBytes)
        {
            string skey = GlobalUtils.TopSecret.EncryptionKey;
            byte[] secretKey = System.Text.Encoding.Unicode.GetBytes(skey);

            if (secretKey.Length > infoBytes.Length)
            {
                List<byte> tmp = new List<byte>();
                for (int i = 0; i < infoBytes.Length; i++)
                    tmp[i] = secretKey[i];
                secretKey = tmp.ToArray();
            }
            else if (secretKey.Length < infoBytes.Length)
            {
                List<byte> tmp = new List<byte>(secretKey);
                for (int i = 0; i < infoBytes.Length - secretKey.Length; i++)
                    tmp.Add(secretKey[i % secretKey.Length]);
                secretKey = tmp.ToArray();
            }

            for (int i = 0; i < secretKey.Length; i++)
                infoBytes[i] = (byte)(infoBytes[i] ^ secretKey[i]);

            return infoBytes;
        }

        public static string ToUserString(byte[] bytes)
        {
            string tmp = "";
            foreach (byte b in bytes)
                tmp += Convert.ToInt16(b).ToString() + "|";
            return tmp;
        }
        public static byte[] FromUserString(string s)
        {
            List<byte> bytes = new List<byte>();
            string[] parts = s.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
                bytes.Add((byte)Convert.ToInt16(part));
            return bytes.ToArray();
        }


        public static string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.Unicode.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    public class SessionManager
    {
        public static int? UserId
        {
            get
            {
                return Model.GetCurrentUserId(SessionManager.UserName);
            }
        }

        public static string UserName
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }
        public static bool IsAdmin
        { 
            get
            {
                if(!SessionManager.IsUserInSession())
                {
                    return false;
                }
                return Model.IsCurrentUserAdmin(SessionManager.UserName);
            }
        }
        public static bool IsUserInSession()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        public static void LogOut()
        {
            FormsAuthentication.SignOut();
        }
        public static void SetAuthentication(string name)
        {
            FormsAuthentication.SetAuthCookie(name, true);
        }
    }

    public class GlobalConst
    {
        public enum RundotnetStatus : int
        {
            Error = 0,
            OK = 1,
            Unknown = 2
        }

        public const int RecordsPerPage = 100;
    }

    public enum EditorsEnum : int
    {
        Codemirror = 1,
        Editarea = 2,
        Simple = 3,
        Unknown = 0
    }

    public enum LanguagesEnum : int
    {
        CSharp = 1,
        VB = 2,
        FSharp = 3,
        Java = 4,
        Python = 5,
        C = 6,
        CPP = 7,
        Php = 8,
        Pascal = 9,
        ObjectiveC = 10,
        Haskell = 11,
        Ruby = 12,
        Perl = 13,
        Lua = 14,
        Nasm = 15,
        SqlServer = 16,
        Javascript = 17,
        Lisp = 18,
        Prolog = 19,
        Go = 20,
        Scala = 21,
        Scheme = 22,
        Nodejs = 23,
        Python3 = 24,
        Octave = 25,
        CClang = 26,
        CPPClang = 27,
        VCPP = 28,
        VC = 29,
        D = 30,
        R = 31,
        Tcl = 32,
        MySql = 33,
        Postgresql = 34,
        Oracle = 35,
        ClientSide = 36,
        Unknown = 0
    }
}