using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Utils;
using System.Collections.Concurrent;

namespace reExp.Controllers.rundotnet
{
    public class RundotnetData
    {
        public LanguagesEnum LanguageChoice
        {
            get;
            set;
        }
        public EditorsEnum EditorChoice
        {
            get;
            set;
        }
        public string LanguageChoiceWrapper
        {
            get
            {
                return ((int)LanguageChoice).ToString();
            }
            set
            {
                LanguageChoice = (LanguagesEnum)Convert.ToInt32(value);
            }
        }
        public string EditorChoiceWrapper
        {
            get
            {
                return ((int)EditorChoice).ToString();
            }
            set
            {
                EditorChoice = (EditorsEnum)Convert.ToInt32(value);
            }
        }
        public List<SelectListItem> Editor
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem()
                {
                    Text = "CodeMirror",
                    Value = ((int)EditorsEnum.Codemirror).ToString()
                });
                list.Add(new SelectListItem()
                {
                    Text = "EditArea",
                    Value = ((int)EditorsEnum.Editarea).ToString()
                });
                list.Add(new SelectListItem()
                {
                    Text = "Simple",
                    Value = ((int)EditorsEnum.Simple).ToString()
                });
                return list;
            }
        }
        public bool IsIntellisense
        {
            get
            {
                if ((this.LanguageChoice == LanguagesEnum.CSharp ||
                    this.LanguageChoice == LanguagesEnum.Java ||
                    this.LanguageChoice == LanguagesEnum.CPP ||
                    this.LanguageChoice == LanguagesEnum.VCPP ||
                    this.LanguageChoice == LanguagesEnum.CPPClang ||
                    this.LanguageChoice == LanguagesEnum.Python) && this.EditorChoice == EditorsEnum.Codemirror)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsInterpreted
        {
            get
            {
                if (this.LanguageChoice == LanguagesEnum.Perl ||
                    this.LanguageChoice == LanguagesEnum.Php ||
                    this.LanguageChoice == LanguagesEnum.Python ||
                    this.LanguageChoice == LanguagesEnum.Ruby ||
                    this.LanguageChoice == LanguagesEnum.Lua ||
                    this.LanguageChoice == LanguagesEnum.SqlServer ||
                    this.LanguageChoice == LanguagesEnum.MySql ||
                    this.LanguageChoice == LanguagesEnum.Javascript ||

                    this.LanguageChoice == LanguagesEnum.Go ||
                    this.LanguageChoice == LanguagesEnum.Nodejs ||
                    this.LanguageChoice == LanguagesEnum.Prolog ||
                    this.LanguageChoice == LanguagesEnum.Scheme || 
                    this.LanguageChoice == LanguagesEnum.Lisp ||
                    this.LanguageChoice == LanguagesEnum.Scala ||
                    this.LanguageChoice == LanguagesEnum.Java ||
                    this.LanguageChoice == LanguagesEnum.Python3 ||
                    this.LanguageChoice == LanguagesEnum.Octave ||
                    this.LanguageChoice == LanguagesEnum.R ||
                    this.LanguageChoice == LanguagesEnum.Tcl)

                    return true;
                else
                    return false;
            }
        }
        public bool ShowInput
        {
            get
            {
                if (this.LanguageChoice == LanguagesEnum.SqlServer || this.LanguageChoice == LanguagesEnum.MySql)
                    return false;
                else
                    return true;
            }
        }
        public bool ShowCompilerArgs
        {
            get
            {
                if (this.LanguageChoice == LanguagesEnum.CPP || 
                    this.LanguageChoice == LanguagesEnum.C ||
                    this.LanguageChoice == LanguagesEnum.CPPClang ||
                    this.LanguageChoice == LanguagesEnum.CClang ||
                    this.LanguageChoice == LanguagesEnum.Go ||
                    this.LanguageChoice == LanguagesEnum.Haskell ||
                    this.LanguageChoice == LanguagesEnum.ObjectiveC ||
                    this.LanguageChoice == LanguagesEnum.VCPP ||
                    this.LanguageChoice == LanguagesEnum.VC ||
                    this.LanguageChoice == LanguagesEnum.D)
                    return true;
                else
                    return false;
            }
        }
        public string Compiler
        {
            get
            {
                switch (LanguageChoice)
                { 
                    case LanguagesEnum.C:
                        return "gcc";
                    case LanguagesEnum.CPP:
                        return "g++";
                    case LanguagesEnum.CClang:
                        return "clang";
                    case LanguagesEnum.CPPClang:
                        return "clang++";
                    case LanguagesEnum.Go:
                        return "go build";
                    case LanguagesEnum.Haskell:
                        return "ghc";
                    case LanguagesEnum.ObjectiveC:
                        return "gcc";
                    case LanguagesEnum.VCPP:
                        return "cl.exe";
                    case LanguagesEnum.VC:
                        return "cl.exe";
                    case LanguagesEnum.D:
                        return "dmd";
                    default:
                        return "";
                }
            }
        }
        public string CompilerArgs
        {
            get;
            set;
        }
        public string CompilerPostArgs
        {
            get;
            set;
        }
        public bool IsOutputInHtml
        {
            get
            {
                if (this.LanguageChoice == LanguagesEnum.SqlServer || this.LanguageChoice == LanguagesEnum.MySql)
                    return true;
                else
                    return false;
            }
        }
        public List<SelectListItem> Languages
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Text = "Assembly",
                        Value = ((int)LanguagesEnum.Nasm).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "C#",
                        Value = ((int)LanguagesEnum.CSharp).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "C++ (gcc)",
                        Value = ((int)LanguagesEnum.CPP).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "C++ (clang)",
                        Value = ((int)LanguagesEnum.CPPClang).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "C++ (vc++)",
                        Value = ((int)LanguagesEnum.VCPP).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "C (gcc)",
                        Value = ((int)LanguagesEnum.C).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "C (clang)",
                        Value = ((int)LanguagesEnum.CClang).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "C (vc)",
                        Value = ((int)LanguagesEnum.VC).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Common Lisp",
                        Value = ((int)LanguagesEnum.Lisp).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "D",
                        Value = ((int)LanguagesEnum.D).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "F#",
                        Value = ((int)LanguagesEnum.FSharp).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Go",
                        Value = ((int)LanguagesEnum.Go).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Haskell",
                        Value = ((int)LanguagesEnum.Haskell).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Java",
                        Value = ((int)LanguagesEnum.Java).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Javascript",
                        Value = ((int)LanguagesEnum.Javascript).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Lua",
                        Value = ((int)LanguagesEnum.Lua).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "MySql",
                        Value = ((int)LanguagesEnum.MySql).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Node.js",
                        Value = ((int)LanguagesEnum.Nodejs).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Octave",
                        Value = ((int)LanguagesEnum.Octave).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Objective-C",
                        Value = ((int)LanguagesEnum.ObjectiveC).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Pascal",
                        Value = ((int)LanguagesEnum.Pascal).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Perl",
                        Value = ((int)LanguagesEnum.Perl).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Php",
                        Value = ((int)LanguagesEnum.Php).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Prolog",
                        Value = ((int)LanguagesEnum.Prolog).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Python",
                        Value = ((int)LanguagesEnum.Python).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Python 3",
                        Value = ((int)LanguagesEnum.Python3).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "R",
                        Value = ((int)LanguagesEnum.R).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Ruby",
                        Value = ((int)LanguagesEnum.Ruby).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Scala",
                        Value = ((int)LanguagesEnum.Scala).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Scheme",
                        Value = ((int)LanguagesEnum.Scheme).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Sql Server",
                        Value = ((int)LanguagesEnum.SqlServer).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Tcl",
                        Value = ((int)LanguagesEnum.Tcl).ToString()
                    },
                    new SelectListItem()
                    {
                        Text = "Visual Basic",
                        Value = ((int)LanguagesEnum.VB).ToString()
                    },
                };
            }
        }
        public string Title
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
        public List<string> Errors
        {
            get;
            set;
        }

        public string WholeError
        {
            get;
            set;
        }

        public bool ShowWarnings
        {
            get;
            set;
        }

        public List<string> Warnings
        {
            get;
            set;
        }

        public string WholeWarning
        {
            get;
            set;
        }

        public GlobalConst.RundotnetStatus Status
        {
            get;
            set;
        }

        public string Output
        {
            get;
            set;
        }

        public List<string> Files
        {
            get;
            set;
        }

        public string GetInitialCode(LanguagesEnum language, EditorsEnum editor)
        {
            switch (language)
            {
                case LanguagesEnum.CSharp:
                    return
@"//Rextester.Program.Main is the entry point for your code. Don't change it.
//Compiler version 4.0.30319.17929 for Microsoft (R) .NET Framework 4.5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rextester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Your code goes here
            Console.WriteLine(""Hello, world!"");
        }
    }
}";

                case LanguagesEnum.VB:
                    return
