<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.rundotnet.RundotnetData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%if (!string.IsNullOrEmpty(Model.Title))
    {
        %><%:Model.Title%>, <%:Model.LanguageChoice.ToLanguage()%> - rextester<%
    }
    else
    {
        if (Model.LanguageChoice == LanguagesEnum.Nasm)
                {
                    %>compile nasm online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CSharp)
                {
                     %>compile c# online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CPP)
                {
                    %>compile c++ gcc online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CPPClang)
                {
                    %>compile c++ clang online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VCPP)
                {
                    %>compile visual studio c++ online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.C)
                {
                    %>compile c gcc online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CClang)
                {
                    %>compile c clang online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VC)
                {
                    %>compile visual studio c online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Lisp)
                {
                    %>compile lisp online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.D)
                {
                    %>compile d online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.FSharp)
                {
                    %>compile f# online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Go)
                {
                    %>compile go online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Haskell)
                {
                    %>compile haskell online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Java)
                {
                    %>compile java online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Javascript)
                {
                    %>compile javascript online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Lua)
                {
                    %>compile lua online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Nodejs)
                {
                    %>compile nodejs online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Octave)
                {
                    %>compile octave online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.ObjectiveC)
                {
                    %>compile objective-c online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Pascal)
                {
                    %>compile pascal online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Perl)
                {
                    %>compile perl online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Php)
                {
                    %>compile php online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Prolog)
                {
                    %>compile prolog online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Python)
                {
                    %>compile python online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Python3)
                {
                    %>compile python3 online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.R)
                {
                    %>compile R online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Ruby)
                {
                    %>compile ruby online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Scala)
                {
                    %>compile scala online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Scheme)
                {
                    %>compile scheme online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.SqlServer)
                {
                    %>compile sql server online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Tcl)
                {
                    %>compile tcl online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VB)
                {
                    %>compile vb online <%
                }
                else
                {
                    %>compile c# online<%
                }                    
    }%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%if (!string.IsNullOrEmpty(Model.Title))
      {%>
            <h2 style="margin-top:0.5em; margin-bottom:0.5em; word-wrap: break-word;" title="<%:Model.Title%>"><%:Model.Title.Length > 70 ? Model.Title.Substring(0, 70) + "..." : Model.Title%></h2>
    <%}
      else
      {%>
            <h2>
                <%if (Model.LanguageChoice == LanguagesEnum.Nasm)
                {
                    %>compile nasm online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CSharp)
                {
                     %>compile c# online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CPP)
                {
                    %>compile c++ gcc online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CPPClang)
                {
                    %>compile c++ clang online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VCPP)
                {
                    %>compile visual studio c++ online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.C)
                {
                    %>compile c gcc online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CClang)
                {
                    %>compile c clang online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VC)
                {
                    %>compile visual studio c online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Lisp)
                {
                    %>compile lisp online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.D)
                {
                    %>compile d online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.FSharp)
                {
                    %>compile f# online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Go)
                {
                    %>compile go online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Haskell)
                {
                    %>compile haskell online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Java)
                {
                    %>compile java online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Javascript)
                {
                    %>compile javascript online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Lua)
                {
                    %>compile lua online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Nodejs)
                {
                    %>compile nodejs online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Octave)
                {
                    %>compile octave online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.ObjectiveC)
                {
                    %>compile objective-c online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Pascal)
                {
                    %>compile pascal online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Perl)
                {
                    %>compile perl online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Php)
                {
                    %>compile php online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Prolog)
                {
                    %>compile prolog online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Python)
                {
                    %>compile python online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Python3)
                {
                    %>compile python3 online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.R)
                {
                    %>compile R online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Ruby)
                {
                    %>compile ruby online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Scala)
                {
                    %>compile scala online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Scheme)
                {
                    %>compile scheme online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.SqlServer)
                {
                    %>compile sql server online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Tcl)
                {
                    %>compile tcl online <%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VB)
                {
                    %>compile vb online <%
                }
                else
                {
                    %>compile c# online<%
                }%>

            </h2>
      <%} %>
    <%using (Html.BeginForm("Index", "rundotnet", FormMethod.Post, new {id = "mainForm"}))
      {%>
        <div class="formcontent" style="padding-top: 0.5em; <%:Model.IsSaved ? "margin-bottom: 0.5em;" : "margin-bottom: 2em;"%>">
            <table style="width: 95%; margin:0;">
                <tr>
                    <td align="left">
                        <span style="margin: 0 0.5em 0 0">Language:</span><%:Html.DropDownListFor(f => f.LanguageChoiceWrapper, Model.Languages)%>
                        <span style="margin: 0 0.5em 0 0.5em">Editor:</span><%:Html.DropDownListFor(f => f.EditorChoiceWrapper, Model.Editor)%>
                    </td>
                </tr>
            </table>
            
            <%if (Model.IsLive)
              {%>
                <table style="width: 95%; margin-top:1em;table-layout:fixed;">
                    <tr>
                        <td rowspan="2" style="width:80%;">
                            <textarea class="editor" spellcheck="false" cols="1000" id="Program" name="Program" rows="30" style="width: 100%;resize:none;">Loading live document...</textarea>
                        </td>
                        <td id="chatarea" style="display:none;width:20%;" valign="top">
                            <textarea id="chatAreaText" readonly="readonly" spellcheck="false" style="width:94%;height:395px; border: solid 1px gray;resize:none;margin-left:1em;background-color:#FFFFBB;" cols="1000"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td id="chatbox" style="display:none;width:20%;" valign="bottom">
                            <textarea id="chatBoxText" idspellcheck="false" style="width:94%;height:80px; border: solid 1px gray; resize:none;margin-left:1em;background-color:#FFFFBB;" cols="1000"></textarea>
                        </td>
                    </tr>
                </table>
            <%}
              else
              { %>
                  <div style="width: 95%; margin-top:1em;">
                        <textarea class="editor" spellcheck="false" cols="1000" id="Program" name="Program" rows="30" style="width: 100%;resize:none;"><%=Model.Program%></textarea>
                  </div>
              <%} %>

            <%if (Model.ShowCompilerArgs) 
            {%>
                <table width="100%" id="CompilerArgsBox" style="display:none; width: 95%; margin-top:0.5em;margin-left:0;">
                    <tr>
                        <td>
                            <pre>&nbsp;<%:Model.Compiler%>&nbsp;</pre>
                        </td>
                        <td style="width:100%">
                            <input spellcheck="false" style="width:100%;border-style:solid;border-width:1px;border-color:#FF9900;" maxlength="5000" type="text" name="CompilerArgs" id="CompilerArgs" value="<%:Model.CompilerArgs %>"/>
                        </td>
                        <%if (!string.IsNullOrEmpty(Model.CompilerPostArgs)) {%>
                        <td>
                            <pre><%:Model.CompilerPostArgs %></pre>
                        </td>
                        <%} %>
                    </tr>
                </table>
            
            <%}%>
            <%if (Model.ShowInput)
              {%>
                <div style="width: 94.5%; margin-top:0.5em;margin-left:0;">                            
                    <textarea spellcheck="false" cols="1000" id="Input" name="Input" rows="5" style="background-color:#FFFFBB;border: solid 1px gray;width: 100%;<%:(string.IsNullOrEmpty(Model.Input)) ? "display:none;":"" %>"><%=Model.Input%></textarea>
                </div>
            <%} %>
            <table style="width: 95%; margin-top:0.5em;">
                <tr>
                    <td align="left">
                        <input id="Run" type="button" value="Run it<%:Model.EditorChoice == EditorsEnum.Codemirror ? " (F8)" : "" %><%:Model.EditorChoice == EditorsEnum.Simple ? " (Shift+Enter)" : "" %>" />
                        <%
                            string button_content = "";
                            if (Model.IsLive)
                                button_content = "Take snapshot";
                            else if (Model.IsInEditMode)
                                button_content = "Save edits";
                            else if (Model.IsSaved)
                                button_content = "Fork it";
                            else
                                button_content = "Save it";
                        %>
                        <input style="margin-left:1em" id="Save" type="button" value="<%:button_content%>" />    
                        <%if(!Model.IsInterpreted) 
                          {
                              %><%: Html.CheckBoxFor(model => model.ShowWarnings, new { style = "vertical-align: middle; margin-left: 1.5em;" })%>
                                <label for="ShowWarnings" style="margin-left: 0.2em;font-size: 0.85em;vertical-align: middle;">Show compiler warnings</label><%
                        }%>
                        <%if (Model.ShowCompilerArgs)
                        {%>
                        <span id="Args_label" style="margin-left: 0.5em;font-size: 0.85em; cursor:pointer;">[&nbsp;<span id="Expand_args_sign" style="font-size: 0.85em;">+</span>&nbsp;]&nbsp;Compiler args</span>
                        <%} %>
                        <%if (Model.ShowInput)
                        {%>
                        <span id="Input_label" style="margin-left: 0.5em;font-size: 0.85em; cursor:pointer;">[&nbsp;<span id="Expand_input_sign" style="font-size: 0.85em;"><%:(string.IsNullOrEmpty(Model.Input) || Model.IsLive) ? "+" : "-"%></span>&nbsp;]&nbsp;<span id="Expand_input_text">Show input</span></span>
                        <%} %>
                        <%if (Model.LanguageChoice == LanguagesEnum.SqlServer)
                        { %>
                        <a href="http://rextester.com:8080/Content/Schema.png" style="text-decoration: underline;color:Gray;margin-left:20px;">View schema</a>
                        <%} %>
                    </td>
                    <td align="right">
                        <%if (!Model.IsLive)
                        {%>
                            <%if (Model.EditorChoice == EditorsEnum.Codemirror)
                             {%>
                            <input style="margin-left:1em;" id="Live" type="button" value="Live cooperation" />   
                            <%} %>
                        <%}
                          else
                         {%>
                            <%if (Model.EditorChoice == EditorsEnum.Codemirror)
                             {%>
                            <span style="font-size: 0.85em;">Number of participants: <span id="UsersCount">-</span></span> 
                            <span  id="chat" style="font-size: 0.85em; margin-left:0.5em;cursor:pointer;">[&nbsp;<span id="chatsign" style="font-size: 0.85em;">+</span>&nbsp;]&nbsp;</span>
                            <%} %>
                         <%}%>
                        <input style="margin-left:1em;" id="Wall" type="button" value="Put on a wall" />
                        <%if (Model.EditorChoice == EditorsEnum.Codemirror && !Model.IsLive)
                        {%>
                            <input title="Fullscreen (F11), Esc to exit" style="margin-left:1em;" id="Full" type="button" value="F"/>
                        <%}%>
                        <input style="margin-left:1em;" id="Help" type="button" value="?"/>
                    </td>                  
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="margin-top: 0.8em;font-size: 0.85em;" id="Stats"><%:Model.RunStats%>&nbsp;</div>
                    </td>
                </tr>
            </table>

            <input id="Title" name="Title" type="hidden"/>
            <input id="SavedOutput" name="SavedOutput" type="hidden"/>
            <input id="WholeError" name="WholeError" type="hidden"/>
            <input id="WholeWarning" name="WholeWarning" type="hidden"/>
            <input id="StatsToSave" name="StatsToSave" type="hidden"/>
            <input id="CodeGuid" name="CodeGuid" type="hidden" value="<%:Model.CodeGuid%>"/>
            <input id="IsInEditMode" name="IsInEditMode" type="hidden" value="<%:Model.IsInEditMode%>"/>
            <input id="IsLive" name="IsLive" type="hidden" value="<%:Model.IsLive%>"/>
            <% if(Model.IsLive && Model.EditorChoice == EditorsEnum.Codemirror)
               {%>
                    <input id="InitialCode" name="InitialCode" type="hidden" value="<%:Model.Program%>"/>
              <%}
            %>
        </div>

        <%if(Model.IsSaved) 
        {%>
        <table style="width: 95%; margin-top:0;">
            <tr>
                <td align="right">
                    <%if (Model.PrimaryGuid != Model.CodeGuid)
                    {%>
                        <a class="smalllink" href="<%:Utils.BaseUrl+(Model.LivesVersion ? "live/" : "")+Model.PrimaryGuid%>">latest</a>&nbsp;<span class="smalllink">|</span>&nbsp;
                    <%} %>
                    <%if(Model.EditVisible)
                    {%>
                    <a class="smalllink" href="<%:Utils.BaseUrl+"edit"+"/"+Model.CodeGuid%>">edit mode</a>&nbsp;<span class="smalllink">|</span>&nbsp;
                    <%} %>
                    <%if(Model.BackToForkVisible)
                    {%>
                    <a class="smalllink" href="<%:Utils.BaseUrl+Model.CodeGuid %>">fork mode</a>&nbsp;<span class="smalllink">|</span>&nbsp;
                    <%} %>
                    <a class="smalllink" href="<%:Utils.BaseUrl+"history"+"/"+ Model.PrimaryGuid %>">history</a>
                    <%if(Model.IsOnAWall)
                    {%>
                    &nbsp;<span class="smalllink">|</span>&nbsp;<a class="smalllink" href="<%:Utils.BaseUrl+"discussion"+"/"+Model.CodeGuid+"/"+Model.Title.StringToUrl()%>">discussion</a>
                    <%} %>
                </td>
            </tr>
        </table>
        <%} %>

    <%} %>
    <pre id="Link" class="resultarea"><%:Model.IsLive && string.IsNullOrEmpty(Model.WholeError)? "This is (permanent) live collaboration session. You will see changes that others make as well as be able to make your own." : ""%></pre>

    <div id="Warning"><%
        if (Model.ShowWarnings && !string.IsNullOrEmpty(Model.WholeWarning))
        { 
            %><pre style="color: Orange" class="resultarea">Warning(s):</pre><pre id="WarningSpan" class="resultarea"><%:Model.WholeWarning%></pre><%   
        }
    %></div>

    <div id="Error"><%
        if (!string.IsNullOrEmpty(Model.WholeError))
        {
            %><%if(Model.LanguageChoice != LanguagesEnum.Prolog) {%><pre style="color: Red" class="resultarea">Error(s)<%:Model.IsInterpreted ? ", warning(s)" : ""%>:</pre><%}%><pre id="ErrorSpan" class="resultarea"><%:Model.WholeError%></pre><%  
        }                   
    %></div> 
    <pre id="Result" class="resultarea"><%
        if (Model.IsOutputInHtml)
        {
            %><%=Model.Output%><%
        }
        else
        {
            %><%:Model.Output%><%
        }
    %></pre>
    <pre id="Files"></pre>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <%if (Model.LanguageChoice == LanguagesEnum.Nasm)
                {
                    %><meta name="Keywords" content="compile nasm online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CSharp)
                {
                     %><meta name="Keywords" content="compile c# online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CPP)
                {
                    %><meta name="Keywords" content="compile c++ gcc online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CPPClang)
                {
                    %><meta name="Keywords" content="compile c++ clang online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VCPP)
                {
                    %><meta name="Keywords" content="compile visual studio c++ online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.C)
                {
                    %><meta name="Keywords" content="compile c gcc online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CClang)
                {
                    %><meta name="Keywords" content="compile c clang online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VC)
                {
                    %><meta name="Keywords" content="compile c visual studio online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Lisp)
                {
                    %><meta name="Keywords" content="compile lisp online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.D)
                {
                    %><meta name="Keywords" content="compile d online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.FSharp)
                {
                    %><meta name="Keywords" content="compile f# online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Go)
                {
                    %><meta name="Keywords" content="compile go online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Haskell)
                {
                    %><meta name="Keywords" content="compile haskell online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Java)
                {
                    %><meta name="Keywords" content="compile java online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Javascript)
                {
                    %><meta name="Keywords" content="compile javascript online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Lua)
                {
                    %><meta name="Keywords" content="compile lua online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Nodejs)
                {
                    %><meta name="Keywords" content="compile nodejs online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Octave)
                {
                    %><meta name="Keywords" content="compile octave online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.ObjectiveC)
                {
                    %><meta name="Keywords" content="compile objective-c online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Pascal)
                {
                    %><meta name="Keywords" content="compile pascal online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Perl)
                {
                    %><meta name="Keywords" content="compile perl online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Php)
                {
                    %><meta name="Keywords" content="compile php online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Prolog)
                {
                    %><meta name="Keywords" content="compile prolog online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Python)
                {
                    %><meta name="Keywords" content="compile python online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Python3)
                {
                    %><meta name="Keywords" content="compile python3 online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.R)
                {
                    %><meta name="Keywords" content="compile R online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Ruby)
                {
                    %><meta name="Keywords" content="compile ruby online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Scala)
                {
                    %><meta name="Keywords" content="compile scala online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Scheme)
                {
                    %><meta name="Keywords" content="compile scheme online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.SqlServer)
                {
                    %><meta name="Keywords" content="compile sql server online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Tcl)
                {
                    %><meta name="Keywords" content="compile tcl online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VB)
                {
                    %><meta name="Keywords" content="compile vb online" /><%
                }
                else
                {
                    %><meta name="Keywords" content="compile c# online" /><%
                }%>
    <%if (!string.IsNullOrEmpty(Model.Title))
    {
        %><meta name="Description" content="<%:Model.Title%> in <%:Model.LanguageChoice.ToLanguage()%>" /><%
    }
    else
    {
        if (Model.LanguageChoice == LanguagesEnum.Nasm)
                {
                    %><meta name="Description" content="compile nasm online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CSharp)
                {
                     %><meta name="Description" content="compile c# online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CPP)
                {
                    %><meta name="Description" content="compile c++ gcc online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CPPClang)
                {
                    %><meta name="Description" content="compile c++ clang online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VCPP)
                {
                    %><meta name="Description" content="compile visual studio c++ online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.C)
                {
                    %><meta name="Description" content="compile c gcc online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.CClang)
                {
                    %><meta name="Description" content="compile c clang online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VC)
                {
                    %><meta name="Description" content="compile c visual studio online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Lisp)
                {
                    %><meta name="Description" content="compile lisp online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.D)
                {
                    %><meta name="Description" content="compile d online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.FSharp)
                {
                    %><meta name="Description" content="compile f# online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Go)
                {
                    %><meta name="Description" content="compile go online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Haskell)
                {
                    %><meta name="Description" content="compile haskell online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Java)
                {
                    %><meta name="Description" content="compile java online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Javascript)
                {
                    %><meta name="Description" content="compile javascript online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Lua)
                {
                    %><meta name="Description" content="compile lua online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Nodejs)
                {
                    %><meta name="Description" content="compile nodejs online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Octave)
                {
                    %><meta name="Description" content="compile octave online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.ObjectiveC)
                {
                    %><meta name="Description" content="compile objective-c online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Pascal)
                {
                    %><meta name="Description" content="compile pascal online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Perl)
                {
                    %><meta name="Description" content="compile perl online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Php)
                {
                    %><meta name="Description" content="compile php online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Prolog)
                {
                    %><meta name="Description" content="compile prolog online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Python)
                {
                    %><meta name="Description" content="compile python online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Python3)
                {
                    %><meta name="Description" content="compile python3 online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.R)
                {
                    %><meta name="Description" content="compile R online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Ruby)
                {
                    %><meta name="Description" content="compile ruby online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Scala)
                {
                    %><meta name="Description" content="compile scala online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Scheme)
                {
                    %><meta name="Description" content="compile scheme online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.SqlServer)
                {
                    %><meta name="Description" content="compile sql server online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.Tcl)
                {
                    %><meta name="Description" content="compile tcl online" /><%
                }
                else if (Model.LanguageChoice == LanguagesEnum.VB)
                {
                    %><meta name="Description" content="compile vb online" /><%
                }
                else
                {
                    %><meta name="Description" content="compile c# online" /><%
                }
    }%>
    <%if (Model.EditorChoice == EditorsEnum.Codemirror)
     {
             %> <link rel="stylesheet" href="http://rextester.com:8080/Scripts/codemirror3/lib/codemirror.css"/>
                <link rel="stylesheet" href="http://rextester.com:8080/Scripts/codemirror3/addon/display/fullscreen.css"/>
                <link rel="stylesheet" href="http://rextester.com:8080/Scripts/codemirror3/addon/dialog/dialog.css"/><%
            if (Model.LanguageChoice == LanguagesEnum.CSharp || Model.LanguageChoice == LanguagesEnum.FSharp || Model.LanguageChoice == LanguagesEnum.VB)
            { 
                %><link rel="stylesheet" href="http://rextester.com:8080/Scripts/codemirror3/theme/csharp.css"/><%
            }
            else if (Model.LanguageChoice == LanguagesEnum.Java || Model.LanguageChoice == LanguagesEnum.Scala)
            { 
                %><link rel="stylesheet" href="http://rextester.com:8080/Scripts/codemirror3/theme/java.css"/><%
            }
            if (Model.IsIntellisense)
            {            
                %><link rel="stylesheet" href="http://rextester.com:8080/Scripts/codemirror3/addon/hint/show-hint.css"><%
            }
    }%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <%if (Model.EditorChoice == EditorsEnum.Codemirror)
      {
            %><script src="http://rextester.com:8080/Scripts/codemirror3/lib/codemirror.js" type="text/javascript"></script>
              <script src="http://rextester.com:8080/Scripts/codemirror3/addon/edit/matchbrackets.js" type="text/javascript"></script>
              <script src="http://rextester.com:8080/Scripts/codemirror3/addon/display/fullscreen.js" type="text/javascript"></script>
              <script src="http://rextester.com:8080/Scripts/codemirror3/addon/dialog/dialog.js" type="text/javascript"></script>
              <script src="http://rextester.com:8080/Scripts/codemirror3/addon/search/searchcursor.js" type="text/javascript"></script>
              <script src="http://rextester.com:8080/Scripts/codemirror3/addon/search/search.js" type="text/javascript"></script><%
    }%>
    <%if (Model.IsIntellisense)
    {
            %><script src="http://rextester.com:8080/Scripts/codemirror3/addon/hint/show-hint.js" type="text/javascript"></script><%
    }%>
    <%if (Model.IsIntellisense)
    {
            %><script src="http://rextester.com:8080/Scripts/codemirror3/addon/hint/csharp-hint.js" type="text/javascript"></script><%
    }%>
    <%if (Model.EditorChoice == EditorsEnum.Editarea)
    {
        %><script src="http://rextester.com:8080/Scripts/editarea/edit_area_full.js" type="text/javascript"></script><%
    }%>
    <%if (Model.EditorChoice == EditorsEnum.Simple)
    {
        %><script type="text/javascript">

              $(document).ready(function () {

                  $(document).delegate('#Program', 'keydown', function(e) {
                      var keyCode = e.keyCode || e.which;

                      if (keyCode == 9) {
                          e.preventDefault();
                          var start = $(this).get(0).selectionStart;
                          var end = $(this).get(0).selectionEnd;

                          // set textarea value to: text before caret + tab + text after caret
                          $(this).val($(this).val().substring(0, start)
                                      + "    "
                                      + $(this).val().substring(end));

                          // put caret at right position again
                          $(this).get(0).selectionStart =
                          $(this).get(0).selectionEnd = start + 4;
                      }
                  });

                  $("#Program").keydown(function(e){
                      // Enter was pressed without shift key
                      if (e.keyCode == 13 && e.shiftKey)
                      {
                          // prevent default behavior
                          e.preventDefault();
                          Run();
                      }
                  });

              });
          </script><%
    }%>
    <%if (Model.IsLive)
    { %>
        <script src="https://cdn.firebase.com/v0/firebase.js"></script>
        <link rel="stylesheet" href="http://rextester.com:8080/Scripts/firepad/firepad.css" />
        <script src="http://rextester.com:8080/Scripts/firepad/firepad-min.js"></script>
    <%} %>
    <script type="text/javascript">
        //<![CDATA[
        <%if(Model.EditorChoice ==  EditorsEnum.Codemirror)
        {
            %>var GlobalEditor;<%
        }%>

        $(document).ready(function () {
                    
            <%if(Model.ShowInput) 
            {%>
            $("#Input_label").click(function() {
                
                $("#Input").toggle();
                if($("#Expand_input_sign").text() == "-") {
                    $("#Expand_input_sign").text("+");
                    $("#Expand_input_text").text("Show input");
                }
                else {
                    $("#Expand_input_sign").text("-");
                    $("#Expand_input_text").text("Hide input");
                }
            });

            $("#Input").bind('keydown', function (e) {
                var code = e.keyCode || e.which;
                if (code == 119 || code == 116) //F8 or F5
                {
                    e.preventDefault();
                    Run();
                }
            });
            <%} %>

            <%if(Model.ShowCompilerArgs) 
            {%>
            $("#Args_label").click(function() {                
                $("#CompilerArgsBox").toggle();
                if($("#Expand_args_sign").text() == "-") {
                    $("#Expand_args_sign").text("+");
                }
                else {
                    $("#Expand_args_sign").text("-");
                }
            });
            $("#CompilerArgs").bind('keydown', function (e) {
                var code = e.keyCode || e.which;
                if (code == 119 || code == 116) //F8 or F5
                {
                    e.preventDefault();
                    Run();
                }
            });
            <%} %>

            <%if(Model.IsLive)
            { %>
            $("#chat").click(function() {       
                $("#chatarea").toggle();
                $("#chatbox").toggle();
                        
                if($("#chatsign").text() == "-") {
                    $("#chatsign").text("+");
                }
                else {
                    ScrollToBottom($("#chatAreaText"));
                    $("#chatsign").text("-");
                    $("#chatsign").css('color', 'black');
                }
            });
            function ScrollToBottom(textArea) {
                textArea.scrollTop(
                    textArea[0].scrollHeight - textArea.height()
                );
            }

                <%if (Model.User_Id != null)
                {%>
            setInterval(function() {
                $.post('/rundotnet/updateliveindex', { code: GlobalEditor.getValue(), chat: $("#chatAreaText").val(), guid: '<%:Model.CodeGuid%>'}, null, 'text'); 
            }, 60000);
            <%}%>

            <%} %>

            $("#Help").click(function () {
                window.open("<%:Utils.GetUrl(Utils.PagesEnum.Faq)%>",'_blank');
            });
            <%if (Model.EditorChoice == EditorsEnum.Codemirror && !Model.IsLive)
            {%>
            $("#Full").click(function () {
                if(!GlobalEditor.hasFocus())
                {
                    GlobalEditor.focus()
                }
                GlobalEditor.setOption("fullScreen", !GlobalEditor.getOption("fullScreen"));
            });
            <%}%>

            $("#Save").click(function () {
                <%if(Model.IsOutputInHtml) 
                {%>
                $("#SavedOutput").val($("#Result").html());
                <%} else
                { %>
                $("#SavedOutput").val($("#Result").text());
                <%} %>
                $("#WholeError").val($("#ErrorSpan").text());
                $("#WholeWarning").val($("#WarningSpan").text());
                $("#StatsToSave").val($("#Stats").text());
                Save(1);
            });
            $("#Wall").click(function () {
                $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Title:<br/><br/>&nbsp;&nbsp;&nbsp;<input style=\"border-style:solid;border-width:1px;border-color:#FF9900;\" size=\"100\" type=\"text\" id=\"titleInput\"/>&nbsp;&nbsp;<br/><br/>Choose the wall:<br/>&nbsp;&nbsp;<input type=\"radio\" name=\"wall_group\" value=\"1\" checked>&nbsp;My wall<br/>&nbsp;&nbsp;<input type=\"radio\" name=\"wall_group\" value=\"2\">&nbsp;Code wall<br/><br/><input type=\"button\" id=\"OKButton\" value=\"Ok\"/>&nbsp;&nbsp;&nbsp;<span style=\"color:red\" id=\"titleError\"></span><br/><br/><br/></pre>");
                $("#titleInput").focus();
                $('html, body').animate({ scrollTop: $("#Run").offset().top }, 200);
                $("#OKButton").click(function() {
                    if($("#titleInput").val().trim() == "")
                    {
                        $("#titleError").text("Title shouldn't be empty.");
                        return;
                    }
                    if($("#titleInput").val().length > 500)
                    {
                        $("#titleError").text("Title shouldn't be longer than 500 characters.");
                        return;
                    }
                    $("#Title").val($("#titleInput").val());
                    <%if(Model.IsOutputInHtml) 
                    {%>
                    $("#SavedOutput").val($("#Result").html());
                    <%} else
                    { %>
                    $("#SavedOutput").val($("#Result").text());
                    <%} %>
                    $("#WholeError").val($("#ErrorSpan").text());
                    $("#WholeWarning").val($("#WarningSpan").text());
                    $("#StatsToSave").val($("#Stats").text());
                    if($('input[type=radio]:checked').val() == '1')
                        Save(4);
                    else
                        Save(2);
                });
            });
            <%if(!Model.IsLive) 
            {%>
            $("#Live").click(function () {
                    <%if(Model.IsOutputInHtml) 
                    {%>
                $("#SavedOutput").val($("#Result").html());
                    <%} else
                    { %>
                $("#SavedOutput").val($("#Result").text());
                    <%} %>
                $("#WholeError").val($("#ErrorSpan").text());
                $("#WholeWarning").val($("#WarningSpan").text());
                $("#StatsToSave").val($("#Stats").text());
                Save(3);
            });
            <%} %>
            $("#Run").click(function () {
                Run();
            });
            $("#LanguageChoiceWrapper").change(function () {
                Reload();
            });
            $("#EditorChoiceWrapper").change(function () {
                Reload();
            });            
            var Reload = function () {
                var act = "";

                if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Nasm%>)
                {
                    act = "/l/nasm_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.CSharp%>)
                {
                    act = "/l/csharp_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.CPP%>)
                {
                    act = "/l/cpp_online_compiler_gcc";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.CPPClang%>)
                {
                    act = "/l/cpp_online_compiler_clang";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.VCPP%>)
                {
                    act = "/l/cpp_online_compiler_visual";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.C%>)
                {
                    act = "/l/c_online_compiler_gcc";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.CClang%>)
                {
                    act = "/l/c_online_compiler_clang";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.VC%>)
                {
                    act = "/l/c_online_compiler_visual";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Lisp%>)
                {
                    act = "/l/common_lisp_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.D%>)
                {
                    act = "/l/d_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.FSharp%>)
                {
                    act = "/l/fsharp_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Go%>)
                {
                    act = "/l/go_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Haskell%>)
                {
                    act = "/l/haskell_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Java%>)
                {
                    act = "/l/java_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Javascript%>)
                {
                    act = "/l/js_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Lua%>)
                {
                    act = "/l/lua_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Nodejs%>)
                {
                    act = "/l/nodejs_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Octave%>)
                {
                    act = "/l/octave_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.ObjectiveC%>)
                {
                    act = "/l/objectivec_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Pascal%>)
                {
                    act = "/l/pascal_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Perl%>)
                {
                    act = "/l/perl_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Php%>)
                {
                    act = "/l/php_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Prolog%>)
                {
                    act = "/l/prolog_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Python%>)
                {
                    act = "/l/python_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Python3%>)
                {
                    act = "/l/python3_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.R%>)
                {
                    act = "/l/r_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Ruby%>)
                {
                    act = "/l/ruby_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Scala%>)
                {
                    act = "/l/scala_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Scheme%>)
                {
                    act = "/l/scheme_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.SqlServer%>)
                {
                    act = "/l/sql_server_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.Tcl%>)
                {
                    act = "/l/tcl_online_compiler";
                }
                else if ($("#LanguageChoiceWrapper").val() == <%=(int)LanguagesEnum.VB%>)
                {
                    act = "/l/visual_basic_online_compiler";
                }
                else
                {
                    act = "/l/csharp_online_compiler";
                }
                
                $('#mainForm').attr('action', act);

                $("#Input").val('');
                $("#SavedOutput").val('');
                $("#WholeError").val('');
                $("#WholeWarning").val('');
                $("#StatsToSave").val('');
                $("#mainForm").submit();
            };

            $.ajaxSetup({
                timeout: 60000,
                error: function (request, status, err) {
                    $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Error occurred ("+err+"). Try again later.</pre>");
                }
            });
        });

        function Save (what) {
            $('html, body').animate({ scrollTop: $("#Link").offset().top }, 500);

            $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Saving...</pre>");
            $("#Warning").replaceWith("<div id=\"Warning\"></div>");
            $("#Error").replaceWith("<div id=\"Error\"></div>");
            $("#Result").replaceWith("<pre id=\"Result\" class=\"resultarea\"></pre>");
            $("#Files").replaceWith("<pre id=\"Files\"></pre>");

            <%if(Model.EditorChoice == EditorsEnum.Codemirror) 
            {
                %>$("#Program").val(GlobalEditor.getValue());<%
            }
            else if(Model.EditorChoice == EditorsEnum.Editarea)
            {
                %>$("#Program").val(editAreaLoader.getValue("Program"));<%
            }%>

            var serializedData = $("#mainForm").serialize();
            if(what == 1)
            {
                $.post('/rundotnet/save', serializedData,
                        function (data) {
                            var obj = jQuery.parseJSON(data);
                            if(obj.Errors != null)
                            {
                                $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\"></pre>");
                                $("#Error").replaceWith("<div id=\"Error\"><pre style=\"color: Red\" class=\"resultarea\">Error:</pre><pre id=\"ErrorSpan\" class=\"resultarea\"></pre></div>");
                                $("#ErrorSpan").text(obj.Errors.replace(/\r/g, ""));
                                return;
                            }                    
                            if(obj.Updated == true)
                                <%if(Model.IsLive) 
                                {%>
                                $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Snapshot taken</pre>");
                                <%}
                                else
                                {%>
                            $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Version created, updates saved</pre>");
                                <%} %>
                        else        
                                $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Permanent link: <a href=\""+obj.Url+"\">"+obj.Url+"</a></pre>");
                        }, 'text');
            }
            else if(what == 2)
            {
                $.post('/rundotnet/saveonwall', serializedData,
                    function (data) {
                        var obj = jQuery.parseJSON(data);
                        if(obj.Errors != null)
                        {
                            $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\"></pre>");
                            $("#Error").replaceWith("<div id=\"Error\"><pre style=\"color: Red\" class=\"resultarea\">Error:</pre><pre id=\"ErrorSpan\" class=\"resultarea\"></pre></div>");
                            $("#ErrorSpan").text(obj.Errors.replace(/\r/g, ""));
                            return;
                        }                        
                        $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">This snippet is on a wall, permanent link: <a href=\""+obj.Url+"\">"+obj.Url+"</a></pre>");
                    }, 'text');       
            }
            else if(what == 4)
            {
                $.post('/rundotnet/saveonpersonalwall', serializedData,
                    function (data) {
                        var obj = jQuery.parseJSON(data);
                        if(obj.NotLoggedIn == true)
                        {
                            $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Please <a href=\"login\">log in</a> first.</pre>");
                            return;
                        }
                        else if(obj.Errors != null)
                        {
                            $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\"></pre>");
                            $("#Error").replaceWith("<div id=\"Error\"><pre style=\"color: Red\" class=\"resultarea\">Error:</pre><pre id=\"ErrorSpan\" class=\"resultarea\"></pre></div>");
                            $("#ErrorSpan").text(obj.Errors.replace(/\r/g, ""));
                            return;
                        }                        
                        $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">This snippet is on a wall, permanent link: <a href=\""+obj.Url+"\">"+obj.Url+"</a></pre>");
                    }, 'text');       
            }
            <%if(!Model.IsLive) 
            {%>
            else if(what == 3)
            {
                $.post('/rundotnet/savelive', serializedData,
                    function (data) {
                        var obj = jQuery.parseJSON(data);
                        if(obj.Errors != null)
                        {
                            $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\"></pre>");
                            $("#Error").replaceWith("<div id=\"Error\"><pre style=\"color: Red\" class=\"resultarea\">Error:</pre><pre id=\"ErrorSpan\" class=\"resultarea\"></pre></div>");
                            $("#ErrorSpan").text(obj.Errors.replace(/\r/g, ""));
                            return;
                        }                        
                        $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Permanent live session created at <a href=\""+obj.Url+"\">"+obj.Url+"</a>\nAnyone who visits this link can edit code and see changes that others make in real-time.</pre>");
                    }, 'text');       
            }
            <%} %>
        };

        function Run () {
            $('html, body').animate({ scrollTop: $("#Link").offset().top }, 500);

            $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Working...</pre>");
            $("#Warning").replaceWith("<div id=\"Warning\"></div>");
            $("#Error").replaceWith("<div id=\"Error\"></div>");
            $("#Result").replaceWith("<pre id=\"Result\" class=\"resultarea\"></pre>");
            $("#Files").replaceWith("<pre id=\"Files\"></pre>");

            <%if(Model.EditorChoice == EditorsEnum.Codemirror) 
            {
                %>$("#Program").val(GlobalEditor.getValue());<%
            }
            else if(Model.EditorChoice == EditorsEnum.Editarea)
            {
                %>$("#Program").val(editAreaLoader.getValue("Program"));<%
            }%>
            
            var serializedData = $("#mainForm").serialize();
            $.post('/rundotnet/Run', serializedData,
                    function (data) {
                        $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\"></pre>");                        
                        var obj = jQuery.parseJSON(data);
                        if(obj.Warnings != null)
                        {
                            $("#Warning").replaceWith("<div id=\"Warning\"><pre style=\"color: Orange\" class=\"resultarea\">Warning(s):</pre><pre id=\"WarningSpan\" class=\"resultarea\"></pre></div>");
                            $("#WarningSpan").text(obj.Warnings.replace(/\r/g, ""));
                        }
                        if(obj.Errors != null)
                        {
                            $("#Error").replaceWith("<div id=\"Error\"><%if(Model.LanguageChoice != LanguagesEnum.Prolog) { %><pre style=\"color: Red\" class=\"resultarea\">Error(s)"+'<%:Model.IsInterpreted ? ", warning(s)" : ""%>'+":</pre><%}%><pre id=\"ErrorSpan\" class=\"resultarea\"></pre></div>");
                            $("#ErrorSpan").text(obj.Errors.replace(/\r/g, ""));
                        }

                        <%if(Model.IsOutputInHtml) 
                        {%>
                        if(obj.Result != null)
                            $("#Result").html(obj.Result);
                        <%}
                        else
                        { %>
                        if(obj.Result != null)
                            $("#Result").text(obj.Result.replace(/\r/g, ""));
                        <%} %>
                        if(obj.Files != null)
                        {
                            for (var key in obj.Files) 
                            {             
                                var img_div = $(document.createElement('div'));
                                var img = $(document.createElement('img'));
                                img.attr('src', "data:image/png;base64," + obj.Files[key]).height(600).width(700);
                                img.appendTo(img_div);
                                img_div.appendTo($('#Files'));
                            }
                        }
                        $("#Stats").text(obj.Stats);

                        $('html, body').animate({ scrollTop: $("#Run").offset().top }, 500);
                    }, 'text');
        };

        <% if(Model.IsIntellisense)
        {%>
        $('body').keypress(function(e){

            if(String.fromCharCode( e.which ) == ':') {
                var cur = GlobalEditor.getCursor();
                var ln = GlobalEditor.getValue().split("\n")[cur.line];
                if(cur.ch > 0 && ln[cur.ch-1] == ':' ) {
                    setTimeout(function() {
                        CodeMirror.showHint(GlobalEditor, CodeMirror.hint.csharp, {async: true});
                    }, 100)
                }
            }
            if(String.fromCharCode( e.which ) == '>') {
                var cur = GlobalEditor.getCursor();
                var ln = GlobalEditor.getValue().split("\n")[cur.line];
                if(cur.ch > 0 && ln[cur.ch-1] == '-' ) {
                    setTimeout(function() {
                        CodeMirror.showHint(GlobalEditor, CodeMirror.hint.csharp, {async: true});
                    }, 100)
                }
            }

            if(String.fromCharCode( e.which ) == '.') {
                setTimeout(function() {
                    CodeMirror.showHint(GlobalEditor, CodeMirror.hint.csharp, {async: true});
                }, 100)
            }
        });
         <%} %>
        

        function keyEvent(cm, e) {
            // Hook into F8 (and F5)
            if ((e.keyCode == 116 || e.keyCode == 119) && e.type == 'keydown') {
                e.stop();                
                Run();
            }
        }
        //]]>
    </script>
  
    <% Html.RenderPartial("EditorControl", Model); %>

</asp:Content>
