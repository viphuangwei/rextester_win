using BookSleeve;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsSandbox
{
    class Program
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

        static void Main(string[] args)
        {
            //Console.OutputEncoding = Encoding.UTF8;
            //Console.InputEncoding = Encoding.UTF8;
            
            while (true)
            {
                try
                {
                    var keys = RedisConnection.Keys.Find(0, "*").Result;
                    foreach (var k in keys)
                    {
                        Console.WriteLine("Sandbox job: " + k);
                        var a = RedisConnection.Keys.Remove(0, k).Result;
                        var input = RedisConnection.Strings.Get(1, k).Result;
                        string inp = null;
                        if (input != null)
                        {
                            a = RedisConnection.Keys.Remove(1, k).Result;
                            inp = Encoding.UTF8.GetString(input);
                        }
                        var nr = k;
                        Console.WriteLine("Sandbox job scheduele: " + k);

                        ThreadPool.QueueUserWorkItem(f => {
                            ExecuteCode(nr, inp);
                        });
                        //ExecuteCode(nr, inp);
                    }
                    Thread.Sleep(200);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Main error: " + e.Message);
                    Thread.Sleep(200);
                }
            }
        }

        static void ExecuteCode(string nr, string input)
        {
            try
            {
                string path = @"C:\inetpub\wwwroot\rextester\usercode\";
                Console.WriteLine("Sandbox start: " + nr);
                using (Process process = new Process())
                using (var job = new Job())
                {
                    process.StartInfo.FileName = Path.Combine(path, nr, "a.exe");

                    Console.WriteLine(process.StartInfo.FileName);

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardInput = true;

                    Console.WriteLine("Sandbox: job start " + nr);

                    process.Start();
                    job.AddProcess(process.Handle);

                    if (!string.IsNullOrEmpty(input))
                    {
                        InputWriter iw = new InputWriter(process.StandardInput, input);
                        Thread inputWriter = new Thread(new ThreadStart(iw.Writeinput));
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
                    string Errors = "";
                    string Output = "";

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
                                    Errors = res;
                                    Output = output.Builder.ToString();
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

                    Console.WriteLine("Sandbox: job over " + nr);

                    if (!killed)
                    {
                        errorReader.Join(5000);
                        outputReader.Join(5000);

                        if (process.ExitCode != 0)
                            error.Output = string.Format("Process exit code is not 0: {0}\n", process.ExitCode) + error.Output;

                        Errors = error.Output;
                        Output = output.Output;
                    }

                    if (!string.IsNullOrEmpty(Errors))
                    {
                        Console.WriteLine("Sandbox: writing errors "  + nr);
                        RedisConnection.Strings.Set(3, nr, Encoding.UTF8.GetBytes(Errors));
                    }

                    if (!string.IsNullOrEmpty(Output))
                    {
                        Console.WriteLine("Sandbox: writing output " + nr);
                        RedisConnection.Strings.Set(2, nr, Encoding.UTF8.GetBytes(Output));                    
                    }

                    RedisConnection.Strings.Set(4, nr, new byte[] { (byte)1 });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sandbox: error " + nr +" " + ex.Message);
                try
                {
                    RedisConnection.Strings.Set(4, nr, new byte[] { (byte)1 });
                }
                catch (Exception) { }

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
                int count = 0;
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
