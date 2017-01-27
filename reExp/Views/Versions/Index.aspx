<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.versions.VersionsData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Versions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>versions</h2>
    <div class="item">
        <%string latest = Model.IsLive ? "Live version" : "Latest version";
          string version = Model.IsLive ? "Snapshot" : "Version"; %>
        <%if (!Model.IsLive)
          {%>
            <input id="radio_left_0" name="LeftChecked" type="radio" value="<%:Model.CodeGuid%>" />
            <input id="radio_right_0" name="RightChecked" type="radio" value="<%:Model.CodeGuid%>" />
        <%}
          else
          {%>
            <input id="radio_left_0" name="LeftChecked" type="radio" value="<%:Model.CodeGuid%>" disabled="disabled"/>
            <input id="radio_right_0" name="RightChecked" type="radio" value="<%:Model.CodeGuid%>" disabled="disabled"/>
          <%} %>
        <a href="<%=Utils.BaseUrl+(Model.IsLive ? "live/" : "" )+Model.CodeGuid%>"><%:latest%>:&nbsp;&nbsp;<%:Model.Title%></a>
        <div class="sub" style="display:inline-block;">
            <i><%if (Model.Author != null)
                {
                      if(Model.Author.Wall_ID != null)
                      {%>
                        by <a href="<%:Utils.BaseUrl+"users/"+Model.Author.Wall_ID%>"><%:Model.Author.Name.StripEmail()%></a>,
                     <%}
                     else
                     {%>
                       by <%:Model.Author.Name.StripEmail()%>,
                     <%}%>                                          
                <%}%>
                <%:Model.CreationDate.TimeAgo()%>
            </i>
        </div>
    </div>
    <hr/>
    <%
        var ordered = Model.Versions.OrderByDescending(f => f.CreationDate).ToList();  
        for (int i = 0; i < ordered.Count; i++)
        {%>
              
            <div class="item">
            <input id="radio_left_<%:i+1%>" name="LeftChecked" type="radio" value="<%:ordered[i].Guid%>" />
            <input id="radio_right_<%:i+1%>" name="RightChecked" type="radio" value="<%:ordered[i].Guid%>" />
            <a href="<%=Utils.BaseUrl+ordered[i].Guid%>"><%:version%>&nbsp;<%:ordered.Count-i%>:&nbsp;&nbsp;<%:ordered[i].Title%></a>
            <div class="sub" style="display:inline-block;">
                <%if (ordered[i].Wall_id != null) 
                {%>
                    <i>by <a href="<%:Utils.BaseUrl+"users/"+ordered[i].Wall_id%>"><%:ordered[i].Author.StripEmail()%></a>, <%:ordered[i].CreationDate.TimeAgo()%></i>
                <%}
                else 
                {%>
                    <i><%:string.IsNullOrEmpty(ordered[i].Author) ? "" : " by " + ordered[i].Author.StripEmail() + ", "%><%:ordered[i].CreationDate.TimeAgo()%></i> 
                <%}%>
            </div>
            </div>
            <hr/>
    <%}%>
    <input id="compare" type="button" value="Compare" style="margin-top:1em;"/>
    <input id="CodeGuid" type="hidden" value="<%:Model.CodeGuid%>" />
    <pre id="info" class="resultarea" style="text-align:center;"></pre>
    <div id="diff" style="width:100%; margin-top:1em;">
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <%if (!SessionManager.IsDarkTheme)
    {%>
    <link rel="Stylesheet" href="/Content/List.css" />
     <style type="text/css">
        table.diff {font-family:Courier; border:medium;}
        .diff_header {background-color:#e0e0e0}
        td.diff_header {text-align:right}
        .diff_next {background-color:#c0c0c0}
        .diff_add {background-color:#aaffaa}
        .diff_chg {background-color:#ffff77}
        .diff_sub {background-color:#ffaaaa}
        /* customized style */
        table.diff {font-family:monospace; border:medium;font-size:14px;}
    </style>
    <%}
    else { %>
     <link rel="Stylesheet" href="/Content/ListDark.css" />
     <style type="text/css">
         table { color: #929292; }
        table.diff {font-family:Courier; border:medium;}
        .diff_header {background-color:#1a1a1a; }
        td.diff_header {text-align:right;background-color:#1a1a1a;}
        .diff_next {background-color:#1a1a1a;}
        .diff_add {background-color:#aaffaa;color: black;}
        .diff_chg {background-color:#ffff77;color: black;}
        .diff_sub {background-color:#ffaaaa;color: black;}
        /* customized style */
        table.diff {font-family:monospace; border:medium;font-size:14px;background-color:#1a1a1a;}
    </style>
    <%} %>
   
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        // <![CDATA[
        $(document).ready(function () {

            $.ajaxSetup({
                timeout: 30000,
                error: function (request, status, err) {
                    $("#info").replaceWith("<pre id=\"info\" style=\"text-align:center;\" class=\"resultarea\">Error occurred. Try again later.</pre>");
                }
            });

            $("#compare").click(function () {
                Submit();
            });

            function Submit() {
                if ($("input[name='LeftChecked']:checked").val() != null && $("input[name='RightChecked']:checked").val() != null) {

                    $('html, body').animate({ scrollTop: $("#info").offset().top }, 500);

                    $("#info").replaceWith("<pre id=\"info\" style=\"text-align:center;\" class=\"resultarea\">Comparing...</pre>");
                    $("#diff").replaceWith("<div id=\"diff\" style=\"width:100%\"></div>");

                    $.post('/versions/GetDiffHtml', { CodeGuid: $("#CodeGuid").val(), LeftGuid: $("input[name='LeftChecked']:checked").val(), RightGuid: $("input[name='RightChecked']:checked").val() },
                        function (data) {
                            var obj = jQuery.parseJSON(data);
                            if (obj.Errors == true) {
                                $("#info").replaceWith("<pre id=\"info\" style=\"text-align:center;\" class=\"resultarea\">Error occurred. Try again later.</pre>");
                                return;
                            }

                            $("#info").replaceWith("<pre id=\"info\" style=\"text-align:center;\" class=\"resultarea\"></pre>");

                            if (obj.Result != null)
                                $("#diff").html(obj.Result);

                            $('html, body').animate({ scrollTop: $("#diff").offset().top }, 500);

                        }, 'html');
                }
            }
        });
        // ]]>
    </script>
</asp:Content>
