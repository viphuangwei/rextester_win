using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsExecutionEngine.Compiling
{
    public class ICompilerFactory
    {
        public static ICompiler GetICompiler(Languages lang)
        {
            switch (lang)
            {
                case Languages.VCPP:
                    return new VCPPCompile();
                case Languages.VC:
                    return new VCCompile();
                default:
                    return null;
            }
        }
    }
}