@"'Rextester.Program.Main is the entry point for your code. Don't change it.
'Compiler version 11.0.50709.17929 for Microsoft (R) .NET Framework 4.5

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text.RegularExpressions

Namespace Rextester
    Public Module Program
        Public Sub Main(args() As string)
            'Your code goes here
            Console.WriteLine(""Hello, world!"")
        End Sub
    End Module
End Namespace";
                case LanguagesEnum.FSharp:
                    return
@"//Rextester.Program.Main is the entry point for your code. Don't change it.
//Compiler version 11.0.50727.1 for Microsoft (R) .NET Framework 4.5

namespace Rextester
module Program =
    open System
    let Main(args : string[]) =
        //Your code goes here
        Console.WriteLine(""Hello, world!"")";
                case LanguagesEnum.Java:
                    return
@"//'main' method must be in a class 'Rextester'.
//Compiler version 1.8.0_72

import java.util.*;
import java.lang.*;

class Rextester
{  
    public static void main(String args[])
    {
        System.out.println(""Hello, World!"");
    }
}";
                case LanguagesEnum.Python:
                    return
@"#python 2.7.6

print ""Hello, world!""
";
                case LanguagesEnum.Python3:
                    return
@"#python 3.4.3

print (""Hello, world!"")
";
                case LanguagesEnum.C:
                    return
