using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace reExp.Controllers.rundotnet.autocomplete
{
    public class VcppComplete
    {
        public static string Complete(string code, int position, int line, int ch)
        {
            string eclim_bat = @"C:\Users\Administrator\Desktop\eclipse\eclim.bat";
            string workspace = @"C:\Users\Administrator\workspace\";
            var random = "a" + new Random().Next(0, Int32.MaxValue - 1);
            JavaScriptSerializer json = new JavaScriptSerializer();
            try
            {
                File.WriteAllText(Path.Combine(workspace, "VCPP", "src", random + ".cpp"), code, Encoding.Unicode);
                //position--;

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = eclim_bat;
                    process.StartInfo.Arguments = "-command c_complete -p \"VCPP\" -f \"" + Path.Combine("src", random + ".cpp") + "\" -o " + position + "  -e utf-8 -l normal > " + Path.Combine(workspace, "VCPP", "src", random + ".out.txt");
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                }

                var res = new List<string>();
                if (File.Exists(Path.Combine(workspace, "VCPP", "src", random + ".out.txt")))
                {
                    var compl = JsonConvert.DeserializeObject<List<CompletionData>>(File.ReadAllText(Path.Combine(workspace, "VCPP", "src", random + ".out.txt")));
                    var r = new Completions() { completions = compl };
                    foreach (var re in r.completions)
                    {
                        if ((re.info + "").StartsWith("<br/>"))
                        {
                            return json.Serialize(new List<string>());
                        }
                        if (string.IsNullOrEmpty((re.info + "").Trim()))
                        {
                            continue;
                        }
                        var ind = re.info.IndexOf(" : ");
                        if (ind > 0)
                        {
                            re.info = re.info.Substring(0, ind);
                        }
                        //ind = re.info.IndexOf("-");
                        //if (ind > 0)
                        //{
                        //    re.info = re.info.Substring(0, ind);
                        //}

                        res.Add(re.info.Trim());
                    }
                }

                res.Sort();
                return json.Serialize(res);
            }
            catch (Exception e)
            {
                return json.Serialize(new List<string>() /*{ e.Message }*/);
            }
            finally
            {
                try
                {
                    File.Delete(Path.Combine(workspace, "VCPP", "src", random + ".out.txt"));
                    File.Delete(Path.Combine(workspace, "VCPP", "src", random + ".cpp"));
                }
                catch (Exception)
                { }
            }
        }
    }
}