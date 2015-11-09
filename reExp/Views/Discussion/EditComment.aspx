<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.discussion.EditData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit comment
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
        <link rel="stylesheet" href="../../Scripts/mdd_styles.css" /> 
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="../../Scripts/MarkdownDeepLib.min.js"></script>

        <script>
            $(document).ready(function () {
                $("textarea.mdd_editor").MarkdownDeep({
                    help_location: "../../Scripts/mdd_help.htm",
                    disableTabHandling: true
                });
            });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit your comment</h2>
    
    <div class="formcontent">
        <%using (Html.BeginForm())
        {%>
            <input type="hidden" name="Comment_ID" id ="Comment_ID" value="<%=Model.Comment_ID%>" />
            <input type="hidden" name="IsEdit" id ="IsEdit" value="false" />
            <input type="hidden" name="Guid" id ="Guid" value="<%=Model.Guid%>" />
            <div style="margin-left:10px;text-align:left; width: 50%" >
                <div class="mdd_toolbar"></div>
                <textarea cols="50" rows="12" class="mdd_editor" id="Text" name="Text"><%=Model.Text%></textarea>
                <div class="mdd_resizer"></div>
            </div>
            <input id="Button" type="submit" value="Submit" style="margin-left:10px; margin-top:5px;"/>
        <%}%>
     </div>
     <div class="mdd_preview" style="margin-left:10px;text-align:left; width: 50%;display:block;word-wrap:break-word;"></div>
</asp:Content>