@"//gcc 4.9.3

#include  <stdio.h>

int main(void)
{
    printf(""Hello, world!\n"");
    return 0;
}";
                case LanguagesEnum.CClang:
                    return
@"//clang 3.7.0

#include  <stdio.h>

int main(void)
{
    printf(""Hello, world!\n"");
    return 0;
}";
                case LanguagesEnum.CPP:
                    return
@"//g++  4.9.3

#include <iostream>

int main()
{
    std::cout << ""Hello, world!\n"";
}";

                case LanguagesEnum.CPPClang:
                    return
@"//clang 3.7.0

#include <iostream>

int main()
{
    std::cout << ""Hello, world!\n"";
}";
                case LanguagesEnum.VCPP:
                    return
@"//Microsoft (R) C/C++ Optimizing Compiler Version 19.00.23506 for x86

#include <iostream>

int main()
{
    std::cout << ""Hello, world!\n"";
}";
                case LanguagesEnum.VC:
                    return
@"//Microsoft (R) C/C++ Optimizing Compiler Version 19.00.23506 for x86

#include  <stdio.h>

int main(void)
{
    printf(""Hello, world!\n"");
    return 0;
}";

                case LanguagesEnum.Php:
                    return
@"<?php //php 5.5.9

    echo ""Hello, world! ""
    
?>";

                case LanguagesEnum.Pascal:
                    return
@"//fpc 2.6.2

program HelloWorld;

begin
    writeln('Hello, world!');
end.
";
                case LanguagesEnum.ObjectiveC:
                    return
@"//gcc 4.8.4

#import <stdio.h>
 
int main(void)
{
    printf(""Hello, world!\n"");
    return 0;
}";
                case LanguagesEnum.Haskell:
                    return
@"--ghc 8.0.1 /opt/ghc/8.0.1/lib/ghc-8.0.0.20160127/

main = print $ ""Hello, world!""";
                case LanguagesEnum.Ruby:
                    return
@"#ruby 1.9.3 

puts ""Hello, world!""";
                case LanguagesEnum.Perl:
                    return
@"#perl 5.18.2 

print ""Hello World\n"";";
   
                case LanguagesEnum.MySql:
                    return
@"#mysql 5.7.12
#please drop objects you've created at the end of the script 
#or check for their existance before creating

select version() as 'mysql version'";
                case LanguagesEnum.SqlServer:
                    return
@"--Sql Server 2014 Express Edition
--Batches are separated by 'go'

select @@version as 'sql server version'";
                case LanguagesEnum.Lua:
                    return
@"--lua 5.2.3

print (""Hello, World!"")";

                case LanguagesEnum.Nasm:
                    return
@";nasm 2.10.9

section .data
    hello:     db 'Hello world!',10    ; 'Hello world!' plus a linefeed character
    helloLen:  equ $-hello             ; Length of the 'Hello world!' string

section .text
	global _start

_start:
	mov eax,4            ; The system call for write (sys_write)
	mov ebx,1            ; File descriptor 1 - standard output
	mov ecx,hello        ; Put the offset of hello in ecx
	mov edx,helloLen     ; helloLen is a constant, so we don't need to say
	                     ;  mov edx,[helloLen] to get it's actual value
	int 80h              ; Call the kernel

	mov eax,1            ; The system call for exit (sys_exit)
	mov ebx,0            ; Exit with return code of 0 (no error)
	int 80h;";
                case LanguagesEnum.Javascript:
                    return @"
//V8 3.31.1

print(""Hello, world!"")";

                case LanguagesEnum.Lisp:
                    return @"
