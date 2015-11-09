<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<reExp.Controllers.discussion.RelatedEntry>>" %>

<span>
    <%foreach (var link in Model)
    {%> 
        <div style="word-wrap:break-word;margin-bottom:10px;max-width:150px;">
            <a class="related" href="<%:Utils.BaseUrl +@"discussion/"+ link.Guid + @"/" + link.Title.StringToUrl()%>" title="<%:link.Title%>"><%:link.Title%></a> 
        </div>
   <%}%>
</span>