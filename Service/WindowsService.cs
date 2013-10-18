using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class WindowsService
    {
        public win.Result DoWork(string Program, string Input, string Compiler_args, win.Languages Language)
        {
            using (var service = new win.Service())
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
                    var res = service.DoWork(Program, Input, Language, GlobalUtils.TopSecret.Service_user, GlobalUtils.TopSecret.Service_pass, Compiler_args, bytes, ProgramCompressed, InputCompressed);

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
                    return new win.Result()
                    {
                        System_Error = string.Format("Error while calling service: {0}", ex.Message)
                    };
                }
            }
        }

    }
}
