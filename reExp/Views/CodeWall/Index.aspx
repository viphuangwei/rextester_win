<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.codewall.CodeWallData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Code wall
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>code wall</h2>
    <table style="width:95%">
        <tr>
            <td align="right">
                <%if (Model.Sort == 0)
                  {%>
                <span id="newest" class="sort_selected">
                    newest
                </span>
                &nbsp;|&nbsp;
                <span id="votes" class="sort_not_selected">
                    votes
                </span>
                <%} 
                else
                {%>
                <span id="newest" class="sort_not_selected">
                    newest
                </span>
                &nbsp;|&nbsp;
                <span id="votes" class="sort_selected">
                    votes
                </span>
                <%} %>
                <%if (Model.IsSubscribed)
                  {%>
                &nbsp;|&nbsp;
                <span id="subscribe_cell"><span id="subscribe" style="cursor:pointer;color: gray;">
                    unsubscribe
                </span></span>
                <%} else {%>
                &nbsp;|&nbsp;
                <span id="subscribe_cell"><span id="subscribe" style="cursor:pointer;color:gray;">
                    subscribe
                </span></span>
                <%} %>
            </td>
        </tr>
    </table>
     <%int maxDisplayLength = 100; %>
     <%if (Model.Codes.Count > 0)
        {%>
            <%foreach (var snippet in Model.Codes)
                {%>
                    <%
                    string title = "";
                    if (!string.IsNullOrEmpty(snippet.Title))
                    {
                        title = snippet.Title;
                    }
                    else
                    {
                        var firstLine = snippet.Program.FirstLine();
                        title = firstLine != null && firstLine.Length > maxDisplayLength ? firstLine.Substring(0, maxDisplayLength) + "..." : firstLine;
                        if (string.IsNullOrEmpty(title))
                            title = "_";
                    }%>
                    <div class="item">
                        <hr />
                        <div class="holder">
                            <span class="number"><%:snippet.Votes%></span>
                            <br />
                            <span class="word">votes</span>
                        </div>
                        <div class="holder">
                            <span class="number"><%:snippet.Views.Number()%></span>
                            <br />
                            <span class="word">views</span>
                        </div>
                        <div style="display:inline-block;">
                            <a href="<%:Utils.BaseUrl +@"discussion/"+ snippet.Guid + @"/" + title.StringToUrl()%>" title="<%:title%>">
                                <%: title.BeginningOfString()%>
                            </a>
                            <br/>
                            <div class="sub"><i><%:snippet.Lang.ToLanguage()%></i>, <%:snippet.Date.TimeAgo()%>
                            <%if(Model.IsAdmin) {%>
                                &nbsp;&nbsp&nbsp;<span style="cursor:pointer;" class="hov" id="<%:snippet.Wall_ID%>">remove</span>
                            <%} %>
                            </div>
                        </div>
                    </div>
             <%} %>
            <hr />
            <div  class="pager">
                <%for (int i = 0; i < (int)Math.Ceiling((double)Model.TotalRecords / (double)GlobalConst.RecordsPerPage); i++)
                {%> 
                    <%if (i == Model.Page)
                      {%>
                            <a class="selected" href="<%:Utils.BaseUrl%>codewall?page=<%:i%>&sort=<%:Model.Sort%>"><%:i + 1%></a>&nbsp;
                    <%}
                      else
                      {%>                     
                            <a href="<%:Utils.BaseUrl%>codewall?page=<%:i%>&sort=<%:Model.Sort%>"><%:i + 1%></a>&nbsp;
                    <%} %>
                <%}%>
            </div>
        <%}
          else
          { %>
            <div align="center">Emptiness...</div>
        <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Keywords" content="online compiler" />
    <meta name="Description" content="run code online" />
    <link rel="Stylesheet" href="http://rextester.com:8080/Content/List.css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        // <![CDATA[
        $(document).ready(function () {
            $("#newest").click(function () {
                window.location.replace("<%=Utils.BaseUrl+"codewall?page=0&sort=0"%>");
            });
            $("#votes").click(function () {
                window.location.replace("<%=Utils.BaseUrl+"codewall?page=0&sort=1"%>");
            });

            $.ajaxSetup({
                timeout: 30000,
                error: function (request, status, err) {
                    $("#subscribe").replaceWith("<span id=\"subscribe\" style=\"cursor:pointer; color:gray;\">Error occurred. Try again later.</span>");
                }
            });

            $("#subscribe_cell").click(function (event) {
                $("#subscribe").replaceWith("<span id=\"subscribe\" style=\"cursor:pointer; color:gray;\">saving...  </span>");

                $.post('/codewall/subscribe', { wall_id: null },
                    function (data) {
                        var obj = jQuery.parseJSON(data);
                        if (obj.Errors == true) {
                            $("#subscribe").replaceWith("<span id=\"subscribe\" style=\"cursor:pointer; color:gray;\">Error occurred. Try again later.</span>");
                            return;
                        }
                        if (obj.NotLoggedIn == true) {
                            window.location.replace("<%:Utils.GetUrl(Utils.PagesEnum.Login)%>");
                            return;
                        }

                        if (obj.Subscribed) {
                            $("#subscribe").replaceWith("<span id=\"subscribe\" style=\"cursor:pointer; color:gray;\">unsubscribe</span>");
                        }
                        else {
                            $("#subscribe").replaceWith("<span id=\"subscribe\" style=\"cursor:pointer; color:gray;\">subscribe  </span>");
                        }
                    }, 'text');
            });

            <%if (Model.IsAdmin) {%>
                $(".sub").children("span").click(function (event) {
                    Remove(event.target.id);
                });

                function Remove(id) {
                    $("#" + id + "").replaceWith("<span class=\"sub\" id=\"info\">removing...</span>");

                    $.post('/codewall/removeitem', { Id: id },
                        function (data) {
                            var obj = jQuery.parseJSON(data);
                            if (obj.Errors == true) {
                                $("#info").replaceWith("<span style=\"color:red;\" class=\"sub\">Error occurred. Try again later.</span>");
                                return;
                            }

                            $("#info").replaceWith("<span style=\"color:red;\" class=\"sub\">Removed from this view.</span>");

                        }, 'text');
                }
            <%}%>
        });

            // ]]>
    </script>
</asp:Content>
