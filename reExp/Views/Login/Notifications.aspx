<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.login.NotificationsData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Notifications
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Notifications</h2>
    <table style="width:95%">
        <tr>
            <td align="right">
                <a href="usersstuff" style="color:gray;">
                    stuff
                </a>
                &nbsp;|&nbsp;
                <a href="notifications" style="color:white; background-color:gray;">
                    notifications
                </a>
              <%--<%if(Model.Wall_ID != null) {%>
                &nbsp;|&nbsp;
                <a href="<%:Utils.BaseUrl+"users/"+Model.Wall_ID%>" style="color:gray;">
                    my wall
                </a>
                <%} %>--%>
             </td>
        </tr>
    </table>
    <%if(Model.Notifications.Count ==  0) {%> &nbsp;<span style="color:gray;">None</span><%} %>
    <%foreach (var not in Model.Notifications)
    {%>
        <div class="item" style="padding-top:10px">
            <div>
                <a href="<%:Utils.BaseUrl +(not.ID == null ? "codewall" : (@"users/"+ not.ID))%>" style="color:gray;" title="<%:not.Name%>">
                    New question<%:not.Many ? "s" : "" %>
                </a>
            </div>
        </div>
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
                 Unsubscribe(event.target.id);
             });

             function Unsubscribe(id) {
                 $("#" + id + "").replaceWith("<span class=\"sub\" id=\"info\">removing...</span>");

                 $.post('/codewall/subscribe', { wall_id: (id == -1 ? null : id) },
                     function (data) {
                         var obj = jQuery.parseJSON(data);
                         if (obj.Errors == true) {
                             $("#info").replaceWith("<span style=\"color:red;\" class=\"sub\">Error occurred. Try again later.</span>");
                             return;
                         }

                         $("#info").replaceWith("<span style=\"color:red;\" class=\"sub\">unsubscribed</span>");

                     }, 'text');
             }
         });
         // ]]>
    </script>
</asp:Content>
