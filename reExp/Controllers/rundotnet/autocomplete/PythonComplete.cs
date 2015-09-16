using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace reExp.Controllers.rundotnet.autocomplete
{
    public class PythonComplete
    {
        public static string Complete(string code, int position, int line, int ch)
        {
            string eclim_bat = @"C:\Users\Administrator\Desktop\eclipse\eclim.bat";
            string workspace = @"C:\Users\Administrator\workspace\";
            var random = "a" + new Random().Next(0, Int32.MaxValue - 1);
            JavaScriptSerializer json = new JavaScriptSerializer();
            try
            {
                File.WriteAllText(Path.Combine(workspace, "PYT", random + ".py"), code, Encoding.Unicode);
                //position--;

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = eclim_bat;
                    process.StartInfo.Arguments = "-command python_complete -p \"PYT\" -f \"" + random + ".py" + "\" -o " + position + "  -e utf-8  > " + Path.Combine(workspace, "PYT", random + ".out.txt");
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                }

                var res = new List<string>();
                if (File.Exists(Path.Combine(workspace, "PYT", random + ".out.txt")))
                {
                    var compl = JsonConvert.DeserializeObject<List<CompletionData>>(File.ReadAllText(Path.Combine(workspace, "PYT", random + ".out.txt")));
                    var r = new Completions() { completions = compl };
                    foreach (var re in r.completions)
                    {
                        string cmpl = re.completion;


                        if (!string.IsNullOrEmpty(re.info) && !string.IsNullOrEmpty(cmpl) &&
                            re.info.IndexOf(cmpl) > -1 && cmpl.Contains('(') && re.info.Contains(')'))
                        {
                            cmpl = re.info.Substring(re.info.IndexOf(cmpl), re.info.IndexOf(')') + 1);
                        }
                        if (!string.IsNullOrEmpty(cmpl.Trim()))
                        {
                            if (cmpl.Contains(" -"))
                            {
                                cmpl = cmpl.Substring(0, cmpl.IndexOf(" -"));
                            }
                            if (cmpl.Contains(")"))
                            {
                                cmpl = cmpl.Substring(0, cmpl.IndexOf(")") + 1);
                            }
                            cmpl = cmpl.Replace("<br/>", "").Replace("\n", "").Replace("\r", "");
                            cmpl = Regex.Replace(cmpl, @"\s+", " ");
                            res.Add(cmpl.Trim());
                        }
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
                    File.Delete(Path.Combine(workspace, "PYT", random + ".out.txt"));
                    File.Delete(Path.Combine(workspace, "PYT", random + ".py"));
                }
                catch (Exception)
                { }
            }
        }
    }
}