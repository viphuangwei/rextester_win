using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using WindowsExecutionEngine;

namespace WindowsService
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://rextester.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {

        [WebMethod]        
        public Result DoWork(string Program, string Input, Languages Language, string user, string pass, string compiler_args = "", bool bytes = false, bool programCompressed = false, bool inputCompressed = false)
        {
            if (user != GlobalUtils.TopSecret.Service_user || pass != GlobalUtils.TopSecret.Service_pass)
            {
                return new Result()
                {
                    Errors = null,
                    Warnings = null,
                    Output = null,
                    Stats = null,
                    Exit_Status = null,
                    Exit_Code = null,
                    System_Error = "Not authorized."
                };
            }

            if (programCompressed)
                Program = GlobalUtils.Utils.Decompress(Program);
            if (inputCompressed)
                Input = GlobalUtils.Utils.Decompress(Input);

            Engine engine = new Engine();
            InputData idata = new InputData()
            {
                Program = Program,
                Input = Input,
                Lang = Language,
                Compiler_args = compiler_args
            };

            var odata = engine.DoWork(idata);

            Regex r = new Regex(engine.RootPath.Replace(@"\", @"\\") + @"\d+\\", RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(odata.Output))
                odata.Output = r.Replace(odata.Output, "");
            Regex r2 = new Regex(engine.RootPath.Replace(@"\", @"\\") + @"\d+\\\d+", RegexOptions.IgnoreCase);

            var res = new Result()
            {
                Errors = !string.IsNullOrEmpty(odata.Errors) ? r.Replace(r2.Replace(odata.Errors, "source_file"), "") : odata.Errors,
                Warnings = !string.IsNullOrEmpty(odata.Warnings) ? r.Replace(r2.Replace(odata.Warnings, "source_file"), "") : odata.Warnings,
                Output = odata.Output,
                Stats = odata.Stats,
                Exit_Status = odata.Exit_Status,
                Exit_Code = odata.ExitCode,
                System_Error = odata.System_Error,
                Files = odata.Files
            };
            if (!string.IsNullOrEmpty(odata.Output) && odata.Output.Length > 1000)
            {
                res.Output = GlobalUtils.Utils.Compress(odata.Output);
                res.IsOutputCompressed = true;
            }
            if (bytes)
            {
                if (!string.IsNullOrEmpty(res.Errors))
                {
                    res.Errors_Bytes = System.Text.Encoding.Unicode.GetBytes(res.Errors);
                    res.Errors = null;
                }
                if (!string.IsNullOrEmpty(res.Warnings))
                {
                    res.Warnings_Bytes = System.Text.Encoding.Unicode.GetBytes(res.Warnings);
                    res.Warnings = null;
                }
                if (!string.IsNullOrEmpty(res.Output))
                {
                    res.Output_Bytes = System.Text.Encoding.Unicode.GetBytes(res.Output);
                    res.Output = null;
                }
            }
            return res;
        }

        [WebMethod]
        public string GetCurrentUser()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }

    }

    public class Result
    {
        public string Errors
        {
            get;
            set;
        }

        public byte[] Errors_Bytes
        {
            get;
            set;
        }

        public string Warnings
        {
            get;
            set;
        }

        public byte[] Warnings_Bytes
        {
            get;
            set;
        }

        public string Output
        {
            get;
            set;
        }

        public bool IsOutputCompressed
        {
            get;
            set;
        }

        public byte[] Output_Bytes
        {
            get;
            set;
        }

        public string Stats
        {
            get;
            set;
        }

        public string Exit_Status
        {
            get;
            set;
        }
        public int? Exit_Code
        {
            get;
            set;
        }
        public string System_Error
        {
            get;
            set;
        }
        public List<byte[]> Files
        {
            get;
            set;
        }
    }
}
