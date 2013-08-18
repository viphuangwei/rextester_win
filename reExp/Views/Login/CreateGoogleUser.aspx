<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.login.GoogleUser>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Account details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Fill account details</h2>

    <% using (Html.BeginForm("CreateGoogleUser", "login"))
       {%>
       <div class="formcontent">
            <div>
                Your name for display:<br/>
                <%: Html.TextBoxFor(model => model.Name, new { size = 70, spellcheck = "false" })%>
            </div>
            <input type="submit" value="Submit" style="margin-top: 5px;" />
        </div>
        <%: Html.HiddenFor(f => f.EmailHash) %>
        <input type="hidden" value="<%:Model.redirectInfo%>" name="redirectInfo" id="redirectInfo" />
    <% } %>
    <pre class="resultarea" id="result"><%
        if (!string.IsNullOrEmpty(Model.Error))
        {      
            %><span style="color: red"><%:Model.Error%></span><%
        }
    %></pre>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
