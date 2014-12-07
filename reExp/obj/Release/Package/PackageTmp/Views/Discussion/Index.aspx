<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.discussion.DiscussionData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:Model.Title%>, <%:Model.Language.ToLanguage()%> - rextester
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%:Model.Title%></h2>
    <textarea id="code" style="display:none;"><%:Model.Code%></textarea>
    <table style="width:100%;">
        <tr>
            <td colspan="2">
                <pre id="output" class="cm-s-default"></pre>
            </td>
        </tr>
        <%if(Model.ShowComments)
        { %>
        <tr>
            <td align="left">
                &nbsp;<a class="smalllink" href="<%:Utils.BaseUrl+Model.Guid%>">run</a>
                &nbsp;<span class="smalllink">|</span>&nbsp;<a class="smalllink" href="<%:Utils.BaseUrl+"edit"+"/"+Model.Guid%>">edit</a>
                &nbsp;<span class="smalllink">|</span>&nbsp;<a class="smalllink" href="<%:Utils.BaseUrl+"history"+"/"+Model.Guid%>">history</a>
                &nbsp;<span class="smalllink">|</span>&nbsp;<a class="smalllink" href="<%:Utils.BaseUrl+"main/faq"%>">help</a>
            </td>
            <td align="right">
                <span id="up"> 
                        <%if (Model.VoteUp == null || !(bool)Model.VoteUp)
                            {%>
                        <img id="upVote" style="cursor:pointer" src="../../Content/up.png" title="Vote for it." alt=""/>
                        <img id="upVoted" style="cursor:pointer; display:none;" src="../../Content/upvoted.png" title="You voted for it. Click to cancel." alt=""/>  
                        <%}
                            else
                            { %>
                        <img id="upVote" style="cursor:pointer; display:none;" src="../../Content/up.png" title="Vote for it." alt=""/>
                        <img id="upVoted" style="cursor:pointer" src="../../Content/upvoted.png" title="You voted for it. Click to cancel." alt=""/>     
                            <%} %>
                    </span>
                    <span id="votes_count" style="margin: 0 0.5em 0 0.5em; font-family: Sans-Serif; font-size: 120%; color:#555555;">
                        <%:Model.Votes%>
                    </span>
                    <span id="down">
                        <%if (Model.VoteUp == null || (bool)Model.VoteUp)
                            {%>
                        <img id="downVote" style="cursor:pointer" src="../../Content/down.png" title="Vote against it." alt=""/>
                        <img id="downVoted" style="cursor:pointer; display:none;" src="../../Content/downvoted.png" title="You voted against it. Click to cancel." alt=""/>   
                        <%}
                            else
                            { %>
                        <img id="downVote" style="cursor:pointer; display:none;" src="../../Content/down.png" title="Vote against it." alt=""/>
                        <img id="downVoted" style="cursor:pointer" src="../../Content/downvoted.png" title="You voted against it. Click to cancel." alt=""/>   
                            <%} %>
                    </span>
                </td>
        </tr>
        <%} %>
    </table>

    <%if(Model.ShowComments)
    {%>
        <div id="comments_thread" style="margin: 3em auto;max-width: 70em;">
            <table style="width:100%">
                <tr>
                    <%--<td style="vertical-align:top; width: 62%">
                         <%foreach(var com in Model.Comments)
                        {%>             
                            <span>
                                <% Html.RenderPartial("DiscussionControl", com); %> 
                            </span>
                        <%}%> 
                        <br/><br/>

                        <%if(SessionManager.IsUserInSession())
                          {
                            using (Html.BeginForm())
                            {%>
                                <div class="mdd_toolbar"></div>
                                <textarea cols="50" rows="12" class="mdd_editor" id="NewComment" name="NewComment"><%=Model.NewComment%></textarea>
                                <div class="mdd_resizer"></div>
                                <div class="mdd_preview" style="display:block;word-wrap:break-word;"></div>
                                <input id="Button" type="submit" value="Submit"/>
                            <%}
                          }
                          else
                          {%>
                                Please <a href="<%=Utils.BaseUrl+"login"%>">log in</a> to post a comment.
                        <%}%>
                    </td>--%>
                    <td style="vertical-align:top; width: 62%">
                        <div id="disqus_thread" style="margin: 3em auto;max-width: 64.3em;"></div>
                        <script type="text/javascript">
                            /* * * CONFIGURATION VARIABLES: EDIT BEFORE PASTING INTO YOUR WEBPAGE * * */
                            var disqus_shortname = 'rextester'; // required: replace example with your forum shortname
                            /* * * DON'T EDIT BELOW THIS LINE * * */
                            (function () {
                                var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true;
                                dsq.src = 'http://' + disqus_shortname + '.disqus.com/embed.js';
                                (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);
                            })();
                               </script>
                        <noscript>Please enable JavaScript to view the <a href="http://disqus.com/?ref_noscript">comments powered by Disqus.</a></noscript>
                    </td>
                    <td style="padding-left: 80px; vertical-align:top;width: 18%;">
                        <% Html.RenderPartial("RelatedControl", Model.Related); %> 
                    </td>
                </tr>          
            </table>
        </div>
    <%}%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <link rel="stylesheet" href="../../Scripts/codemirror3/lib/codemirror.css" />
    <link rel="stylesheet" href="../../Scripts/codemirror3/doc/docs2.css" />
    <link rel="stylesheet" href="../../Scripts/mdd_styles.css" /> 


    <%--<%if (Model.Language == LanguagesEnum.CSharp)
    { 
        %><link rel="stylesheet" href="../../Scripts/codemirror2/theme/csharp.css"/><%
    }
    else if (Model.Language == LanguagesEnum.Java)
    { 
        %><link rel="stylesheet" href="../../Scripts/codemirror2/theme/java.css"/><%
    }%>--%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="../../Scripts/codemirror3/lib/codemirror.js"></script>
    <script src="../../Scripts/codemirror3/addon/runmode/runmode.js"></script>
    <%--<script src="../../Scripts/MarkdownDeepLib.min.js"></script>--%>

    <%  
  string mode = null;
  List<string> js = new List<string>();
  switch (Model.Language)
  {
      case LanguagesEnum.CSharp:
          mode = "text/x-csharp";
          js.Add("mode/clike/clike.js");
          break;
      case LanguagesEnum.VB:
          mode = "text/x-vb";
          js.Add("mode/vb/vb.js");        
          break;
      case LanguagesEnum.FSharp:
          mode = "text/x-fsharp";
          js.Add("mode/clike/clike.js");
          break;
      case LanguagesEnum.Java:
          mode = "text/x-java";
          js.Add("mode/clike/clike.js");
          break;
      case LanguagesEnum.Javascript:
          mode = "text/javascript";
          js.Add("mode/javascript/javascript.js");
          break;
      case LanguagesEnum.Python:
          mode = "python";
          js.Add("mode/python/python.js");
          break;
      case LanguagesEnum.Python3:
          mode = "python";
          js.Add("mode/python/python.js");
          break;
      case LanguagesEnum.C:
          mode = "text/x-csrc";
          js.Add("mode/clike/clike.js");
          break;
      case LanguagesEnum.VC:
          mode = "text/x-csrc";
          js.Add("mode/clike/clike.js");
          break;
      case LanguagesEnum.CPP:
          mode = "text/x-c++src";
          js.Add("mode/clike/clike.js");
          break;
      case LanguagesEnum.VCPP:
          mode = "text/x-c++src";
          js.Add("mode/clike/clike.js");
          break;
      case LanguagesEnum.CClang:
          mode = "text/x-csrc";
          js.Add("mode/clike/clike.js");
          break;
      case LanguagesEnum.CPPClang:
          mode = "text/x-c++src";
          js.Add("mode/clike/clike.js");
          break;
      case LanguagesEnum.Php:
          mode = "application/x-httpd-php";
          js.Add("mode/clike/clike.js");
          js.Add("mode/xml/xml.js");
          js.Add("mode/javascript/javascript.js");
          js.Add("mode/css/css.js");
          js.Add("mode/php/php.js");          
          break;
      case LanguagesEnum.Pascal:
          mode = "text/x-pascal";
          js.Add("mode/pascal/pascal.js");
          break;
      case LanguagesEnum.ObjectiveC:
          mode = "text/x-csrc";
          js.Add("mode/clike/clike.js");
          break;
      case LanguagesEnum.Haskell:
          mode = "text/x-haskell";
          js.Add("mode/haskell/haskell.js");
          break;
      case LanguagesEnum.Ruby:
          mode = "text/x-ruby";
          js.Add("mode/ruby/ruby.js");
          break;
      case LanguagesEnum.Perl:
          mode = "text/x-perl";
          js.Add("mode/perl/perl.js");
          break;
      case LanguagesEnum.Lua:
          mode = "text/x-lua";
          js.Add("mode/lua/lua.js");
          break;
      case LanguagesEnum.Nasm:
          mode = "text/x-nasm";
          js.Add("mode/nasm/nasm.js");
          break;
      case LanguagesEnum.SqlServer:
          mode = "text/x-mssql";
          js.Add("mode/sql/sql.js");
          break;
      case LanguagesEnum.Go:
          mode = "text/x-go";
          js.Add("mode/go/go.js");
          break;
      case LanguagesEnum.Lisp:
          mode = "text/x-common-lisp";
          js.Add("mode/clisp/commonlisp.js");
          break;
      case LanguagesEnum.Prolog:
          mode = "text/x-prolog";
          js.Add("mode/prolog/prolog.js");
          break;
      case LanguagesEnum.Scala:
          mode = "text/x-scala";
          js.Add("mode/scala/scala.js");
          break;
      case LanguagesEnum.Scheme:
          mode = "text/x-scheme";
          js.Add("mode/scheme/scheme.js");
          break;
      case LanguagesEnum.Nodejs:
          mode = "text/javascript";
          js.Add("mode/javascript/javascript.js");
          break;
      case LanguagesEnum.Octave:
          mode = "text/x-octave";
          js.Add("mode/octave/octave.js");
          break;
      case LanguagesEnum.D:
          mode = "text/x-d";
          js.Add("mode/d/d.js");
          break;
      case LanguagesEnum.R:
          mode = "text/x-rsrc";
          js.Add("mode/r/r.js");
          break;
  }
  foreach(var j in js)
  {
      %><script type="text/javascript" src="../../Scripts/codemirror3/<%:j%>"></script><%
  }
  %>
    <%if(Model.ShowComments)
    { %>
    <script>
        //<![CDATA[
        $(document).ready(function () {
            CodeMirror.runMode(document.getElementById("code").value, "<%:mode%>",
                               document.getElementById("output"));


            //$("textarea.mdd_editor").MarkdownDeep({
            //    help_location: "../../Scripts/mdd_help.htm",
            //    disableTabHandling: true
            //});
            $("#upVote").click(VoteUpClick);
            function VoteUpClick() {
                $('#upVote').unbind('click');
                VoteUp();
                setTimeout(function () {
                    $("#upVote").click(VoteUpClick);
                }, 3000);
            }
            function VoteUp() {
                var guid = "<%:Model.Guid%>";
            $.post('/discussion/vote', { Guid: guid, VoteUp: "true" },
            function (data) {
                var obj = jQuery.parseJSON(data);
                if (obj.NotLoggedIn == true) {
                    window.location.replace("<%:Utils.BaseUrl+"login?redirectInfo=discussion/"+Model.Guid%>");
                    return;
                }
                if (obj.AlreadyVoted == true) {
                    return;
                }
                $("#votes_count").text(parseInt($("#votes_count").text(), 10) + 1);
                $("#upVote").hide();
                $("#upVoted").show();
            }, 'text');
        }

            $("#downVote").click(VoteDownClick);
            function VoteDownClick() {
                $('#downVote').unbind('click');
                VoteDown();
                setTimeout(function () {
                    $("#downVote").click(VoteDownClick);
                }, 3000);
            }
            function VoteDown() {
                var guid = "<%:Model.Guid%>";
            $.post('/discussion/vote', { Guid: guid, VoteUp: "false" },
                function (data) {
                    var obj = jQuery.parseJSON(data);
                    if (obj.NotLoggedIn == true) {
                        window.location.replace("<%:Utils.BaseUrl+"login?redirectInfo=discussion/"+Model.Guid%>");
                        return;
                    }
                    if (obj.AlreadyVoted == true) {
                        return;
                    }
                    $("#votes_count").text(parseInt($("#votes_count").text(), 10) - 1);
                    $("#downVote").hide();
                    $("#downVoted").show();
                }, 'text');
            }

            $("#upVoted").click(UpVotedCancelClick);
            $("#downVoted").click(DownVotedCancelClick);

            function UpVotedCancelClick() {
                $('#upVoted').unbind('click');
                CancelVote(true);
                setTimeout(function () {
                    $("#upVoted").click(UpVotedCancelClick);
                }, 3000);
            }
            function DownVotedCancelClick() {
                $('#downVoted').unbind('click');
                CancelVote(false);
                setTimeout(function () {
                    $("#downVoted").click(DownVotedCancelClick);
                }, 3000)
            }
            function CancelVote(up) {
                var guid = "<%:Model.Guid%>";
            $.post('/discussion/CancelVote', { Guid: guid },
            function (data) {
                var obj = jQuery.parseJSON(data);
                if (obj.NotLoggedIn == true) {
                    window.location.replace("<%:Utils.BaseUrl+"login?redirectInfo=discussion/"+Model.Guid%>");
                    return;
                }
                if (obj.AlreadyVoted == true) {
                    return;
                }
                if (up) {
                    $("#votes_count").text(parseInt($("#votes_count").text(), 10) - 1);
                    $("#upVoted").hide();
                    $("#upVote").show();
                }
                else {
                    $("#votes_count").text(parseInt($("#votes_count").text(), 10) + 1);
                    $("#downVoted").hide();
                    $("#downVote").show();
                }
            }, 'text');
        }

        });
    //]]>
    </script>
    <%} %>
</asp:Content>
