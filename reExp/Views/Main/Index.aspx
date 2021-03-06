﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.main.MainData>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    About rextester
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width: 95%;">
        <h2>About</h2>
        Rextester - some online tools for anyone who finds them useful. It was started as <a href="http://rextester.com/tester">online .net regex tester</a>. Rextester stands for <b>r</b>egular <b>ex</b>pression <b>tester</b>.
        <br/>
        <a href="http://rextester.com/main/faq/">Short faq and troubleshooting.</a> <br/>
        <a href="https://groups.google.com/forum/#!forum/rextester">Discussion forum.</a><br/><br/>

        <table>
            <tr>
                <td>
                    Source code: <a href="https://github.com/ren85/rextester_win">part I</a> and <a href="https://github.com/ren85/rextester_linux">part II</a>
                </td>
            </tr>
            <tr>
                <td>
                    <form action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_top">
                        <input type="hidden" name="cmd" value="_s-xclick">
                        <input type="hidden" name="hosted_button_id" value="5GVYLXYXTHCZA">
                        <input type="image" src="https://www.paypalobjects.com/en_US/i/btn/btn_donate_SM.gif" border="0" name="submit" alt="PayPal - The safer, easier way to pay online!">
                        <img alt="" border="0" src="https://www.paypalobjects.com/en_US/i/scr/pixel.gif" width="1" height="1">
                    </form>
                </td>
            </tr>
        </table>
        <br/>
        <br/>
        <div style="padding-left: 2em">
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Tester)%>">Regex tester</a> - .net regex tester. <br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Replace)%>">Regex replace</a> - .net regex replacement.<br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Reference)%>">Regex reference</a> - short regex reference.<br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Diff)%>">Diff checker</a> - find difference between two text fragments.<br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Rundotnet)%>">Run code</a> - online compiling and execution for some languages.
            <br/><br/><b style="color:Gray">C#, Visual basic</b><br/>
            .Net framework v. 4.5 is used. Your code will be given max 5 sec of cpu time and limited memory (~150 Mb). Also your code will run in an appdomain with basic execution, reflection, thread control and web privileges only.<br/>
            The entry point for your code is given Main method in type Program in namespace Rextester. <b>This entry point shouldn't be changed.</b>
            Types from following assemblies are available:
             <ul>
                <li><code>System.dll</code></li>
                <li><code>System.Core.dll</code></li>
                <li><code>System.Data.dll</code></li>
                <li><code>System.Data.DataSetExtensions.dll</code></li>
                <li><code>System.Xml.dll</code></li>
                <li><code>System.Xml.Linq.dll</code></li>
                <li><code>System.Numerics.dll</code></li>
                <li><code>Microsoft.CSharp.dll - when C# is used</code></li>
                <li><code>Microsoft.VisualBasic.dll - when Visual Basic is used</code></li>
                <li><code>System.Net.dll</code></li>
                <li><code>System.Web.dll</code></li>
                <li><code>System.ComponentModel.DataAnnotations.dll</code></li>
                <li><code>System.ComponentModel.Composition.dll</code></li>
                <li><code>System.Drawing.dll</code></li>
                <li><code>Newtonsoft.Json.dll</code></li>
                <li><code><a href="http://rextester.com/feedback/">Let us know if you need more.</a></code></li>
            </ul>
            If you found security breaches and can break something in some way - we would appreciate your feedback on this.
            <br/>Compiler versions:
            <ul>
                <li><code><a href="http://rextester.com/l/csharp_online_compiler">C#</a> - <a href="http://rextester.com/l/csharp">Microsoft (R) Visual C# Compiler version 4.0.30319.17929 for Microsoft (R) .NET Framework 4.5</a></code></li>
                <li><code><a href="http://rextester.com/l/visual_basic_online_compiler">Visual Basic</a> - <a href="http://rextester.com/l/vb">Microsoft (R) Visual Basic Compiler version 11.0.50709.17929</a></code></li>            </ul>
            <br/><b style="color:Gray">Sql Server</b><br/>
            <a href="http://rextester.com/l/sql_server_online_compiler">Sql Server</a> <a href="http://rextester.com/l/sql_server">2014 Express Edition</a> is used. There is only one database that queries run against. Queries executed on behalf database owner so all sort of actions are allowed including DDL queries.
            However, all actions run in transaction which is rolled back immediately after execution is over. This way any desired objects may be built, populated with data and worked on within the scope 
            of one request. There is 10 seconds limit for work to be completed. Execution of queries is achieved using ado.net mechanisms, in particular SqlDataReader type. 
            This reader is also monitored for memory and cpu consumption. <br/>
            'GO' statement just like in transact sql has special meaning: 'GO' will be removed from your code, but statements separated by 'GO' will be executed 
            separately, in different batches. So, for example, if you want to create a function and later use it in select statement you have to separate create statement and select statement by 'GO', 
            otherwise you'll receive an error from parser since in select statement you would be reffering to a function which doesn't yet exist.<br/>
            For convinience there is simple pre-built schema, shown <a href="/Content/Schema.png">here</a>.
            <br/><b style="color:Gray">MySQL</b><br/>
            <a href="http://rextester.com/l/mysql_online_compiler">MySQL</a> <a href="http://rextester.com/l/mysql">version  5.7.12</a> is used (on Windows). There is only one database that queries run against. Queries executed on behalf database owner so all sort of actions are allowed including DDL queries.
            However, all actions run in transaction which is rolled back immediately after execution is over. This way any desired objects may be built, populated with data and worked on within the scope 
            of one request. There is 10 seconds limit for work to be completed. Unfortunately MySQL doesn't support rollback of DDL statements, so once object is created it stays. Therefore one should check if object exists before creating it and ideally drop it at the end of the script.
            <br/><b style="color:Gray">PostgreSQL</b><br/>
            <a href="http://rextester.com/l/postgresql_online_compiler">PostgreSQL</a> setup is simillar to Sql Server.
            <br/><b style="color:Gray">Oracle</b><br/>
             <a href="http://rextester.com/l/oracle_online_compiler">Oracle</a> setup is simillar to MySQL.

            <br/><br/><b style="color:Gray">Visual C++ (and C)</b><br/>
            Your code is compiled to native binary which runs on Windows Server 2012 (maximum compile time is 30 seconds). Your process will be associated with job object that has <code>LimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE</code> flag set.
            After 10 seconds of execution this process will be killed. This used to be the only security measures for a while. However, after discovering dubious services running and questionable .exes at some weird places it was decided to sandbox
            all usercode through <a href="http://www.sandboxie.com/">Sandboxie</a>. <br/> 
            Also let us know if you need some other Windows-based compilers.
            <br/>Compiler version:
            <ul>
                <li><code><a href="http://rextester.com/l/cpp_online_compiler_visual">C++ (vc++)</a> and <a href="http://rextester.com/l/c_online_compiler_visual">C</a> - <a href="http://rextester.com/l/vcpp">Microsoft (R) C/C++ Optimizing Compiler Version 19.00.23506 for x86</a></code></li>
            </ul>
            <br/><b style="color:Gray">Client Side</b><br/>
            This option allows you to create full javascript/css/html fiddle that runs in your browser. The result is displayed in a separate iframe. You can use anything that runs on a client side. Errors are examined in your browser's developer console.<br/>
            <br/><b style="color:Gray">Java, Python, C, C++ and others</b><br/>
            These languages run on linux. For some languages compiler parameters could be supplied. Here are compiler versions (you can always check by <a href="http://rextester.com/CLSPB84560">running commands on a server</a>):
            <ul>
                <li><code><a href="http://rextester.com/l/ada_online_compiler">Ada</a> - <a href="http://rextester.com/l/ada">GNAT 4.9.3</a></code></li>
                <li><code><a href="http://rextester.com/l/nasm_online_compiler">Assembly</a> - <a href="http://rextester.com/l/nasm">nasm 2.11.08</a></code></li>
                <li><code><a href="http://rextester.com/l/bash_online_compiler">Bash</a> - <a href="http://rextester.com/l/bash">GNU bash, version 4.3.46</a></code></li>
                <li><code><a href="http://rextester.com/l/cpp_online_compiler_gcc">C++ (gcc)</a> - <a href="http://rextester.com/l/gcc">g++  5.4.0 (g++ -Wall -std=c++11 -O2)</a></code></li>
                <li><code><a href="http://rextester.com/l/cpp_online_compiler_clang">C++ (clang)</a> - <a href="http://rextester.com/l/clang">clang 3.8.0 (clang++ -Wall -std=c++11 -O2)</a></code></li>
                <li><code><a href="http://rextester.com/l/c_online_compiler_gcc">C (gcc)</a> - <a href="http://rextester.com/l/c_gcc">gcc 5.4.0 (gcc -Wall -std=gnu99 -O2)</a></code></li>
                <li><code><a href="http://rextester.com/l/c_online_compiler_clang">C (clang)</a> - <a href="http://rextester.com/l/c_clang">clang 3.8.0 (clang -Wall -std=gnu99 -O2)</a></code></li>
                <li><code><a href="http://rextester.com/l/common_lisp_online_compiler">Common Lisp</a> - <a href="http://rextester.com/l/clisp">gnu clisp 2.49</a></code></li>
                <li><code><a href="http://rextester.com/l/d_online_compiler">D</a> - <a href="http://rextester.com/l/d">DMD64 D Compiler v2.072.2</a></code></li>
                <li><code><a href="http://rextester.com/l/elixir_online_compiler">Elixir</a> - <a href="http://rextester.com/l/elixir">Elixir 1.1.0</a></code></li>
                <li><code><a href="http://rextester.com/l/erlang_online_compiler">Erlang</a> - <a href="http://rextester.com/l/erlang">Erlang 7.3</a></code></li>
                <li><code><a href="http://rextester.com/l/fsharp_online_compiler">F#</a> - <a href="http://rextester.com/l/fsharp">F# Compiler for F# 4.0 (Open Source Edition), Mono 4.2.1</a></code></li>
                <li><code><a href="http://rextester.com/l/go_online_compiler">Go</a> - <a href="http://rextester.com/l/go">go 1.6.2</a></code></li>
                <li><code><a href="http://rextester.com/l/haskell_online_compiler">Haskell</a> - <a href="http://rextester.com/l/haskell">ghc 7.10.3</a></code></li>
                <li><code><a href="http://rextester.com/l/java_online_compiler">Java</a> - <a href="http://rextester.com/l/java">1.8.0_111 (javac -Xlint -encoding UTF-8 &nbsp;|&nbsp; java -Xmx256m -Dfile.encoding=UTF-8)</a></code></li>
                <li><code><a href="http://rextester.com/l/js_online_compiler">Javascript</a> - <a href="http://rextester.com/l/js">JavaScript-C24.2.0 (SpiderMonkey)</a></code></li>
                <li><code><a href="http://rextester.com/l/lua_online_compiler">Lua</a> - <a href="http://rextester.com/l/lua">lua 5.3</a></code></li>
                <li><code><a href="http://rextester.com/l/nodejs_online_compiler">Node.js</a> - <a href="http://rextester.com/l/nodejs">nodejs v4.2.6</a></code></li>
                <li><code><a href="http://rextester.com/l/objectivec_online_compiler">Objective-C</a> - <a href="http://rextester.com/l/objectivec">gcc 5.4.0 (gcc `gnustep-config --objc-flags` -lobjc -lgnustep-base)</a></code></li>
                 <li><code><a href="http://rextester.com/l/ocaml_online_compiler">Ocaml</a> - <a href="http://rextester.com/l/ocaml">Ocaml 4.02.3</a></code></li>
                <li><code><a href="http://rextester.com/l/octave_online_compiler">Octave</a> - <a href="http://rextester.com/l/octave">GNU Octave 4.0.0 (octave -q -f --no-window-system)</a></code></li>
                <li><code><a href="http://rextester.com/l/pascal_online_compiler">Pascal</a> - <a href="http://rextester.com/l/pascal">fpc 3.0.0</a></code></li>
                <li><code><a href="http://rextester.com/l/perl_online_compiler">Perl</a> - <a href="http://rextester.com/l/perl">perl 5.22.1 (perl -w)</a></code></li>
                <li><code><a href="http://rextester.com/l/php_online_compiler">Php</a> - <a href="http://rextester.com/l/php">php 7.0.8</a></code></li>
                <li><code><a href="http://rextester.com/l/prolog_online_compiler">Prolog</a> - <a href="http://rextester.com/l/prolog">swi-prolog 7.2.3</a></code></li>
                <li><code><a href="http://rextester.com/l/python_online_compiler">Python</a> - <a href="http://rextester.com/l/python">python 2.7.12</a></code></li>
                <li><code><a href="http://rextester.com/l/python3_online_compiler">Python 3</a> - <a href="http://rextester.com/l/python3">python3 3.5.2</a></code></li>
                <li><code><a href="http://rextester.com/l/r_online_compiler">R</a> - <a href="http://rextester.com/l/r">R version 3.3.2</a></code></li>
                <li><code><a href="http://rextester.com/l/ruby_online_compiler">Ruby</a> - <a href="http://rextester.com/l/ruby">ruby 2.3.1  (ruby -w -W1)</a></code></li>
                <li><code><a href="http://rextester.com/l/scala_online_compiler">Scala</a> - <a href="http://rextester.com/l/scala">scala 2.11.7 (fsc -deprecation -unchecked -encoding UTF-8 &nbsp;|&nbsp; scala -Dfile.encoding=UTF-8)</a></code></li>
                <li><code><a href="http://rextester.com/l/scheme_online_compiler">Scheme</a> - <a href="http://rextester.com/l/scheme">guile 2.0.11</a></code></li>
                <li><code><a href="http://rextester.com/l/swift_online_compiler">Swift</a> - <a href="http://rextester.com/l/swift">swift 3.0.0</a></code></li>
                <li><code><a href="http://rextester.com/l/tcl_online_compiler">Tcl</a> - <a href="http://rextester.com/l/tcl">tclsh 8.6</a></code></li>
            </ul> 
            Your code will be run on behalf special user and group. Also your code will be executed from Python wrapper which sets various limits to the process. It does so
            by using <code>'setrlimit'</code> system call. You'll have max 30 sec to compile, max 5 sec of cpu time to run, limited memory (~1500 Mb) and other restrictions will apply (like no writing permissions). Also your process and all its children will be run in a
            newly created process group which will be terminated after 10 seconds from start if still running.<br/>
            <br/>We don't claim that this is secure. In many senses you'll have the power of special user. On a bright side, this has some <a href="http://rextester.com/runcode?code=KAKN22727">useful</a> side-effects. The reason why, at least for now, 
            we leave so many potential security breaches is because it's <b>hard</b> to make it really secure. What are the options? 
            <ul>
                <li><a href="http://codepad.org/">Codepad</a> seems to <a href="http://rextester.com/runcode?code=JAJ71205"><code>'ptrace'</code></a>
                    everything and disallow many system calls, for example, <code>'fork()'</code>. That's pretty restrictive.</li>
                <li>Apparmor - out of everything we tried, this did the job pretty well and it was rather easy to configure. But it doesn't work with OpenVZ virtualization that is used by our server provider.</li>
                <li>SELinux - stopped reading documentation in the middle of it. Too (unnecessarily?) complex.</li>
                <li>Native security mechanisms - like chroot and file permissions. Hard to make it secure this way without breaking the service and the system.</li>
                <li>Chromium sandboxes that exist both for windows and linux. These sandboxes run chrome's page renderers and probably quite secure. Drawbacks are: hard to setup and possibly too limiting (however this needs to be verified)</li>
            </ul>
            So, if you can take the system down - then be it, but please report how you did this. Your advice on this topic is always <a href="http://rextester.com/feedback/">welcome</a>.
            <br/><br/>
            <b style="color:Gray">Input support</b><br/>
            Everything that will be submitted as input will be piped to process via <code>stdin</code> stream. So your code should consume input as if it came from keyboard.
            <br/><br/>
            <b style="color:Gray">Live collaboration</b><br/>
            Write code so that others see this real-time. Every participant can make changes and see changes made by others. We use <a href="http://www.firepad.io">Firepad</a> and <a href="https://www.firebase.com/">Firebase</a>.
            <br/><br/>
            <b style="color:Gray">Credit</b><br/>
            Special thanks goes to people behind <a href="http://codemirror.net/">CodeMirror</a>, <a href="http://www.cdolivet.com/editarea/">Edit area</a>, <a href="http://en.wikipedia.org/wiki/Microsoft_Roslyn">Roslyn</a>, <a href="http://www.toptensoftware.com/markdowndeep/">MarkdownDeep</a>, <a href="http://www.firepad.io">Firepad (and Firebase)</a> and <a href="http://code.google.com/p/coderev/">Coderev</a>.<br/><br />
            <b style="color:Gray">Code wall</b><br/>
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Codewall)%>">Code wall</a> as well as <a href="<%:Utils.GetUrl(Utils.PagesEnum.Users)%>">personal code walls</a> - place code on a wall for public display. These entries will be crawled by search engines, so one 
            possible use is to put there scripts that you may need for easy access later. For example, whenever I need a sql script for searching database definitions I simply search for <a href="https://www.google.com/search?q=sql+definition+rextester">'sql definition rextester'</a> and there is my script (it seems that code wall is 
            crawled better than personal walls, though). It's important that code snippet have a meaningful title.
            <br /> <br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Feedback)%>">Feedback</a> - give us feedback. We have implemented a lot of requests so far.<br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Login)%>">Login</a> - once logged in you'll be able to track your saved snippets and more. <br />
            <br/><br/>
            <b style="color:Gray">Stats (since 2013-08-15)</b><br/>
            <table style="border-color:gray;">
                <tr>
                    <td>
                        Language&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        Runs&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <%
                long sum = Model.LangCounters.Sum(f => f.Value);
                sum = sum == 0 ? 1 : sum;
                foreach(var lang in Model.LangCounters.OrderByDescending(f => f.Value)) 
                {
                    int filled = (int)Math.Round((double)Model.LangCounters[lang.Key]*100 / (double)sum);
                    %>
                    <tr>
                        <td><%:lang.Key%></td>
                        <td><%:lang.Value%></td>
                        <%for(int i = 0; i < filled; i++)
                        {%>
                             <td style="width:1px; background-color:grey;"></td>    
                       <%}%>
                        <%for(int i = 0; i < 100 - filled; i++)
                        {%>
                             <td style="width:1px;"></td>    
                       <%}%>
                    </tr>    
                <%}%>
            </table>
            <br/>
            <b style="color:Gray">Api</b><br/>
            Restfull api is supported (POST only) at <code>http://rextester.com/rundotnet/api</code>. What needs to be supplied are these values (as http data name=val&name2=val2, content type header must <b>not</b> indicate json):
