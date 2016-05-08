using BookSleeve;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsExecutionEngine.Compiling;


namespace WindowsExecutionEngine
{
    public class InputData
    {
        public string Program
        {
            get;
            set;
        }

        public string Input
        {
            get;
            set;
        }

        public Languages Lang
        {
            get;
            set;
        }

        public string BaseDir
        {
            get;
            set;
        }

        public string PathToSource
        {
            get;
            set;
        }

        public string Rand
        {
            get;
            set;
        }
        public string Compiler_args
        {
            get;
            set;
        }
    }

    public class OutputData
    {
        public string Output
        {
            get;
            set;
        }
        public string Errors
        {
            get;
            set;
        }
        public string Warnings
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
        public int ExitCode
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
    public enum Languages
    {
        VCPP,
        VC,
        MySql
    }

    public class CompilerData
    {
        public bool Success
        {
            get;
            set;
        }
        public string Warning
        {
            get;
            set;
        }
        public string Error
        {
            get;
            set;
        }
        public string Executor
        {
            get;
            set;
        }
        public string ExecuteThis
        {
            get;
            set;
        }
        public string CleanThis
        {
            get;
            set;
        }
        public long CompileTimeMs
        {
            get;
            set;
        }
        public string Rand
        {
            get;
            set;
        }
    }

    public class Engine
    {
        static RedisConnection redis_conn = null;
        static object redis_lock = new object();
        public static RedisConnection RedisConnection
        {
            get
            {
                lock (redis_lock)
                {
                    if (redis_conn == null)
                    {
                        redis_conn = new RedisConnection("localhost", allowAdmin: true);
                    }
                    if (redis_conn.State != RedisConnectionBase.ConnectionState.Open)
                    {
                        try
                        {
                            redis_conn.Open().Wait();
                        }
                        catch (Exception)
                        {
                            try
                            {
                                redis_conn.Dispose();
                            }
                            catch (Exception) { }

                            redis_conn = new RedisConnection("localhost", allowAdmin: true);
                            redis_conn.Open().Wait();
                        }
                    }
                }
                return redis_conn;
            }
        }

        public Engine()
        { }

        public string RootPath
        {
            get
            {
                //return @"/home/ren/Desktop/rextester/linux/RextesterService/usercode/";
                return @"C:\inetpub\wwwroot\rextester\usercode\";
            }
        }

        public string BasePath
        {
            get
            {
                //return @"/home/ren/Desktop/rextester/linux/RextesterService/usercode/";
                return @"C:\inetpub\wwwroot\rextester\";
            }
        }

        double CpuTimeInSec
        {
            get;
            set;
        }
        int MemoryPickInKilobytes
        {
            get;
            set;
        }
        public OutputData DoWork(InputData idata)
        {
            if (idata.Lang == Languages.VC || idata.Lang == Languages.VCPP)
            {
                return RunVC(idata);
            }
            else
            {
                return RunSql(idata);
            }
        }

