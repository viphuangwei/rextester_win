<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.login.UserData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User's stuff
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%if (!Model.IsError)
      { %>        
        <h2><%:Model.UserName%>'s stuff</h2>
            <table style="width:95%">
                <tr>
                    <td align="right">
                        <a href="usersstuff" style="color:white; background-color:gray;">
                            stuff
                        </a>
                        &nbsp;|&nbsp;
                        <a href="notifications" style="color:gray;">
                            notifications
                        </a>
                        <%if(Model.Wall_ID != null) {%>
                        &nbsp;|&nbsp;
                        <a href="<%:Utils.BaseUrl+"users/"+Model.Wall_ID%>" style="color:gray;">
                            my wall
                        </a>
                        <%} %>
                     </td>
                </tr>
            </table>
         <%int maxDisplayLength = 100; %>
         <%if (Model.Items.Count > 0)
            {%>
                <%foreach (var item in Model.Items.OrderByDescending(f => f.Date))
                    {%>
                        <div class="item">
                            <hr />
                            <%if (item.Type == 1)
                                {%>
                                <a href="<%:Utils.BaseUrl+(item.IsLive ? "live/" : "") + item.Guid%>">
                                    <%if (!string.IsNullOrEmpty(item.Title))
                                      {%>
                                        <%:item.Title%>
                                    <%}
                                      else
                                      {%>
                                        <%var firstLine = item.Program.FirstLine();
                                          var title = firstLine != null && firstLine.Length > maxDisplayLength ? firstLine.Substring(0, maxDisplayLength) + "..." : firstLine;
                                          if(string.IsNullOrEmpty(title))
                                              title = "_";%>
                                        <%:title%>
                                    <%}%>
                                </a>
                                <br/>
                                <div class="sub">
                                    <i><%:item.Lang.ToLanguage()%>, <%:item.IsLive ? "live, " : "" %><%:item.IsWall ? "on a wall, " : ""%><%if(item.IsPersonalWall) {%><a href="<%:Utils.BaseUrl%>users/<%:Model.Wall_ID%>">on your wall,</a><%}%></i><%:item.Date.TimeAgo()%>
                                    &nbsp;&nbsp&nbsp;<span style="cursor:pointer;" class="hov" id="<%:item.ID%>">remove</span>
                                </div>
                            <%}
                              else if (item.Type == 2)
                              {%>
                                <a href="<%:Utils.BaseUrl%>tester/<%:item.Guid%>">
                                    <%var firstLine = item.Regex.FirstLine();
                                      var title = firstLine != null && firstLine.Length > maxDisplayLength ? firstLine.Substring(0, maxDisplayLength) + "..." : firstLine;
                                      if(string.IsNullOrEmpty(title))
                                          title="_";%>
                                    <%:title%>
                                </a>
                                <br/>
                                <div  class="sub">
                                    <i>Regex</i>,&nbsp;<%:item.Date.TimeAgo()%>
                                    &nbsp;&nbsp&nbsp;<span style="cursor:pointer;" class="hov" id="<%:item.ID%>">remove</span>
                                </div>
                              <%}
                              else if (item.Type == 3)
                              {%>
                                <a href="<%:Utils.BaseUrl%>replace/<%:item.Guid%>">
                                    <%var firstLine = item.Regex.FirstLine();
                                      var title = firstLine != null && firstLine.Length > maxDisplayLength ? firstLine.Substring(0, maxDisplayLength) + "..." : firstLine;
                                      if(string.IsNullOrEmpty(title))
                                          title = "_";%>
                                    <%:title%>
                                </a>
                                <br/>
                                <div  class="sub">
                                    <i>Regex replace</i>,&nbsp;<%:item.Date.TimeAgo()%>
                                    &nbsp;&nbsp&nbsp;<span style="cursor:pointer;" class="hov" id="<%:item.ID%>">remove</span>
                                </div>
                              <%}%>
                        </div>
                 <%} %>
                <hr />
                <div  class="pager">
                    <%for (int i = 0; i < (int)Math.Ceiling((double)Model.TotalRecords / (double)GlobalConst.RecordsPerPage); i++)
                    {%> 
                        <%if (i == Model.CurrentPage)
                          {%>
                                <a class="selected" href="<%:Utils.BaseUrl%>login/UsersStuff/<%:i%>"><%:i + 1%></a>&nbsp;
                        <%}
                          else
                          {%>                     
                                <a href="<%:Utils.BaseUrl%>login/UsersStuff/<%:i%>"><%:i + 1%></a>&nbsp;
                        <%} %>
                    <%}%>
                </div>
            <%}
              else
              { %>
                <div align="center">Emptiness...</div>
            <%} %>
    <%}
      else
      { %>
        <br/><br/><br/>
        <b><%:Model.Error%></b>
      <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <link rel="Stylesheet" href="../../Content/List.css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        // <![CDATA[
        $(document).ready(function () {

            $.ajaxSetup({
                timeout: 30000,
                error: function (request, status, err) {
                    $("#info").replaceWith("<span style=\"color:red;\" class=\"sub\">Error occurred. Try again later.</span>");
                }
            });

            $("span").click(function (event) {
                Remove(event.target.id);
            });

            function Remove(id) {
                $("#"+id+"").replaceWith("<span class=\"sub\" id=\"info\">removing...</span>");

                $.post('/login/removeitem', { Id: id },
                    function (data) {
                        var obj = jQuery.parseJSON(data);
                        if (obj.Errors == true) {
                            $("#info").replaceWith("<span style=\"color:red;\" class=\"sub\">Error occurred. Try again later.</span>");
                            return;
                        }

                        $("#info").replaceWith("<span style=\"color:red;\" class=\"sub\">Removed from this view.</span>");

                    }, 'text');
            }
        });
        // ]]>
    </script>
</asp:Content>