<pre>
    LanguageChoice=Language number (see below)
    Program=Code to run
    Input=Input to be supplied to stdin of a process
    CompilerArgs=compiler args as one string (when applicable)
</pre>
            Returned is json string with the following properties:
<pre>
    Result=Output of a program (in case of Sql Server - html)
    Warnings=Warnings, if any, as one string
    Errors=Errors, if any, as one string
    Stats=Execution stats as one string
    Files=In case of Octave and R - list of png images encoded as base64 strings
</pre>
            Language numbers:
<pre>
    C# = 1
    VB.NET = 2
    F# = 3
    Java = 4
    Python = 5
    C (gcc) = 6
    C++ (gcc) = 7
    Php = 8
    Pascal = 9
    Objective-C = 10
    Haskell = 11
    Ruby = 12
    Perl = 13
    Lua = 14
    Nasm = 15
    Sql Server = 16
    Javascript = 17
    Lisp = 18
    Prolog = 19
    Go = 20
    Scala = 21
    Scheme = 22
    Node.js = 23
    Python 3 = 24
    Octave = 25
    C (clang) = 26
    C++ (clang) = 27    
    C++ (vc++) = 28
    C (vc) = 29
    D = 30
    R = 31
    Tcl = 32
    MySQL = 33
    PostgreSQL = 34
    Oracle = 35
    Swift = 37
    Bash = 38
    Ada = 39
    Erlang = 40
    Elixir = 41
    Ocaml = 42
