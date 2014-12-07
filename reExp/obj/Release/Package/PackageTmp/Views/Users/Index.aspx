<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.users.UsersWallsData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>users</h2>
     <%int maxDisplayLength = 100; %>
     <%if (Model.Walls.Count > 0)
        {%>
            <%foreach (var w in Model.Walls)
                {%>
                    <div class="item">
                        <hr />
                        <div>
                            <a href="<%:Utils.BaseUrl +@"users/"+ w.ID%>" title="<%:w.Name%>">
                                <%: w.Name.BeginningOfString()%>
                            </a>
                        </div>
                    </div>
             <%} %>
            <hr />
            <div  class="pager">
                <%for (int i = 0; i < (int)Math.Ceiling((double)Model.TotalRecords / (double)GlobalConst.RecordsPerPage); i++)
                {%> 
                    <%if (i == Model.Page)
                      {%>
                            <a class="selected" href="<%:Utils.BaseUrl%>users?page=<%:i%>"><%:i + 1%></a>&nbsp;
                    <%}
                      else
                      {%>                     
                            <a href="<%:Utils.BaseUrl%>users?page=<%:i%>"><%:i + 1%></a>&nbsp;
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
    <meta name="Keywords" content="rextester users' code walls" />
    <meta name="Description" content="rextester users' code walls" />
    <link rel="Stylesheet" href="../../Content/List.css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
        
</asp:Content>
