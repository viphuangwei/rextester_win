using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace reExp.Controllers.rundotnet.autocomplete
{
    public class CompletionComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x.Take(4).Select(f => f.ToString()).Aggregate((a, b) => a + b) == y.Take(4).Select(f => f.ToString()).Aggregate((a, b) => a + b);
        }

        public int GetHashCode(string obj)
        {
            return obj.Take(4).Select(f => f.ToString()).Aggregate((a, b) => a + b).GetHashCode();
        }
    }
    public class VcppComplete
    {
        public static string Complete(string code, int position, int line, int ch)
        {
            var l = YcmdCompletions(code, position, line, ch);
            if (l == null)
            {
                l = new List<string>();
            }
            return JsonConvert.SerializeObject(l);

            //var t1 = Task.Run<List<string>>(() => EclimCompletions(code, position, line, ch));
            //var t2 = Task.Run<List<string>>(() => YcmdCompletions(code, position, line, ch));

            //var l1 = t1.Result;
            //var l2 = t2.Result;

            //if (l1 == null)
            //{
            //    l1 = new List<string>();
            //}
            //if (l2 == null)
            //{
            //    l2 = new List<string>();
            //}

            ////l2.AddRange(l1.Where(f => !string.IsNullOrEmpty(f) && char.IsLetter(f[0])));
            ////l2 = l2.Distinct(new CompletionComparer()).ToList();
            ////l2.AddRange(l1.Where(f => !string.IsNullOrEmpty(f) && !char.IsLetter(f[0])));
            //var dic = new Dictionary<string, string>();
            //foreach (var l in l2)
            //{
            //    var k = l.Length > 7 ? l.Substring(0, 7) : l;
            //    dic[k] = l;
            //}

            //foreach (var l in l1)
            //{ 
            //    var k = l.Length > 7 ? l.Substring(0, 7) : l;
            //    if (!dic.ContainsKey(k))
            //    {
            //        l2.Add(l);
            //    }
            //}

            //l2 = l2.OrderBy(f => f).ToList();
            //return JsonConvert.SerializeObject(l2);
        }

        public static List<string> EclimCompletions(string code, int position, int line, int ch)
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
                            return new List<string>();
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
                        res.Add(re.info.Trim());
                    }
                }
                
                return res;
            }
            catch (Exception)
            {
                return new List<string>();
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
        public static List<string> YcmdCompletions(string code, int position, int line, int ch)
        {
            try
            {
                Service.LinuxService serv = new Service.LinuxService();
                var compl = serv.GetCppCompletions(code, line, ch);
                return JsonConvert.DeserializeObject<List<string>>(compl);
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}