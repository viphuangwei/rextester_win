using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using LinqDb;
using System.Configuration;

namespace reExp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               null,
               "tester/{savedNr}",
               new { controller = "Tester", action = "Index" },
               new { savedNr = @"[A-Za-z]+\d+" }
            );

            routes.MapRoute(
               null,
               "replace/{savedNr}",
               new { controller = "Replace", action = "Index" },
               new { savedNr = @"[A-Za-z]+\d+" }
            );

            routes.MapRoute(
               null,
               "diff",
               new { controller = "Diff", action = "Index" }
            );

            routes.MapRoute(
               null,
               "discussion/{Guid}",
               new { controller = "Discussion", action = "Index" },
               new { Guid = @"[A-Za-z]+\d+" }
            );

            routes.MapRoute(
               null,
               "discussion/{Guid}/{title}",
               new { controller = "Discussion", action = "Index" },
               new { Guid = @"[A-Za-z]+\d+" }
            );

            routes.MapRoute(
               null,
               "edit/{savedNr}",
               new { controller = "RunDotNet", action = "GetEditCode" },
               new { savedNr = @"[A-Za-z]+\d+" }
            );

            routes.MapRoute(
               null,
               "history/{CodeGuid}",
               new { controller = "Versions", action = "Index" },
               new { CodeGuid = @"[A-Za-z]+\d+" }
            );

            routes.MapRoute(
               null,
               "runcode",
               new { controller = "RunDotNet", action = "Index", savedNr = (string)null }
           );

            routes.MapRoute(
               null,
               "live/{savedNr}",
               new { controller = "RunDotNet", action = "GetLiveCode" },
               new { savedNr = @"[A-Za-z]+\d+" }
            );

            routes.MapRoute(
               null,
               "users/{Wall_ID}",
               new { controller = "Users", action = "GetUserWallsCode" },
               new { Wall_ID = @"\d+" }
            );

            routes.MapRoute(
               null,
               "rundotnet/api_get/{nr}",
               new { controller = "RunDotNet", action = "Api_get" },
               new { nr = @"[A-Za-z]+\d+" }
            );

            routes.MapRoute(
               null,
               "{savedNr}",
               new { controller = "RunDotNet", action = "Index" },
               new { savedNr = @"[A-Za-z]+\d+" }
           );

            routes.MapRoute(
               null,
               "{savedNr}/{title}",
               new { controller = "RunDotNet", action = "Index" },
               new { savedNr = @"[A-Za-z]+\d+" }
            );

            routes.MapRoute(
               null,
               "l/{lang_title}",
               new { controller = "RunDotNet", action = "Index" },
               new { lang_title = @".+" }
            );

            routes.MapRoute(
                null,
                "codewall",
                new { controller = "codewall", action = "Index" }
            );

            routes.MapRoute(
                null,
                "users",
                new { controller = "users", action = "Index" }
            );

            routes.MapRoute(
                null,
                "login/UsersStuff/{page}",
                new { controller = "login", action = "UsersStuff" },
                new { page = @"\d+" }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "RunDotNet", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            reExp.Utils.Utils.RootFolder = Server.MapPath("~/");
            //System.Environment.SetEnvironmentVariable("FSHARP_BIN", Utils.Utils.PathToFsc);

            Utils.Utils.db = new Db(ConfigurationManager.AppSettings["LogPath"]);
        }

        protected void Application_End()
        {
            Utils.Utils.db.Dispose();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;

            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                Response.Redirect("~/Error/NotFound");
            }
            else
            {
                reExp.Utils.Log.LogInfo(DateTime.Now + " \n" + exception.Message + " \n" + exception.StackTrace, "ERROR");
                Response.Redirect("~/Error/Unknown");
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs ev)
        {
            if (Request.Url.Host.StartsWith("www", StringComparison.InvariantCultureIgnoreCase) && !Request.Url.IsLoopback)
            {
                Response.Clear();
                Response.AddHeader("Location",
                    String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host.Substring(4), Request.Url.PathAndQuery)
                    );
                Response.StatusCode = 301;
                Response.End();
            }
        }

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    if (!Request.Url.Host.StartsWith("www") && !Request.Url.IsLoopback)
        //    {
        //        UriBuilder builder = new UriBuilder(Request.Url);
        //        builder.Host = "www." + Request.Url.Host;
        //        Response.StatusCode = 301;
        //        Response.AddHeader("Location", builder.ToString());
        //        Response.End();
        //    }
        //}

    }
}