        OutputData RunVC(InputData idata)
        {
            CompilerData cdata = null;
            try
            {
                OutputData odata = new OutputData();
                cdata = CreateExecutable(idata);
                if (!cdata.Success)
                {
                    odata.Errors = cdata.Error;
                    odata.Warnings = cdata.Warning;
                    odata.Stats = string.Format("Compilation time: {0} sec", Math.Round((double)cdata.CompileTimeMs / (double)1000, 2));
                    return odata;
                }
                if (!string.IsNullOrEmpty(cdata.Warning))
                {
                    odata.Warnings = cdata.Warning;
                }

                Stopwatch watch = new Stopwatch();
                watch.Start();
                string nr = cdata.Rand;
                if (!string.IsNullOrEmpty(idata.Input))
                {
                    RedisConnection.Strings.Set(1, nr, Encoding.UTF8.GetBytes(idata.Input));
                }

                RedisConnection.Strings.Set(0, nr, new byte[] { (byte)1 });

                for (int i = 400; i > 0; i--)
                {
                    Thread.Sleep(100);
                    bool _break = false;
                    var res = RedisConnection.Strings.Get(4, nr).Result;
                    if (res != null)
                    {
                        _break = true;
                        var output = RedisConnection.Strings.Get(2, nr).Result;
                        if (output != null)
                        {
                            odata.Output = Encoding.UTF8.GetString(output);
                            var a = RedisConnection.Keys.Remove(2, nr).Result;
                        }
                        var errors = RedisConnection.Strings.Get(3, nr).Result;
                        if (errors != null)
                        {
                            odata.Errors = Encoding.UTF8.GetString(errors);
                            var a = RedisConnection.Keys.Remove(3, nr).Result;
                        }
                    }
                    if (_break)
                    {
                        break;
                    }
                }
                watch.Stop();


                if (Utils.IsCompiled(idata.Lang))
                {
                    //odata.Stats = string.Format("Compilation time: {0} sec, absolute running time: {1} sec, cpu time: {2} sec, memory peak: {3} Mb", Math.Round((double)cdata.CompileTimeMs / (double)1000, 2), Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2), Math.Round(CpuTimeInSec, 2), MemoryPickInKilobytes / 1024);
                    odata.Stats = string.Format("Compilation time: {0} sec, absolute running time: {1} sec", Math.Round((double)cdata.CompileTimeMs / (double)1000, 2), Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2));
                }
                else
                {
                    //odata.Stats = string.Format("Absolute running time: {0} sec, cpu time: {1} sec, memory peak: {2} Mb", Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2), Math.Round(CpuTimeInSec, 2), MemoryPickInKilobytes / 1024);
                    odata.Stats = string.Format("Absolute running time: {0} sec", Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2));
                }
                return odata;
            }
            catch (Exception ex)
            {
                return new OutputData()
                {
                    System_Error = ex.Message
                };
            }
            finally
            {
                if (cdata != null)
                    Cleanup(cdata.CleanThis);
            }
        }

