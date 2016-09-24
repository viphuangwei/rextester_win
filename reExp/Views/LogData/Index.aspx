<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.log.LogModel>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Log - <%=Model.Total %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Log - <%=Model.Total %></h2>
    <form method="post">
    <table style="width:95%">
        <tr>
            <td align="right">
                <input type="hidden" name="id" value="0"/>
                <select id="lang" name="lang">
                    <option <%=Model.lang == 0 ? "selected" : "" %> value="0">All</option>
                    <%
                    foreach (LanguagesEnum lang in Enum.GetValues(typeof(LanguagesEnum)).OfType<LanguagesEnum>().ToList().Where(f => f != LanguagesEnum.Unknown).OrderBy(f => f.ToLanguage()))
                    {%>
                       <option <%=Model.lang == (int)lang ? "selected" : "" %> value="<%=(int)lang%>"><%=lang.ToLanguage()%></option>
                    <%}%>
                </select>
                &nbsp;&nbsp;&nbsp;Is api:&nbsp;
                <select id="api" name="api">
                    <option <%=Model.api == 0 ? "selected" : "" %> value="0"></option>
                    <option <%=Model.api == 1 ? "selected" : "" %> value="1">Yes</option>
                    <option <%=Model.api == 2 ? "selected" : "" %> value="2">No</option>
                </select>
                &nbsp;&nbsp;&nbsp;From:&nbsp;
                <input type="text" name="from" value="<%=Model.from%>"/>
                &nbsp;&nbsp;&nbsp;To:&nbsp;
                <input type="text" name="to" value="<%=Model.to%>"/>
                &nbsp;&nbsp;&nbsp;Search:&nbsp;
                <input type="text" name="search" value="<%=Model.search%>"/>    
                &nbsp;&nbsp;&nbsp;<input type="submit" value="lookup"/>            
            </td>
        </tr>
    </table>
    </form>
    <br/><br />
    <%int maxDisplayLength = 100; %>
    <%if (Model.id == 0 && Model.Entries.Count > 0)
        {%>
            <%foreach (var snippet in Model.Entries)
                {%>
                    <%
                    string title = "";
                    if (string.IsNullOrEmpty(snippet.Data))
                    {
                        title = "Empty";
                    }
                    else
                    {
                        var firstLine = snippet.Data.FirstLine();
                        title = firstLine != null && firstLine.Length > maxDisplayLength ? firstLine.Substring(0, maxDisplayLength) + "..." : firstLine;
                        if (string.IsNullOrEmpty(title))
                            title = "_";
                    }%>
                    <div class="item">
                        <hr />
                        <div style="display:inline-block;">
                            <a href="<%:Utils.BaseUrl +@"logdata?id="+ snippet.Id%>" title="<%:snippet.Data+""%>">
                                <%: title.BeginningOfString()%>
                            </a>
                            <br/>
                            <div class="sub"><i><%:((LanguagesEnum)snippet.Lang).ToLanguage()%></i>, <i><%:snippet.Is_api == 1 ? "api, " : ""%></i><i><%:snippet.Result%></i>, <%:snippet.Time%>, <%:DateTime.SpecifyKind(snippet.Time,DateTimeKind.Unspecified).ToUniversalTime().TimeAgo()%>
                            </div>
                        </div>
                    </div>
             <%} %>
            <hr />
        <%}
        else if(Model.id > 0 && Model.Entries.Count > 0 && Model.Entries.FirstOrDefault() != null)
        {%>
            Code:
            <pre><%=HttpUtility.HtmlEncode(Model.Entries.FirstOrDefault().Data+"")%></pre> 
            Input:
            <pre><%=HttpUtility.HtmlEncode(Model.Entries.FirstOrDefault().Input+"")%></pre>      
            Result:
            <pre><%=HttpUtility.HtmlEncode(Model.Entries.FirstOrDefault().Result+"")%></pre>    
        <%}
        else
        { %>
            <div align="center">Emptiness...</div>
        <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Keywords" content="online compiler" />
    <meta name="Description" content="run code online" />
    <link rel="Stylesheet" href="/Content/List.css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">

</asp:Content>
