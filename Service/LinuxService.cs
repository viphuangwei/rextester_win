using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class LinuxService
    {
        public linux.Result DoWork(string Program, string Input, string Compiler_args, linux.Languages Language)
        {
            using (var service = new linux.Service())
            {
                try
                {
                    bool ProgramCompressed = false;
                    if (!string.IsNullOrEmpty(Program) && Program.Length > 1000)
                    {
                        ProgramCompressed = true;
                        Program = GlobalUtils.Utils.Compress(Program);
                    }
                    bool InputCompressed = false;
                    if (!string.IsNullOrEmpty(Input) && Input.Length > 1000)
                    {
                        InputCompressed = true;
                        Input = GlobalUtils.Utils.Compress(Input);
                    }


                    bool bytes = true;
                    var res = service.DoWork(Program, Input, Language, GlobalUtils.TopSecret.Linux_user, GlobalUtils.TopSecret.Linux_pass, Compiler_args, bytes, ProgramCompressed, InputCompressed);

                    if (bytes)
                    {
                        if (res.Errors_Bytes != null)
                            res.Errors = System.Text.Encoding.Unicode.GetString(res.Errors_Bytes);
                        if (res.Warnings_Bytes != null)
                            res.Warnings = System.Text.Encoding.Unicode.GetString(res.Warnings_Bytes);
                        if (res.Output_Bytes != null)
                            res.Output = System.Text.Encoding.Unicode.GetString(res.Output_Bytes);
                    }
                    if (res.IsOutputCompressed)
                        res.Output = GlobalUtils.Utils.Decompress(res.Output);

                    return res;
                }
                catch (Exception ex)
                {
                    return new linux.Result()
                    {
                        System_Error = string.Format("Error while calling service: {0}", ex.Message)
                    };
                }
            }
        }

        public linux.DiffResult GetDiff(string left, string right)
        {
            using (var service = new linux.Service())
            {
                try 
                {
                    var res = service.Diff(GlobalUtils.Utils.Compress(left), GlobalUtils.Utils.Compress(right), GlobalUtils.TopSecret.Linux_user, GlobalUtils.TopSecret.Linux_pass);
                    res.Result = GlobalUtils.Utils.Decompress(res.Result);
                    return res;
                }
                catch (Exception ex)
                {
                    return new linux.DiffResult()
                    {
                        IsError = true,
                        Result = string.Format("Error while calling diff service: {0}", ex.Message)
                    };
                }
            }
        }

        public string GetCppCompletions(string source, int line, int column)
        {
            using (var service = new linux.Service())
            {
                try
                {
                    var res = service.GetCPPCompletions(GlobalUtils.Utils.Compress(source), line, column);
                    return GlobalUtils.Utils.Decompress(res);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
    }
}