;gnu dmd 2.49

(print ""Hello, world!"")";

                case LanguagesEnum.Prolog:
                    return @"
%commands to the interpreter are submitted from stdin input ('show input' box below)
%'halt.' will be automatically appended to stdin input.
%swi-prolog 6.6.4

program :- write('Hello, world!').
:- program.";
                case LanguagesEnum.Go:
                    return @"
//go 1.2.1

package main  
import ""fmt"" 

func main() { 
    fmt.Printf(""hello, world\n"") 
}";
                case LanguagesEnum.Scala:
                    return @"
//'Rextester' class is the entry point for your code.
//Don't declare a package.
//scala 2.11.7

object Rextester extends App {
    println(""Hello, World!"")
 }";
                case LanguagesEnum.Scheme:
                    return @"
;guile 2.0.9

(display ""Hello, World!"")";

                case LanguagesEnum.Nodejs:
                    return @"
//nodejs 0.10.25

console.log(""Hello, World!"");";
                case LanguagesEnum.Octave:
                    return
@"%To view plots after 'plot' (and other plot-producing commands) this command must follow: 'print -dpng some_unique_plot_name.png;'
%It exports current plot to png image which then is sent to your browser
%GNU Octave 3.8.1

x=1:0.1:10;
plot(x, sin(x));
print -dpng some_name.png;
";
                case LanguagesEnum.D:
                    return
@"//DMD64 D Compiler v2.070

import std.stdio;
 
void main()
{
    writeln(""Hello, World!"");
}";

                case LanguagesEnum.R:
                    return
@"#R version 3.0.2 
  
print(""Hello, world!"")
";
                case LanguagesEnum.Tcl:
                    return
@"#tclsh 8.6

puts ""Hello, world!""
";
                default:
                    return @"";
            }
        }

        public string GetInitialCompilerArgs(LanguagesEnum language)
        {
            switch (language)
            { 
                case LanguagesEnum.C:
                    return "-Wall -std=gnu99 -O2 -o a.out source_file.c";
                case LanguagesEnum.CPP:
                    return "-Wall -std=c++14 -O2 -o a.out source_file.cpp";
                case LanguagesEnum.CClang:
                    return "-Wall -std=gnu99 -O2 -o a.out source_file.c";
                case LanguagesEnum.CPPClang:
                    return "-Wall -std=c++14 -stdlib=libc++ -O2 -o a.out source_file.cpp";
                case LanguagesEnum.VCPP:
                    return @"source_file.cpp -o a.exe /EHsc /MD /I C:\boost_1_60_0 /link /LIBPATH:C:\boost_1_60_0\stage\lib";
                case LanguagesEnum.VC:
                    return "source_file.c -o a.exe";
                case LanguagesEnum.Go:
                    return "-o a.out source_file.go";
                case LanguagesEnum.Haskell:
                    return "-o a.out source_file.hs";
                case LanguagesEnum.ObjectiveC:
                    return "-MMD -MP -DGNUSTEP -DGNUSTEP_BASE_LIBRARY=1 -DGNU_GUI_LIBRARY=1 -DGNU_RUNTIME=1 -DGNUSTEP_BASE_LIBRARY=1 -fno-strict-aliasing -fexceptions -fobjc-exceptions -D_NATIVE_OBJC_EXCEPTIONS -pthread -fPIC -Wall -DGSWARN -DGSDIAGNOSE -Wno-import -g -O2 -fgnu-runtime -fconstant-string-class=NSConstantString -I. -I /usr/include/GNUstep -I/usr/include/GNUstep -o a.out source_file.m -lobjc -lgnustep-base";
                case LanguagesEnum.D:
                    return "source_file.d -ofa.out";
                default:
                    return "";
            }
        }
        
        public string RunStats
        {
            get;
            set;
        }
        public string CodeGuid
        {
            get;
            set;
        }
        public string SavedOutput
        {
            get;
            set;
        }
        public string StatsToSave
        {
            get;
            set;
        }
        public bool IsLive
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public bool IsInEditMode
        {
            get;
            set;
        }
        public bool EditVisible
        {
            get;
            set;
        }
        public bool BackToForkVisible
        {
            get;
            set;
        }
        public int? Votes
        {
            get;
            set;
        }
        public bool IsOnAWall
        {
            get
            {
                return Votes != null;
            }
        }
        public string PrimaryGuid
        {
            get;
            set;
        }
        public bool LivesVersion
        {
            get;
            set;
        }
        public bool IsSaved
        {
            get;
            set;
        }
        public bool IsApi
        {
            get;
            set;
        }
        public int? User_Id
        {
            get;
            set;
        }
    }
}