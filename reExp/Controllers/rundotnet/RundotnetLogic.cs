using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using reExp.Utils;
using Service;
namespace reExp.Controllers.rundotnet
{
    public class RundotnetLogic
    {
        public static RundotnetData RunProgram(RundotnetData data)
        {

            if (data.LanguageChoice == LanguagesEnum.CSharp || data.LanguageChoice == LanguagesEnum.VB)
            {
                return RunDotNet(data);
            }
            else if (data.LanguageChoice == LanguagesEnum.FSharp)
            {
                data.RunStats = "";
                data.Errors = new List<string>() { "F# is no longer supported on rextester." };
                return data;
            }
            else if (data.LanguageChoice == LanguagesEnum.SqlServer)
            {
                return RunSqlServer(data);
            }
            else if (data.LanguageChoice == LanguagesEnum.VCPP || data.LanguageChoice == LanguagesEnum.VC)
            {
                return RunWindows(data);
            }
            else if (data.LanguageChoice == LanguagesEnum.MySql)
            {
                return RunMySql(data);
            }
            else if (data.LanguageChoice == LanguagesEnum.Postgresql)
            {
                return RunPostgre(data);
            }
            else if (data.LanguageChoice == LanguagesEnum.Oracle)
            {
                return RunOracle(data);
            }
            else
            {
                return RunLinux(data);
            }
        }

