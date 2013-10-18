using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsExecutionEngine.Compiling
{
    public interface ICompiler
    {
        CompilerData Compile(InputData idata, CompilerData cdata);
    }
}
