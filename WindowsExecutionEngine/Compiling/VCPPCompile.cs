using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsExecutionEngine.Compiling
{
    public class VCPPCompile : ICompiler
    {
        #region ICompiler implementation
        public CompilerData Compile(InputData idata, CompilerData cdata)
        {
            string compiler = @"C:\inetpub\wwwroot\rextester\cl.bat";/*@"C:\Users\Renat\Desktop\win_service\cl.bat";*//*@"C:\Program Files\Microsoft Visual Studio 11.0\VC\bin\cl.exe"*/;
            if (string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("-o a.exe"))
            {
                cdata.Error = "Compiler args must contain '-o a.exe'";
                cdata.Success = false;
                return cdata;
            }
            if (string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("source_file.cpp"))
            {
                cdata.Error = "Compiler args must contain 'source_file.cpp'";
                cdata.Success = false;
                return cdata;
            }

            idata.Compiler_args = idata.Compiler_args.Replace("source_file.cpp", idata.PathToSource);
            idata.Compiler_args = idata.Compiler_args.Replace("-o a.exe", "-o " + idata.BaseDir + "a.exe");

            string args = idata.Compiler_args;
            long compileTime;
            var res = Engine.CallCompiler(compiler, args, out compileTime);
            cdata.CompileTimeMs = compileTime;
            if (!File.Exists(idata.BaseDir + "a.exe"))
            {
                if (res.Count > 1)
                {
                    cdata.Error = RemoveLines(Utils.ConcatenateString(res[0], res[1]), idata);
                }
                cdata.Success = false;
                return cdata;
            }
            if (res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
                cdata.Warning = RemoveLines(Utils.ConcatenateString(res[0], res[1]), idata);
            cdata.ExecuteThis = idata.BaseDir + "a.exe";
            cdata.Executor = "";
            cdata.Success = true;
            return cdata;
        }
        #endregion

        string RemoveLines(string text, InputData idata)
        {
            if(string.IsNullOrEmpty(text))
                return text;
            if (text.StartsWith("\r\n" + idata.BaseDir.Trim(@"\".ToCharArray())))
            {
                StringBuilder sb = new StringBuilder();
                text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(2).ToList().ForEach(f => sb.Append(f + "\r\n"));
                text = sb.ToString();
            }
            var regex = new Regex(@"Microsoft \(R\) C/C\+\+ Optimizing Compiler Version [\d\.]+ for x86");
            text = regex.Replace(text, "")
                        .Replace("Copyright (C) Microsoft Corporation.  All rights reserved.", "")
                        .Replace("cl : Command line warning D9035 : option 'o' has been deprecated and will be removed in a future release", "");

            regex = new Regex(@"Microsoft \(R\) Incremental Linker Version [\d\.]+");
            text = regex.Replace(text, "");

            regex = new Regex(@"^/out:\d+.exe\s*$", RegexOptions.Multiline);
            text = regex.Replace(text, "");

            regex = new Regex(@"^/out:"+idata.BaseDir.Replace(@"\", @"\\")+@"a.exe\s*$", RegexOptions.Multiline);
            text = regex.Replace(text, "");

            regex = new Regex(@"^\d+.obj\s*$", RegexOptions.Multiline);
            text = regex.Replace(text, "");

            StringBuilder sb2 = new StringBuilder();
            text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(f => 
            {
                if (!string.IsNullOrEmpty(f.Trim()))
                    sb2.Append(f + "\r\n");
            });
            
            return sb2.ToString();
        }
    }
}
