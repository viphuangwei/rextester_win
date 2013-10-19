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
        VC
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
    }

    public class Engine
    {
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
                using (Process process = new Process())
                using(var job = new Job())
                {
                    process.StartInfo.FileName = cdata.Executor + (string.IsNullOrEmpty(cdata.Executor) ? "" : " ") + cdata.ExecuteThis;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardInput = true;

                    process.Start();
                    job.AddProcess(process.Handle);

                    if (!string.IsNullOrEmpty(idata.Input))
                    {
                        InputWriter input = new InputWriter(process.StandardInput, idata.Input);
                        Thread inputWriter = new Thread(new ThreadStart(input.Writeinput));
                        inputWriter.Start();
                    }

                    OutputReader output = new OutputReader(process.StandardOutput);
                    Thread outputReader = new Thread(new ThreadStart(output.ReadOutput));
                    outputReader.Start();
                    OutputReader error = new OutputReader(process.StandardError);
                    Thread errorReader = new Thread(new ThreadStart(error.ReadOutput));
                    errorReader.Start();

                    var start = DateTime.Now;
                    bool killed = false;
                    do
                    {
                        // Refresh the current process property values.
                        process.Refresh();
                        if (!process.HasExited)
                        {
                            try
                            {
                                if (start + TimeSpan.FromSeconds(10) < DateTime.Now)
                                {
                                    process.Kill();
                                    var res = string.Format("Process killed because it ran longer than 10 seconds");
                                    odata.Errors = res;
                                    odata.Output = output.Builder.ToString();
                                    killed = true;
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

                    if (!killed)
                    {
                        errorReader.Join(5000);
                        outputReader.Join(5000);

                        if (process.ExitCode != 0)
                            error.Output = string.Format("Process exit code is not 0: {0}\n", process.ExitCode) + error.Output;

                        odata.Errors = error.Output;
                        odata.Output = output.Output;
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
                Directory.SetCurrentDirectory(RootPath);
                Directory.Delete(dir, true);
            }
            catch (Exception e) 
            {
                var a = e;
            }
        }
        CompilerData CreateExecutable(InputData input)
        {
            string ext = "";
            string rand = Utils.RandomString();
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
            CompilerData cdata = new CompilerData();
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
            {
                process.StartInfo.FileName = compiler;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                //process.StartInfo.CreateNoWindow = false;
                //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                watch.Start();
                process.Start();

                OutputReader output = new OutputReader(process.StandardOutput, 100);
                Thread outputReader = new Thread(new ThreadStart(output.ReadOutput));
                outputReader.Start();
                OutputReader error = new OutputReader(process.StandardError, 100);
                Thread errorReader = new Thread(new ThreadStart(error.ReadOutput));
                errorReader.Start();

                process.WaitForExit();
                watch.Stop();

                CompileTimeMs = watch.ElapsedMilliseconds;
                errorReader.Join(5000);
                outputReader.Join(5000);

                List<string> compOutput = new List<string>();
                compOutput.Add(output.Output);
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
}