        OutputData RunSql(InputData idata)
        {
            OutputData odata = new OutputData();
            string path = BasePath + @"usercode\" + Utils.RandomString() + ".sql";
            using (TextWriter tw = new StreamWriter(path))
            {
                tw.Write(idata.Program);
            }

            using (Process process = new Process())
            {
                try
                {
                    double TotalMemoryInBytes = 0;
                    double TotalThreadCount = 0;
                    int samplesCount = 0;

                    process.StartInfo.FileName = BasePath + @"executables\SqlSandbox.exe";
                    process.StartInfo.Arguments = path.Replace(" ", "|_|");
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;

                    DateTime start = DateTime.Now;
                    process.Start();

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

                                if (proc.TotalSeconds > 5 || mem1 + mem2 > 100000000 || process.Threads.Count > 100 || start + TimeSpan.FromSeconds(15) < DateTime.Now)
                                {
                                    var time = proc.TotalSeconds;
                                    var mem = mem1 + mem2;
                                    process.Kill();
                                    var res = string.Format("Process killed because it exceeded given resources.\nCpu time used {0} sec, absolute running time {1} sec, memory used {2} Mb, nr of threads {3}", time, (int)(DateTime.Now - start).TotalSeconds, (int)(mem / 1048576), process.Threads.Count);
                                    odata.Errors = odata.Errors + "\n" + res;
                                    string partialResult = output.Builder.ToString();
                                    odata.Output = partialResult;
                                    //odata.Stats = string.Format("Absolute service time: {0} sec", Math.Round((double)(DateTime.Now - start).TotalMilliseconds / 1000, 2));
                                    //Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, res, (int)data.LanguageChoice, data.IsApi);
                                    return odata;
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
                        odata.Output = output.Builder.ToString();
                        odata.Errors += "\n" + error.Output;
                        odata.Stats = string.Format("Absolute service time: {0} sec", Math.Round((double)(DateTime.Now - start).TotalMilliseconds / 1000, 2));
                        //Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, error.Output, (int)data.LanguageChoice, data.IsApi);
                        return odata;
                    }

                    if (File.Exists(path + ".stats"))
                    {
                        using (TextReader tr = new StreamReader(path + ".stats"))
                        {
                            odata.Stats = tr.ReadLine();
                            if (!string.IsNullOrEmpty(odata.Stats))
                                odata.Stats += ", ";
                            else
                                odata.Stats = "";
                            odata.Stats += string.Format("absolute service time: {0} sec", Math.Round((double)(DateTime.Now - start).TotalMilliseconds / 1000, 2));
                        }
                    }
                    //else
                    //{
                    //    odata.Stats = string.Format("Absolute service time: {0} sec", Math.Round((double)(DateTime.Now - start).TotalMilliseconds / 1000, 2));
                    //}

                    odata.Output = output.Output;
                   // Utils.Log.LogCodeToDB(data.Program, data.Input, data.CompilerArgs, "OK", (int)data.LanguageChoice, data.IsApi);
                    return odata;
                }
                catch (Exception e)
                {
                    if (!process.HasExited)
                    {
                        //reExp.Utils.Log.LogInfo("Process left running " + e.Message, "RunSqlServer");
                    }
                    throw;
                }
                finally
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception)
                    { }

                    //SqlServerUtils job = new SqlServerUtils();
                    //Thread t = new Thread(job.DoShrinkJob);
                    //t.Start();
                }
            }
        }

        class FileData
        {
            public byte[] Data { get; set; }
            public DateTime CreationDate { get; set; }
        }

        private void Cleanup(string dir)
        {

            try
            {
                //cleanup
                //Directory.SetCurrentDirectory(RootPath);
                //Directory.Delete(dir, true);
            }
            catch (Exception e) 
            {
                var a = e;
            }
        }
        CompilerData CreateExecutable(InputData input)
        {
            CompilerData cdata = new CompilerData();
            string ext = "";
            string rand = Utils.RandomString();
            cdata.Rand = rand;
            string dir = rand + @"\";
            switch (input.Lang)
            {
                case Languages.VCPP:
                    ext = ".cpp";
                    break;
                case Languages.VC:
                    ext = ".c";
                    break;
                default:
                    ext = ".unknown";
                    break;
            }
            string PathToSource = RootPath + dir + rand + ext;
            input.PathToSource = PathToSource;
            input.BaseDir = RootPath + dir;
            input.Rand = rand;
            Directory.CreateDirectory(RootPath + dir);

            Directory.SetCurrentDirectory(RootPath + dir);
            //DirectorySecurity sec = Directory.GetAccessControl(RootPath + dir);
            //SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            //sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.Modify | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            //Directory.SetAccessControl(RootPath + dir, sec);

            using (TextWriter sw = new StreamWriter(PathToSource))
            {
                sw.Write(input.Program);
            }

            cdata.CleanThis = RootPath + dir;

            var comp = ICompilerFactory.GetICompiler(input.Lang);
            if (comp != null)
                return comp.Compile(input, cdata);

            cdata.Success = false;
            return cdata;
        }

        public static List<string> CallCompiler(string compiler, string args, out long CompileTimeMs)
        {
            Stopwatch watch = new Stopwatch();
            using (Process process = new Process())
            using (var job = new Job())
            {
                process.StartInfo.FileName = compiler;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.Start();
                watch.Start();
                job.AddProcess(process.Handle);

                OutputReader output = new OutputReader(process.StandardOutput);
                Thread outputReader = new Thread(new ThreadStart(output.ReadOutput));
                outputReader.Start();
                OutputReader error = new OutputReader(process.StandardError);
                Thread errorReader = new Thread(new ThreadStart(error.ReadOutput));
                errorReader.Start();

                process.WaitForExit(30000);
                bool is_killed = false;
                if (!process.HasExited)
                {
                    process.Kill();
                    is_killed = true;
                }
                watch.Stop();
                
                CompileTimeMs = watch.ElapsedMilliseconds;
                errorReader.Join(5000);
                outputReader.Join(5000);


                List<string> compOutput = new List<string>();
                compOutput.Add(output.Output);
                if (is_killed)
                {
                    error.Output = "Compilation terminated after 30 seconds. " + Environment.NewLine + error.Output;
                }
                compOutput.Add(error.Output);
                return compOutput;
            }
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
            var encoding = new System.Text.UTF8Encoding(false);
            using (StreamWriter utf8Writer = new StreamWriter(writer.BaseStream, encoding))
            {
                utf8Writer.Write(input);
            }
        }
    }
    class OutputReader
    {
        StreamReader reader;
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
        public OutputReader(StreamReader reader, int interval = 10)
        {
            this.reader = reader;
            this.CheckInterval = interval;
        }

        int CheckInterval
        {
            get;
            set;
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
                    Thread.Sleep(CheckInterval);
                    count = reader.BaseStream.Read(buffer, 0, bufferSize);
                    if (count != 0)
                    {
                        if (addMore)
                        {
                            sb.Append(Encoding.UTF8.GetString(buffer, 0, count));
                            if (sb.Length > outputLimit)
                            {
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

    #region Job ojects
    public class Job : IDisposable
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr CreateJobObject(IntPtr a, string lpName);

        [DllImport("kernel32.dll")]
        static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, UInt32 cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        internal static extern int CloseHandle(IntPtr hObject);

        private IntPtr handle;
        private bool disposed;

        public Job()
        {
            handle = CreateJobObject(IntPtr.Zero, null);

            var info = new JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                LimitFlags = (uint)LimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
            };

            var io = new IO_COUNTERS
            {
                OtherOperationCount = 0,
                OtherTransferCount = 0,
                ReadOperationCount = 0,
                ReadTransferCount = 0,
                WriteOperationCount = 0,
                WriteTransferCount = 0
            };
            var extendedInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            {
                BasicLimitInformation = info
            };

            int length = Marshal.SizeOf(typeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
            IntPtr extendedInfoPtr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);

            if (!SetInformationJobObject(handle, JobObjectInfoType.ExtendedLimitInformation, extendedInfoPtr, (uint)length))
                throw new Exception(string.Format("Job object: Unable to set information.  Error: {0}", Marshal.GetLastWin32Error()));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing) { }

            Close();
            disposed = true;
        }

        public void Close()
        {
            CloseHandle(handle);
            handle = IntPtr.Zero;
        }

        public bool AddProcess(IntPtr processHandle)
        {
            return AssignProcessToJobObject(handle, processHandle);
        }

        public bool AddProcess(int processId)
        {
            return AddProcess(Process.GetProcessById(processId).Handle);
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    struct IO_COUNTERS
    {
        public UInt64 ReadOperationCount;
        public UInt64 WriteOperationCount;
        public UInt64 OtherOperationCount;
        public UInt64 ReadTransferCount;
        public UInt64 WriteTransferCount;
        public UInt64 OtherTransferCount;
    }

    public enum LimitFlags
    {
        JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 0x00000008,
        JOB_OBJECT_LIMIT_AFFINITY = 0x00000010,
        JOB_OBJECT_LIMIT_BREAKAWAY_OK = 0x00000800,
        JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 0x00000400,
        JOB_OBJECT_LIMIT_JOB_MEMORY = 0x00000200,
        JOB_OBJECT_LIMIT_JOB_TIME = 0x00000004,
        JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000,
        JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = 0x00000040,
        JOB_OBJECT_LIMIT_PRIORITY_CLASS = 0x00000020,
        JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x00000100,
        JOB_OBJECT_LIMIT_PROCESS_TIME = 0x00000002,
        JOB_OBJECT_LIMIT_SCHEDULING_CLASS = 0x00000080,
        JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = 0x00001000,
        JOB_OBJECT_LIMIT_WORKINGSET = 0x00000001
    }

    [StructLayout(LayoutKind.Sequential)]
    struct JOBOBJECT_BASIC_LIMIT_INFORMATION
    {
        public Int64 PerProcessUserTimeLimit;
        public Int64 PerJobUserTimeLimit;
        public UInt32 LimitFlags;
        public UIntPtr MinimumWorkingSetSize;
        public UIntPtr MaximumWorkingSetSize;
        public UInt32 ActiveProcessLimit;
        public UIntPtr Affinity;
        public UInt32 PriorityClass;
        public UInt32 SchedulingClass;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES
    {
        public UInt32 nLength;
        public IntPtr lpSecurityDescriptor;
        public Int32 bInheritHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
    {
        public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
        public IO_COUNTERS IoInfo;
        public UIntPtr ProcessMemoryLimit;
        public UIntPtr JobMemoryLimit;
        public UIntPtr PeakProcessMemoryUsed;
        public UIntPtr PeakJobMemoryUsed;
    }

    public enum JobObjectInfoType
    {
        AssociateCompletionPortInformation = 7,
        BasicLimitInformation = 2,
        BasicUIRestrictions = 4,
        EndOfJobTimeInformation = 6,
        ExtendedLimitInformation = 9,
        SecurityLimitInformation = 5,
        GroupInformation = 11
    }

    #endregion


    public class Result
    {
        public List<string> Errors { get; set; }
        public string Output { get; set; }
        public string RunStats { get; set; }
    }
}