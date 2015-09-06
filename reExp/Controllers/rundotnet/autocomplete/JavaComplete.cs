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
    public class CompletionData
    {
        public string completion { get; set; }
        public string menu { get; set; }
        public string info { get; set; }
        public string type { get; set; }
    }
    public class Completions
    {
        public List<CompletionData> completions { get; set; }
    }
    public class JavaComplete
    {
        public static string Complete(string code, int position, int line, int ch)
        {
            string eclim_bat = @"C:\Users\Administrator\Desktop\eclipse\eclim.bat";
            string workspace = @"C:\Users\Administrator\workspace\";
            var random = "a"+new Random().Next(0, Int32.MaxValue - 1);
            JavaScriptSerializer json = new JavaScriptSerializer();
            try
            {
                File.WriteAllText(Path.Combine(workspace, "JAVA", "src", random + ".java"), code, Encoding.Unicode);
                //position--;

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = eclim_bat;
                    process.StartInfo.Arguments = "-command java_complete -p \"JAVA\" -f \"" + Path.Combine("src", random + ".java") + "\" -o " + position + "  -e utf-8 -l normal > " + Path.Combine(workspace, "JAVA", "src", random + ".out.txt");
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                }

                var res = new List<string>();
                if (File.Exists(Path.Combine(workspace, "JAVA", "src", random + ".out.txt")))
                {
                    var r =  JsonConvert.DeserializeObject<Completions>(File.ReadAllText(Path.Combine(workspace, "JAVA", "src", random + ".out.txt")));
                    foreach(var re in r.completions)
                    {
                        var ind = re.info.IndexOf(":");
                        if (ind > 0)
                        {
                            re.info = re.info.Substring(0, ind);
                        }
                        ind = re.info.IndexOf("-");
                        if (ind > 0)
                        {
                            re.info = re.info.Substring(0, ind);
                        }
                        if (re.info.StartsWith("java."))
                        {
                            re.info = re.info.Substring(5);
                        }
                        
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
                    File.Delete(Path.Combine(workspace, "JAVA", "src", random + ".out.txt"));
                    File.Delete(Path.Combine(workspace, "JAVA", "src", random + ".java"));
                }
                catch (Exception)
                { }
            }
        }
    }
}