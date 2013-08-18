<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.main.MainData>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    About rextester
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Search</h2>
    <div style="width: 95%;">
        <!-- Place this tag where you want both of the search box and the search results to render -->
        <gcse:search></gcse:search>
        <br/>
        <h2>About</h2>
        Rextester - some online tools for anyone who finds them useful. It was started as online .net regex tester.
        <br/>
        <a href="http://rextester.com/main/faq/">Short faq and troubleshooting.</a> <br/><br/>
        <div style="padding-left: 2em">
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Tester)%>">Regex tester</a> - .net regex tester. <br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Replace)%>">Regex replace</a> - .net regex replacement.<br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Reference)%>">Regex reference</a> - short regex reference.<br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Diff)%>">Diff checker</a> - find difference between two text fragments.<br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Rundotnet)%>">Run code</a> - online compiling and execution for some languages.
            <br/><br/><b style="color:Gray">C#, Visual basic, F#</b><br/>
            .Net framework v. 4.5 is used. Your code will be given max 5 sec of cpu time and limited memory (~150 Mb). Also your code will run in an appdomain with basic execution, reflection, thread control and web privileges only.<br/>
            The entry point for you code is given Main method in type Program in namespace Rextester. <b>This entry point shouldn't be changed.</b>
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
                <li><code><a href="http://rextester.com/feedback/">Let us know if you need more.</a></code></li>
            </ul>
            For F# additional assemblies are included:
             <ul>
                <li><code>FSharp.Core.dll</code></li>
                <li><code>FSharp.Powerpack.dll</code></li>
                <li><code>FSharp.PowerPack.Compatibility.dll</code></li>
                <li><code>FSharp.PowerPack.Linq.dll</code></li>
                <li><code>FSharp.PowerPack.Metadata.dll</code></li>
                <li><code>FSharp.PowerPack.Parallel.Seq.dll</code></li>
            </ul>
            If you found security breaches and can break something in some way - we would appreciate your feedback on this.
            <br/><br/><b style="color:Gray">Sql Server</b><br/>
            Sql Server 2012 Express Edition is used. There is only one database that queries run against. Queries executed on behalf database owner so all sort of actions are allowed including DDL queries.
            However, all actions run in transaction which is rolled back immediately after execution is over. This way any desired objects may be built, populated with data and worked on within the scope 
            of one request. There is 10 seconds limit for work to be completed. Execution of queries is achieved using ado.net mechanisms, in particular SqlDataReader type. 
            This reader is also monitored for memory and cpu consumption. <br/>
            'GO' statement just like in transact sql has special meaning: 'GO' will be removed from your code, but statements separated by 'GO' will be executed 
            separately, in different batches. So, for example, if you want to create a function and later use it in select statement you have to separate create statement and select statement by 'GO', 
            otherwise you'll receive an error from parser since in select statement you would be reffering to a function which doesn't yet exist.<br/>
            For convinience there is simple pre-built schema, shown <a href="../../Content/Schema.png">here</a>.
            <br/><br/><b style="color:Gray">Java, Python, C, C++ and others</b><br/>
            These languages run on linux. For some languages compiler parameters could be supplied. Here are compiler versions (you can always check by <a href="http://rextester.com/CLSPB84560">running commands on a server</a>):
            <ul>
                <li><code>Assembly - nasm 2.09.04</code></li>
                <li><code>C++ (gcc) - g++ 4.7.2 (g++ -Wall -std=c++11 -O2)</code></li>
                <li><code>C++ (clang) - clang 3.0-6 (clang++ -Wall -std=c++11 -O2)</code></li>
                <li><code>C (gcc) - gcc 4.7.2 (gcc -Wall -std=gnu99 -O2)</code></li>
                <li><code>C (clang) - clang 3.0-6 (clang -Wall -std=gnu99 -O2)</code></li>
                <li><code>Common Lisp - gnu clisp 2.49</code></li>
                <li><code>Go - go 1.0.2</code></li>
                <li><code>Haskell - ghc 7.4.2</code></li>
                <li><code>Java - Oracle's implementation of Java, compiler version 1.7.0_17 (javac -Xlint -encoding UTF-8 &nbsp;|&nbsp; java -Xmx256m -Dfile.encoding=UTF-8)</code></li>
                <li><code>Javascript - V8 3.12.3</code></li>
                <li><code>Lua - lua 5.0.3</code></li>
                <li><code>Node.js - nodejs 0.10.0</code></li>
                <li><code>Objective-C - gcc 4.7.2 (gcc `gnustep-config --objc-flags` -lobjc -lgnustep-base)</code></li>
                <li><code>Octave - GNU Octave 3.6.2 (octave -q -f --no-window-system)</code></li>
                <li><code>Pascal - fpc 2.6.0-6</code></li>
                <li><code>Perl - perl 5.14.2 (perl -w)</code></li>
                <li><code>Php - php 5.4.6</code></li>
                <li><code>Prolog - swi-prolog 5.10.4</code></li>
                <li><code>Python - python 2.7.3</code></li>
                <li><code>Python 3 - python 3.2.3</code></li>
                <li><code>Ruby - ruby 1.9.3 (ruby -w -W1)</code></li>
                <li><code>Scala - scala 2.9.2 (fsc -deprecation -unchecked -encoding UTF-8 &nbsp;|&nbsp; scala -Dfile.encoding=UTF-8)</code></li>
                <li><code>Scheme - guile 1.8.8</code></li>
            </ul> 
            Your code will be run on behalf user <code>'nobody'</code> and group <code>'nogroup'</code>. Also your code will be executed from Python wrapper which sets various limits to the process. It does so
            by using <code>'setrlimit'</code> system call. You'll have max 5 sec of cpu time, limited memory (~500 Mb) and other restrictions will apply (like no writing permissions). Also your process and all its children will be run in a
            newly created process group which will be terminated after 10 seconds from start if still running.<br/>
            <br/>We don't claim that this is secure. In many senses you'll have the power of <code>'nobody'</code> user. On a bright side, this has some <a href="http://rextester.com/runcode?code=KAKN22727">useful</a> side-effects. The reason why, at least for now, 
            we leave so many potential security breaches is because it's <b>hard</b> to make it really secure. What are the options? 
            <ul>
                <li><a href="http://codepad.org/">Codepad</a> seems to <a href="http://rextester.com/runcode?code=JAJ71205"><code>'ptrace'</code></a>
                    everything and disallow many system calls, for example, <code>'fork()'</code>. That's pretty restrictive.</li>
                <li>Apparmor - out of everything we tried, this did the job pretty well and it was rather easy to configure. But it doesn't work with OpenVZ virtualization that is used by our server provider.</li>
                <li>SELinux - stopped reading documentation in the middle of it. Too (unnecessarily?) complex.</li>
                <li>Native security mechanisms - like chroot and file permissions. Hard to make it secure this way without breaking the service and the system.</li>
            </ul>
            So, if you can take the system down - then be it, but please report how you did this. Your advice on this topic is always <a href="http://rextester.com/feedback/">welcome</a>.
            <br/><br/>
            <b style="color:Gray">Input support</b><br/>
            Everything that will be submitted as input will be piped to process via <code>stdin</code> stream. So your code should consume input as if it came from keyboard.
            <br/><br/>
            <b style="color:Gray">Live collaboration</b><br/>
            Write code so that others see this real-time. Every participant can make changes and see changes made by others. <a href="http://sharejs.org/">ShareJS</a> library is used for operational transformations on text.
            <br/><br/>
            <b style="color:Gray">Credit</b><br/>
            Special thanks goes to people behind <a href="http://codemirror.net/">CodeMirror</a>, <a href="http://www.cdolivet.com/editarea/">Edit area</a>, <a href="http://sharejs.org/">ShareJS</a>, <a href="http://code.google.com/p/coderev/">Coderev</a> and <a href="http://jedi.jedidjah.ch/">Jedi</a>.<br/><br />
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
        </div>  
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Keywords" content=".net regex tester, online c#, vb, php, java, python, c, c++ code compiler, online code execution, rundotnet, runcode, online compiler, run code online, snippet, run your code online, programming online, run code, run snippet, execute snippet, execute code, C#, C++, Java, Javascript, Python, Pascal, Ruby, Lua, Perl, Haskell, Assembly" />
    <meta name="Description" content="Rextester - test .net regexes, run code snippets in many languages from your browser." />    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