        static RundotnetData RunSqlServer(RundotnetData data)
        {
            string path = reExp.Utils.Utils.RootFolder + "executables/usercode/" + Utils.Utils.RandomString() + ".sql";
            using (TextWriter tw = new StreamWriter(path))
            {
                tw.Write(data.Program);
            }

            using (Process process = new Process())
            {
                try
                {
                    double TotalMemoryInBytes = 0;
                    double TotalThreadCount = 0;
                    int samplesCount = 0;

                    process.StartInfo.FileName = reExp.Utils.Utils.RootFolder + "executables/SqlServer.exe";
                    process.StartInfo.Arguments = path.Replace(" ", "|_|");
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;

                    DateTime start = DateTime.Now;
                    process.Start();

                    OutputReader output = new OutputReader(process.StandardOutput, false);
                    Thread outputReader = new Thread(new ThreadStart(output.ReadOutput));
                    outputReader.Start();
                    OutputReader error = new OutputReader(process.StandardError);
                    Thread errorReader = new Thread(new ThreadStart(error.ReadOutput));
                    errorReader.Start();


                    do
                    {
                        // Refresh the current process property values.
                        process.Refresh();
                        if (!process.HasExited)
                        {
                            try
                            {
                                var proc = process.TotalProcessorTime;
                                // Update the values for the overall peak memory statistics.
                                var mem1 = process.PagedMemorySize64;
                                var mem2 = process.PrivateMemorySize64;

                                //update stats
                                TotalMemoryInBytes += (mem1 + mem2);
                                TotalThreadCount += (process.Threads.Count);
                                samplesCount++;

                                if (proc.TotalSeconds > 5 || mem1 + mem2 > 100000000 || process.Threads.Count > 100 || start + TimeSpan.FromSeconds(15) < DateTime.Now)
                                {
                                    var time = proc.TotalSeconds;
                                    var mem = mem1 + mem2;
                                    process.Kill();
                                    var res = string.Format("Process killed because it exceeded given resources.\nCpu time used {0} sec, absolute running time {1} sec, memory used {2} Mb, nr of threads {3}", time, (int)(DateTime.Now - start).TotalSeconds, (int)(mem / 1048576), process.Threads.Count);
                                    data.Errors.Add(res);
                                    string partialResult = output.Builder.ToString();
                                    data.Output = partialResult;
                                    data.RunStats = string.Format("Absolute service time: {0} sec", Math.Round((double)(DateTime.Now - start).TotalMilliseconds / 1000, 2));
                                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, res, (int)data.LanguageChoice, data.IsApi);
                                    return data;
                                }
                            }
                            catch (InvalidOperationException)
                            {
                                break;
                            }
                        }
                    }
                    while (!process.WaitForExit(10));
                    process.WaitForExit();

                    errorReader.Join(5000);
                    outputReader.Join(5000);

                    if (!string.IsNullOrEmpty(error.Output))
                    {
                        data.Output = output.Builder.ToString();
                        data.Errors.Add(error.Output);
                        data.RunStats = string.Format("Absolute service time: {0} sec", Math.Round((double)(DateTime.Now - start).TotalMilliseconds / 1000, 2));
                        Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, error.Output, (int)data.LanguageChoice, data.IsApi);
                        return data;
                    }

                    if (File.Exists(path + ".stats"))
                    {
                        using (TextReader tr = new StreamReader(path + ".stats"))
                        {
                            data.RunStats = tr.ReadLine();
                            if (!string.IsNullOrEmpty(data.RunStats))
                                data.RunStats += ", ";
                            else
                                data.RunStats = "";
                            data.RunStats += string.Format("absolute service time: {0} sec", Math.Round((double)(DateTime.Now - start).TotalMilliseconds / 1000, 2));
                        }
                    }
                    else
                    {
                        data.RunStats = string.Format("Absolute service time: {0} sec", Math.Round((double)(DateTime.Now - start).TotalMilliseconds / 1000, 2));
                    }

                    data.Output = output.Output;
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "OK", (int)data.LanguageChoice, data.IsApi);
                    return data;
                }
                catch (Exception e)
                {
                    if (!process.HasExited)
                    {
                        reExp.Utils.Log.LogInfo("Process left running " + e.Message, "RunSqlServer");
                    }
                    throw;
                }
                finally
                {
                    try
                    {
                        reExp.Utils.CleanUp.DeleteFile(path);
                        reExp.Utils.CleanUp.DeleteFile(path + ".stats");
                    }
                    catch (Exception)
                    { }

                    SqlServerUtils job = new SqlServerUtils();
                    Thread t = new Thread(job.DoShrinkJob);
                    t.Start();
                }
            }
        }


        static RundotnetData RunDotNet(RundotnetData data)
        {
            int compilationTimeInMs;
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = false;
            Random rg = Utils.Utils.GetTrulyRandom();
            string folder = reExp.Utils.Utils.RootFolder + @"\executables\usercode\";
            string assemblyName = "userAssembly_" + rg.Next(0, 10000000);

            string path = folder + assemblyName + ".dll";
            cp.OutputAssembly = path;
            // Save the assembly as a physical file.
            cp.GenerateInMemory = false;
            // Set whether to treat all warnings as errors.
            cp.TreatWarningsAsErrors = false;
            cp.WarningLevel = 4;
            cp.IncludeDebugInformation = false;


            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Core.dll");

            cp.ReferencedAssemblies.Add("System.Numerics.dll");
            cp.ReferencedAssemblies.Add("System.Data.dll");
            cp.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
            cp.ReferencedAssemblies.Add("System.Xml.dll");
            cp.ReferencedAssemblies.Add("System.Xml.Linq.dll");
            cp.ReferencedAssemblies.Add("System.Drawing.dll");

            if (data.LanguageChoice == LanguagesEnum.CSharp)
                cp.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            else if (data.LanguageChoice == LanguagesEnum.VB)
                cp.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll");

            cp.ReferencedAssemblies.Add(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute).Assembly.Location);
            cp.ReferencedAssemblies.Add(typeof(System.ComponentModel.Composition.ImportAttribute).Assembly.Location);
            cp.ReferencedAssemblies.Add(typeof(System.Web.HttpRequest).Assembly.Location);
            cp.ReferencedAssemblies.Add(typeof(System.Net.Http.HttpClient).Assembly.Location);
            cp.ReferencedAssemblies.Add(typeof(Newtonsoft.Json.JsonSerializer).Assembly.Location);

            //cp.ReferencedAssemblies.Add(typeof(System.Windows.Threading.DispatcherTimer).Assembly.Location);
            
            CompilerResults cr = null;

            using (var provider = GetProvider(data.LanguageChoice))
            {
                DateTime comp_start = DateTime.Now;
                // Invoke compilation of the source file.
                cr = provider.CompileAssemblyFromSource(cp, new string[] { data.Program });
                compilationTimeInMs = (int)(DateTime.Now - comp_start).TotalMilliseconds;
            }

            var messages = cr.Errors.Cast<CompilerError>();
            var warnings = messages.Where(f => f.IsWarning == true);
            var errors = messages.Where(f => f.IsWarning == false);

            if (warnings.Count() != 0)
            {
                foreach (var warn in warnings)
                    data.Warnings.Add(string.Format("({0}:{1}) {2}", warn.Line, warn.Column, warn.ErrorText));
            }
            if (errors.Count() != 0)
            {
                foreach (var ce in errors)
                {
                    data.Errors.Add(string.Format("({0}:{1}) {2}", ce.Line, ce.Column, ce.ErrorText));
                }
                Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Compilation errors", (int)data.LanguageChoice, data.IsApi);
                data.RunStats = string.Format("Compilation time: {0} s", Math.Round((compilationTimeInMs / (double)1000), 2));
                return data;
            }
            else
            {
                using (Process process = new Process())
                {
                    try
                    {
                        double TotalMemoryInBytes = 0;
                        double TotalThreadCount = 0;
                        int samplesCount = 0;

                        process.StartInfo.FileName = reExp.Utils.Utils.RootFolder + "executables/SpawnedProcess.exe";
                        process.StartInfo.Arguments = folder.Replace(" ", "|_|") + " " + assemblyName + " Rextester|Program|Main";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.RedirectStandardInput = true;

                        DateTime start = DateTime.Now;
                        process.Start();
                        //try
                        //{
                        //    process.PriorityClass = ProcessPriorityClass.BelowNormal;
                        //}
                        //catch (Exception)
                        //{ }

                        if (!string.IsNullOrEmpty(data.Input))
                        {
                            InputWriter input = new InputWriter(process.StandardInput, data.Input);
                            Thread inputWriter = new Thread(new ThreadStart(input.Writeinput));
                            inputWriter.Start();
                        }

                        OutputReader output = new OutputReader(process.StandardOutput);
                        Thread outputReader = new Thread(new ThreadStart(output.ReadOutput));
                        outputReader.Start();
                        OutputReader error = new OutputReader(process.StandardError);
                        Thread errorReader = new Thread(new ThreadStart(error.ReadOutput));
                        errorReader.Start();

                        
                        do
                        {
                            // Refresh the current process property values.
                            process.Refresh();
                            if (!process.HasExited)
                            {
                                try
                                {
                                    var proc = process.TotalProcessorTime;
                                    // Update the values for the overall peak memory statistics.
                                    var mem1 = process.PagedMemorySize64;
                                    var mem2 = process.PrivateMemorySize64;

                                    //update stats
                                    TotalMemoryInBytes += (mem1 + mem2);
                                    TotalThreadCount += (process.Threads.Count);
                                    samplesCount++;

                                    if (proc.TotalSeconds > 5 || mem1 + mem2 > 100000000 || process.Threads.Count > 100 || start + TimeSpan.FromSeconds(10) < DateTime.Now)
                                    {
                                        var time = proc.TotalSeconds;
                                        var mem = mem1 + mem2;
                                        process.Kill();
                                        var res = string.Format("Process killed because it exceeded given resources.\nCpu time used {0} sec, absolute running time {1} sec, memory used {2} Mb, nr of threads {3}", time, (int)(DateTime.Now - start).TotalSeconds, (int)(mem / 1048576), process.Threads.Count);
                                        data.Errors.Add(res);
                                        string partialResult = output.Builder.ToString();
                                        data.Output = partialResult;
                                        Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, res, (int)data.LanguageChoice, data.IsApi);
                                        data.RunStats = string.Format("Compilation time: {0} sec, absolute running time: {1} sec, cpu time: {2} sec, average memory usage: {3} Mb, average nr of threads: {4}",
                                            Math.Round((compilationTimeInMs / (double)1000), 2),
                                            Math.Round((DateTime.Now - start).TotalSeconds, 2),
                                            Math.Round(proc.TotalSeconds, 2),
                                            samplesCount != 0 ? (int?)((TotalMemoryInBytes / samplesCount) / 1048576) : null,
                                            samplesCount != 0 ? (int?)(TotalThreadCount / samplesCount) : null);
                                        return data;
                                    }
                                }
                                catch (InvalidOperationException)
                                {
                                    break;
                                }
                            }
                        }
                        while (!process.WaitForExit(10));
                        process.WaitForExit();

                        data.RunStats = string.Format("Compilation time: {0} sec, absolute running time: {1} sec, cpu time: {2} sec, average memory usage: {3} Mb, average nr of threads: {4}",
                                            Math.Round((compilationTimeInMs / (double)1000), 2),
                                            Math.Round((process.ExitTime - process.StartTime).TotalSeconds, 2),
                                            Math.Round(process.TotalProcessorTime.TotalSeconds, 2),
                                            samplesCount != 0 ? (int?)((TotalMemoryInBytes / samplesCount) / 1048576) : null,
                                            samplesCount != 0 ? (int?)(TotalThreadCount / samplesCount) : null);

                        errorReader.Join(5000);
                        outputReader.Join(5000);
                        if (!string.IsNullOrEmpty(error.Output))
                        {
                            data.Output = output.Builder.ToString();
                            data.Errors.Add(error.Output);
                            Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, error.Output, (int)data.LanguageChoice, data.IsApi);
                            return data;
                        }
                        data.Output = output.Output;
                        Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "OK", (int)data.LanguageChoice, data.IsApi);
                        return data;
                    }
                    catch (Exception e)
                    {
                        if (!process.HasExited)
                        {
                            reExp.Utils.Log.LogInfo("Process left running " + e.Message, "RunDotNet");
                        }
                        throw;
                    }
                    finally
                    {
                        reExp.Utils.CleanUp.DeleteFile(path);
                    }
                }
            }
        }

        static CodeDomProvider GetProvider(LanguagesEnum language)
        {
            switch (language)
            { 
                case LanguagesEnum.CSharp:
                    return new Microsoft.CSharp.CSharpCodeProvider();
                case LanguagesEnum.VB:
                    return new Microsoft.VisualBasic.VBCodeProvider();
            }

            return null;
        }

        static RundotnetData RunMySql(RundotnetData data)
        {
            WindowsService service = new WindowsService();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var res = service.DoWorkMySql(data.Program, data.Input, data.CompilerArgs);
            watch.Stop();
            if (res != null)
            {
                if (string.IsNullOrEmpty(res.Stats))
                    res.Stats = "";
                else
                    res.Stats += ", ";
                res.Stats += string.Format("absolute service time: {0} sec", Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2));
                data.RunStats = res.Stats;
            }
            bool logged = false;
            if (!string.IsNullOrEmpty(res.System_Error))
            {
                reExp.Utils.Log.LogInfo("MySql " + res.System_Error, "RunDotNet");
                data.Errors.Add(res.System_Error);
                Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "MySql: system error", (int)data.LanguageChoice, data.IsApi);
                return data;
            }
            if (!string.IsNullOrEmpty(res.Errors))
            {
                data.Errors.Add(res.Errors);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "MySql: error", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            if (res.Exit_Code < 0)
            {
                data.Errors.Add(res.Exit_Status);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "MySql: negative exit code", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            if (!string.IsNullOrEmpty(res.Warnings))
            {
                data.Warnings.Add(res.Warnings);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "MySql: warnings", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            data.Output = res.Output;
            if (res.Files != null)
            {
                data.Files = new List<string>();
                foreach (var f in res.Files)
                {
                    data.Files.Add(Convert.ToBase64String(f));
                }
            }
            if (!logged)
            {
                Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "MySql: ok", (int)data.LanguageChoice, data.IsApi);
                logged = true;
            }
            return data;
        }

        static RundotnetData RunPostgre(RundotnetData data)
        {
            WindowsService service = new WindowsService();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var res = service.DoPostgreSql(data.Program, data.Input, data.CompilerArgs);
            watch.Stop();
            if (res != null)
            {
                if (string.IsNullOrEmpty(res.Stats))
                    res.Stats = "";
                else
                    res.Stats += ", ";
                res.Stats += string.Format("absolute service time: {0} sec", Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2));
                data.RunStats = res.Stats;
            }
            bool logged = false;
            if (!string.IsNullOrEmpty(res.System_Error))
            {
                reExp.Utils.Log.LogInfo("PostgreSql " + res.System_Error, "RunDotNet");
                data.Errors.Add(res.System_Error);
                Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "PostgreSql: system error", (int)data.LanguageChoice, data.IsApi);
                return data;
            }
            if (!string.IsNullOrEmpty(res.Errors))
            {
                data.Errors.Add(res.Errors);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "PostgreSql: error", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            if (res.Exit_Code < 0)
            {
                data.Errors.Add(res.Exit_Status);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "PostgreSql: negative exit code", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            if (!string.IsNullOrEmpty(res.Warnings))
            {
                data.Warnings.Add(res.Warnings);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "PostgreSql: warnings", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            data.Output = res.Output;
            if (res.Files != null)
            {
                data.Files = new List<string>();
                foreach (var f in res.Files)
                {
                    data.Files.Add(Convert.ToBase64String(f));
                }
            }
            if (!logged)
            {
                Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "PostgreSql: ok", (int)data.LanguageChoice, data.IsApi);
                logged = true;
            }
            return data;
        }

        static RundotnetData RunOracle(RundotnetData data)
        {
            WindowsService service = new WindowsService();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var res = service.DoOracleSql(data.Program, data.Input, data.CompilerArgs);
            watch.Stop();
            if (res != null)
            {
                if (string.IsNullOrEmpty(res.Stats))
                    res.Stats = "";
                else
                    res.Stats += ", ";
                res.Stats += string.Format("absolute service time: {0} sec", Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2));
                data.RunStats = res.Stats;
            }
            bool logged = false;
            if (!string.IsNullOrEmpty(res.System_Error))
            {
                reExp.Utils.Log.LogInfo("Oracle " + res.System_Error, "RunDotNet");
                data.Errors.Add(res.System_Error);
                Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Oracle: system error", (int)data.LanguageChoice, data.IsApi);
                return data;
            }
            if (!string.IsNullOrEmpty(res.Errors))
            {
                data.Errors.Add(res.Errors);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Oracle: error", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            if (res.Exit_Code < 0)
            {
                data.Errors.Add(res.Exit_Status);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Oracle: negative exit code", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            if (!string.IsNullOrEmpty(res.Warnings))
            {
                data.Warnings.Add(res.Warnings);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Oracle: warnings", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            data.Output = res.Output;
            if (res.Files != null)
            {
                data.Files = new List<string>();
                foreach (var f in res.Files)
                {
                    data.Files.Add(Convert.ToBase64String(f));
                }
            }
            if (!logged)
            {
                Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Oracle: ok", (int)data.LanguageChoice, data.IsApi);
                logged = true;
            }
            return data;
        }
        static RundotnetData RunWindows(RundotnetData data)
        {
            WindowsService service = new WindowsService();
            Service.win.Languages lang = Service.win.Languages.VCPP;

            switch (data.LanguageChoice)
            {
                case LanguagesEnum.VCPP:
                    lang = Service.win.Languages.VCPP;
                    break;
                case LanguagesEnum.VC:
                    lang = Service.win.Languages.VC;
                    break;
                default:
                    break;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var res = service.DoWork(data.Program, data.Input, data.CompilerArgs, lang);
            watch.Stop();
            if (res != null)
            {
                if (string.IsNullOrEmpty(res.Stats))
                    res.Stats = "";
                else
                    res.Stats += ", ";
                res.Stats += string.Format("absolute service time: {0} sec", Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2));
                data.RunStats = res.Stats;
            }
            bool logged = false;
            if (!string.IsNullOrEmpty(res.System_Error))
            {
                reExp.Utils.Log.LogInfo("Windows " + res.System_Error, "RunDotNet");
                data.Errors.Add(res.System_Error);
                Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Windows: system error", (int)data.LanguageChoice, data.IsApi);
                return data;
            }
            if (!string.IsNullOrEmpty(res.Errors))
            {
                data.Errors.Add(res.Errors);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Windows: error", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            if (res.Exit_Code < 0)
            {
                data.Errors.Add(res.Exit_Status);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Windows: negative exit code", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            if (!string.IsNullOrEmpty(res.Warnings))
            {
                data.Warnings.Add(res.Warnings);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Windows: warnings", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            data.Output = res.Output;
            if (res.Files != null)
            {
                data.Files = new List<string>();
                foreach (var f in res.Files)
                {
                    data.Files.Add(Convert.ToBase64String(f));
                }
            }
            if (!logged)
            {
                Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Windows: ok", (int)data.LanguageChoice, data.IsApi);
                logged = true;
            }
            return data;
        }
        static RundotnetData RunLinux(RundotnetData data)
        {
            LinuxService service = new LinuxService();
            Service.linux.Languages lang = Service.linux.Languages.Java;

            if (data.LanguageChoice == LanguagesEnum.Prolog)
            {
                data.Input += "\nhalt.";
            }
            switch(data.LanguageChoice)
            {
                case LanguagesEnum.Java:
                    lang = Service.linux.Languages.Java;
                    break;
                case LanguagesEnum.Python:
                    lang = Service.linux.Languages.Python;
                    break;
                case LanguagesEnum.C:
                    lang = Service.linux.Languages.C;
                    break;
                case LanguagesEnum.CPP:
                    lang = Service.linux.Languages.CPP;
                    break;
                case LanguagesEnum.Php:
                    lang = Service.linux.Languages.Php;
                    break;
                case LanguagesEnum.Pascal:
                    lang = Service.linux.Languages.Pascal;
                    break;
                case LanguagesEnum.ObjectiveC:
                    lang = Service.linux.Languages.ObjectiveC;
                    break;
                case LanguagesEnum.Haskell:
                    lang = Service.linux.Languages.Haskell;
                    break;
                case LanguagesEnum.Ruby:
                    lang = Service.linux.Languages.Ruby;
                    break;
                case LanguagesEnum.Perl:
                    lang = Service.linux.Languages.Perl;
                    break;
                case LanguagesEnum.Lua:
                    lang = Service.linux.Languages.Lua;
                    break;
                case LanguagesEnum.Nasm:
                    lang = Service.linux.Languages.Nasm;
                    break;
                case LanguagesEnum.Javascript:
                    lang = Service.linux.Languages.Javascript;
                    break;
                case LanguagesEnum.Lisp:
                    lang = Service.linux.Languages.Lisp;
                    break;
                case LanguagesEnum.Prolog:
                    lang = Service.linux.Languages.Prolog;
                    break;
                case LanguagesEnum.Go:
                    lang = Service.linux.Languages.Go;
                    break;
                case LanguagesEnum.Scala:
                    lang = Service.linux.Languages.Scala;
                    break;
                case LanguagesEnum.Scheme:
                    lang = Service.linux.Languages.Scheme;
                    break;
                case LanguagesEnum.Nodejs:
                    lang = Service.linux.Languages.Nodejs;
                    break;
                case LanguagesEnum.Python3:
                    lang = Service.linux.Languages.Python3;
                    break;
                case LanguagesEnum.Octave:
                    lang = Service.linux.Languages.Octave;
                    break;
                case LanguagesEnum.CClang:
                    lang = Service.linux.Languages.CClang;
                    break;
                case LanguagesEnum.CPPClang:
                    lang = Service.linux.Languages.CppClang;
                    break;
                case LanguagesEnum.D:
                    lang = Service.linux.Languages.D;
                    break;
                case LanguagesEnum.R:
                    lang = Service.linux.Languages.R;
                    break;
                case LanguagesEnum.Tcl:
                    lang = Service.linux.Languages.Tcl;
                    break;
                default:
                    break;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var res = service.DoWork(data.Program, data.Input, data.CompilerArgs, lang);
            watch.Stop();
            if (res != null)
            {
                if (string.IsNullOrEmpty(res.Stats))
                    res.Stats = "";
                else
                    res.Stats += ", ";
                res.Stats += string.Format("absolute service time: {0} sec", Math.Round((double)watch.ElapsedMilliseconds/(double)1000, 2));
                data.RunStats = res.Stats;
            }
            bool logged = false;
            if (!string.IsNullOrEmpty(res.System_Error))
            {
                reExp.Utils.Log.LogInfo("Linux " + res.System_Error, "RunDotNet");
                data.Errors.Add(res.System_Error);
                Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Linux: system error", (int)data.LanguageChoice, data.IsApi);
                return data;
            }
            if (!string.IsNullOrEmpty(res.Errors))
            {
                data.Errors.Add(res.Errors);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Linux: error", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            if (res.Exit_Code < 0)
            {
                data.Errors.Add(res.Exit_Status);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Linux: negative exit code", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            if (!string.IsNullOrEmpty(res.Warnings))
            {
                data.Warnings.Add(res.Warnings);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Linux: warnings", (int)data.LanguageChoice, data.IsApi);
                    logged = true;
                }
            }
            data.Output = res.Output;
            if (res.Files != null)
            {
                data.Files = new List<string>();
                foreach (var f in res.Files)
                {
                    data.Files.Add(Convert.ToBase64String(f));
                }
            }
            if (!logged)
            {
                Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "Linux: ok", (int)data.LanguageChoice, data.IsApi);
                logged = true;
            }
            return data;
        }
    }

    class InputWriter
    {
        StreamWriter writer;
        string input;

        public InputWriter(StreamWriter writer, string input)
        {
            this.writer = writer;
            this.input = input;
        }

        public void Writeinput()
        {
            using (StreamWriter utf8Writer = new StreamWriter(writer.BaseStream, Encoding.UTF8))
            {
                utf8Writer.Write(input);
            }
        }
    }
    class OutputReader
    {
        StreamReader Reader
        {
            get;
            set;
        }
        bool DoAppend
        {
            get;
            set;
        }
        public string Output
        {
            get;
            set;
        }
        StringBuilder sb = new StringBuilder();
        public StringBuilder Builder
        {
            get
            {
                return sb;
            }
        }
        public OutputReader(StreamReader reader, bool DoAppend = true)
        {
            this.Reader = reader;
            this.DoAppend = DoAppend;
        }

        public void ReadOutput()
        {
            try
            {                
                int bufferSize = 40000;
                byte[] buffer = new byte[bufferSize];
                int outputLimit = 200000;
                int count;
                bool addMore = true;
                while (true)
                {
                    Thread.Sleep(10);
                    count = Reader.BaseStream.Read(buffer, 0, bufferSize);
                    if (count != 0)
                    {
                        if (addMore)
                        {
                            sb.Append(Encoding.UTF8.GetString(buffer, 0, count));
                            if (sb.Length > outputLimit)
                            {
                                if(DoAppend)
                                    sb.Append("\n\n...");
                                addMore = false;
                            }
                        }
                    }
                    else
                        break;
                }
                Output = sb.ToString();
            }
            catch (Exception e)
            {
                Output = string.Format("Error while reading output: {0}", e.Message);
            }
        }
    }
}