</pre>
            
            <br/>
    Full javascript example:
<pre>
<%=HttpUtility.HtmlEncode(
@"<!DOCTYPE html>
<html>
<body>

    <head>
    <script src=""https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js""></script>
    <script>
    $(document).ready(function(){
        $(""button"").click(function(){

		    var to_compile = {
			    ""LanguageChoice"": ""1"",
			    ""Program"": $(""#code"").val(),
			    ""Input"": """",
			    ""CompilerArgs"" : """"
		    };

		    $.ajax ({
			        url: ""http://rextester.com/rundotnet/api"",
			        type: ""POST"",
			        data: to_compile
			    }).done(function(data) {
			        alert(JSON.stringify(data));
			    }).fail(function(data, err) {
			        alert(""fail "" + JSON.stringify(data) + "" "" + JSON.stringify(err));
		        });
	    });
    });
    </script>
    </head>

    <textarea id=""code"">
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
        }      
    </textarea>
    <button id=""run"">Run</button>

</body>
</html>")%>
    </pre>
            <br/>
            Api stats:
            <table style="border-color:gray;">
                <tr>
                    <td>
                        Language&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        Runs&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <%
                sum = Model.ApiLangCounters.Sum(f => f.Value);
                sum = sum == 0 ? 1 : sum;
                foreach(var lang in Model.ApiLangCounters.OrderByDescending(f => f.Value)) 
                {
                    int filled = (int)Math.Round((double)Model.ApiLangCounters[lang.Key]*100 / (double)sum);
                    %>
                    <tr>
                        <td><%:lang.Key%></td>
                        <td><%:lang.Value%></td>
                        <%for(int i = 0; i < filled; i++)
                        {%>
                             <td style="width:1px; background-color:grey;"></td>    
                       <%}%>
                        <%for(int i = 0; i < 100 - filled; i++)
                        {%>
                             <td style="width:1px;"></td>    
                       <%}%>
                    </tr>    
                <%}%>
            </table>
        </div>  
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Keywords" content=".net regex tester, online c#, vb, php, java, python, c, c++ code compiler, online code execution, rundotnet, runcode, online compiler, run code online, snippet, run your code online, programming online, run code, run snippet, execute snippet, execute code, C#, C++, Java, Javascript, Python, Pascal, Ruby, Lua, Perl, Haskell, Assembly" />
    <meta name="Description" content="Rextester - test .net regexes, run code snippets in many languages from your browser." />    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
