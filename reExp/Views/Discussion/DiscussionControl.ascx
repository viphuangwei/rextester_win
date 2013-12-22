<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<reExp.Models.Comment>" %>

<span>
    <span style="display:block;word-wrap:break-word;">
        <%:Html.Raw(Model.Text)%>
        <span style="color:gray; font-size:12px;">&nbsp; by &nbsp;<%:Model.User_Name%>,&nbsp;<%:Model.Date.TimeAgo()%></span>
        <%if(SessionManager.IsUserInSession() && SessionManager.UserId == Model.User_Id)
        {%>
            &nbsp;<a style="font-size:12px;" class="smalllink" href="<%:Utils.BaseUrl+"discussion/EditComment"+"?Guid="+Model.Guid+"&IsEdit=true&Comment_ID="+Model.Id%>">edit</a>
        <%} %>
    </span>
    <br/>
    <hr/>
</span>