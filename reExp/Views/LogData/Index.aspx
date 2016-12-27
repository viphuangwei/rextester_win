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
                    <%foreach (LanguagesEnum lang in Enum.GetValues(typeof(LanguagesEnum)).OfType<LanguagesEnum>().ToList().Where(f => f != LanguagesEnum.Unknown).OrderBy(f => f.ToLanguage()))
                    {%>
                       <option <%=Model.lang == (int)lang ? "selected" : "" %> value="<%=(int)lang%>"><%=lang.ToLanguage()%></option>
                    <%}%>
                </select>
                &nbsp;&nbsp;&nbsp;View:&nbsp;
                <select id="view" name="view">
                    <option <%=Model.View == 0 ? "selected" : "" %> value="0">Snippets</option>
                    <option <%=Model.View == 1 ? "selected" : "" %> value="1">Run stats</option>
                </select>
                &nbsp;&nbsp;&nbsp;Is api:&nbsp;
                <select id="api" name="api">
                    <option <%=Model.api == 0 ? "selected" : "" %> value="0"></option>
                    <option <%=Model.api == 1 ? "selected" : "" %> value="1">Yes</option>
                    <option <%=Model.api == 2 ? "selected" : "" %> value="2">No</option>
                </select>
                &nbsp;&nbsp;&nbsp;Range:
                <select id="Date_range" name="Date_range" onchange="selectRange()">
                    <option <%=Model.Date_range == 0 ? "selected" : "" %> value="0">Last 24h</option>
                    <option <%=Model.Date_range == 1 ? "selected" : "" %> value="1">Last week</option>
                    <option <%=Model.Date_range == 2 ? "selected" : "" %> value="2">Last month</option>
                    <option <%=Model.Date_range == 3 ? "selected" : "" %> value="3">Custom</option>
                </select>
                <span id="cusotm_range" style="display:none;">
                    &nbsp;&nbsp;&nbsp;From:&nbsp;
                    <input type="text" name="from" value="<%=Model.from%>"/>
                    &nbsp;&nbsp;&nbsp;To:&nbsp;
                    <input type="text" name="to" value="<%=Model.to%>"/>
                </span>
                &nbsp;&nbsp;&nbsp;Search:&nbsp;
                <input type="text" name="search" value="<%=Model.search%>"/>    
                &nbsp;&nbsp;&nbsp;<input type="submit" value="lookup"/>            
            </td>
        </tr>
    </table>
    </form>
    <br/><br />
    <%if (Model.View == 0)
    {%>
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
                                <a href="<%:Utils.BaseUrl + @"logdata?id=" + snippet.Id%>" title="<%:snippet.Data + ""%>">
                                    <%: title.BeginningOfString()%>
                                </a>
                                <br/>
                                <div class="sub"><i><%:((LanguagesEnum)snippet.Lang).ToLanguage()%></i>, <i><%:snippet.Is_api == 1 ? "api, " : ""%></i><i><%:snippet.Result%></i>, <%:snippet.Time%>, <%:DateTime.SpecifyKind(snippet.Time, DateTimeKind.Unspecified).ToUniversalTime().TimeAgo()%>
                                </div>
                            </div>
                        </div>
                 <%} %>
                <hr />
            <%}
            else if (Model.id > 0 && Model.Entries.Count > 0 && Model.Entries.FirstOrDefault() != null)
            {%>
                Code:
                <pre><%=HttpUtility.HtmlEncode(Model.Entries.FirstOrDefault().Data + "")%></pre> 
                Input:
                <pre><%=HttpUtility.HtmlEncode(Model.Entries.FirstOrDefault().Input + "")%></pre>      
                Result:
                <pre><%=HttpUtility.HtmlEncode(Model.Entries.FirstOrDefault().Result + "")%></pre>    
            <%}
            else
            { %>
                <div align="center">Emptiness...</div>
            <%} %>
    <%}
    else {%>
        <%if (Model.Languages_runs != null)
        {%>
            <%foreach (var lang in Model.Languages_runs.OrderByDescending(f => f.Value.Key))
            {%>
                <%=lang.Key.PadRight(15, '|').Replace("|", "&nbsp;")%>&nbsp;&nbsp;&nbsp;<%=(lang.Value.Key.ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + (lang.Value.Value * 100 / lang.Value.Key) + "%")%><br/>
            <%} %>
         <%}%>
        <%if (Model.Language_runs != null)
        {%>
            <%foreach (var lang in Model.Language_runs.OrderBy(f => f.Key))
            {%>
                <%=lang.Key.PadRight(15, '|').Replace("|", "&nbsp;")%>&nbsp;&nbsp;&nbsp;<%=lang.Value.Key.ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + (lang.Value.Value * 100 / lang.Value.Key) + "%"%><br/>
            <%} %>
        <%}%>
    <%}%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Keywords" content="online compiler" />
    <meta name="Description" content="run code online" />
    <link rel="Stylesheet" href="/Content/List.css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        // <![CDATA[
        $(document).ready(function () {
            selectRange();
        });
        function selectRange() {
            if ($("#Date_range").val() == 3) {
                $("#cusotm_range").show();
            }
            else {
                $("#cusotm_range").hide();
            }
        }
        // ]]>
    </script>
</asp:Content>
