using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsExecutionEngine;

namespace WindowsEngineTests
{
    class Program
    {
        static void Main(string[] args)
        {
            using (localhost.Service service = new localhost.Service())
            {
                var testProgram = TestProgram.GetTestPrograms().Where(f => f.Name.Contains("MySql_") && f.Name.Contains("_Hello")).Single();
                //TestEngineThroughService(testProgram.Program, testProgram.Input, testProgram.Lang, testProgram.Args);
                TestEngineDirectly(testProgram.Program, testProgram.Input, testProgram.Lang, testProgram.Args);
            }   
        }

        static void TestEngineThroughService(string Program, string Input, Languages Lang, string Args)
        {
            OutputData odata;
            bool bytes = true;

            using (var s = new localhost.Service())
            {

                Stopwatch watch = new Stopwatch();
                watch.Start();
                var res = s.DoWork(Program, Input, (localhost.Languages)Lang, GlobalUtils.TopSecret.Service_user, GlobalUtils.TopSecret.Service_pass, Args, bytes, false, false);

                watch.Stop();
                if (res != null)
                {
                    if (string.IsNullOrEmpty(res.Stats))
                        res.Stats = "";
                    else
                        res.Stats += ", ";
                    res.Stats += string.Format("absolute service time: {0} sec", Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2));
                }

                odata = new OutputData()
                {
                    Errors = res.Errors,
                    Warnings = res.Warnings,
                    Stats = res.Stats,
                    Output = res.Output,
                    Exit_Status = res.Exit_Status,
                    System_Error = res.System_Error
                };
                if (bytes)
                {
                    if (res.Errors_Bytes != null)
                        odata.Errors = System.Text.Encoding.Unicode.GetString(res.Errors_Bytes);
                    if (res.Warnings_Bytes != null)
                        odata.Warnings = System.Text.Encoding.Unicode.GetString(res.Warnings_Bytes);
                    if (res.Output_Bytes != null)
                        odata.Output = System.Text.Encoding.Unicode.GetString(res.Output_Bytes);
                }
            }
            ShowData(odata);
        }
        static void TestEngineDirectly(string Program, string Input, Languages Lang, string Args = null)
        {
            Engine engine = new Engine();
            InputData idata = new InputData()
            {
                Program = Program,
                Input = Input,
                Lang = Lang,
                Compiler_args = Args
            };
            var odata = engine.DoWork(idata);
            ShowData(odata);
        }

        static void ShowData(OutputData odata)
        {
            if (!string.IsNullOrEmpty(odata.System_Error))
            {
                Console.WriteLine("System error:");
                Console.WriteLine(odata.System_Error);
            }
            else
            {
                Console.WriteLine("Errors:");
                Console.WriteLine(odata.Errors);
                Console.WriteLine("Warnings:");
                Console.WriteLine(odata.Warnings);
                Console.WriteLine("Output:");
                Console.WriteLine(odata.Output);
                Console.WriteLine("Exit status:");
                Console.WriteLine(odata.Exit_Status);
                Console.WriteLine("Stats:");
                Console.WriteLine(odata.Stats);
            }
            Console.ReadLine();
        }

        class TestProgram
        {
            public string Name
            {
                get;
                set;
            }
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
            public string ShouldContain
            {
                get;
                set;
            }
            public string Args
            {
                get;
                set;
            }
            public static List<TestProgram> GetTestPrograms()
            {
                List<TestProgram> list = new List<TestProgram>();

                #region C and C++
                list.Add(new TestProgram()
                {
                    Program = @"
#include <iostream>

using namespace std;

int main () {
		cout << ""Hello, world!"";
}",
                    Lang = Languages.VCPP,
                    Name = "VCPP_Hello",
                    Args = "source_file.cpp -o a.exe /EHsc"
                });
                list.Add(new TestProgram()
                {
                    Program = @"
#include <iostream>
#include <fstream>
#include <windows.h>
#include <strsafe.h>
#include <direct.h>
#include <string.h>

using namespace std;

int main () {
  	STARTUPINFO si;
	PROCESS_INFORMATION pi;
	

	ZeroMemory( &si, sizeof(si) );
	si.cb = sizeof(si);
	ZeroMemory( &pi, sizeof(pi) );
	if (!CreateProcess(TEXT(""C:\\Users\\Renat\\Desktop\\test\\test.exe""), NULL,NULL,NULL,FALSE,NULL,NULL,NULL, &si, &pi))
	{
		cout << ""Unable to execute."";
	}
	else
	{
		while(true)
			;
	}
	cout << ""Hello, world"";
    return 0;
}",
                    Lang = Languages.VCPP,
                    Name = "VCPP_JOB_OBJECT",
                    Args = "source_file.cpp -o a.exe /EHsc"
                });

                list.Add(new TestProgram()
                {
                    Program = @"
#include  <stdio.h>

int main(void)
{
    printf(""Hello, world from C!\n"");
    return 0;
}",
                    Lang = Languages.VC,
                    Name = "VC_Hello",
                    Args = "source_file.c -o a.exe /EHsc"
                });

                #endregion

                #region MySql
                list.Add(new TestProgram()
                {
                    Program = @"selecty 5 ",
                    Lang = Languages.MySql,
                    Name = "MySql_Hello"
                });
                #endregion             

                return list;
            }
        }
    }
}
