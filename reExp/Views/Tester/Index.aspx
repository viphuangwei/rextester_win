<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.regex.RegexData>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
Regex tester
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MetaContent" runat="server">    
    <meta name="Keywords" content="online .net regex tester" />
    <meta name="Description" content="online .net regex tester" />  
    <link rel="Stylesheet" href="http://rextester.com:8080/Content/Tester.css" />  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
    // <![CDATA[
        $(document).ready(function () {
            $("#Test").click(function () {
                Match();
            });
            $("input[type=checkbox]").click(function () {
                Match();
            });
            $('textarea').bind('keydown', function (e) {
                var code = e.keyCode || e.which;
                if (code == 119 || code == 9) //F8 or TAB
                    Match();
            });

            $("#Save").click(function () {
                $("#SavedOutput").val($("#Result").html());
                Save();
            });

            $.ajaxSetup({
                timeout: 30000,
                error: function (request, status, err) {
                    $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Error occurred. Try again later.</pre>");
                }
            });

            function Match() {
                var maxChar = 500000;
                if ($("#Text").val().length > maxChar) {
                    $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\"></pre>");
                    $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Text too long (max " + maxChar + " characters).</pre>");
                    return;
                }

                $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\"></pre>");
                $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Working...</pre>");
                
                var serializedData = $("form").serialize();
                $.post('/tester/TakeText', serializedData,
                                    function (data) {
                                        $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\"></pre>");
                                        $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\">" + data + "</pre>");
                                    }, 'html');
            };

            function Save() {
                $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\"></pre>");
                $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Saving...</pre>");                
                var serializedData = $("form").serialize();
                $.post('/tester/Save', serializedData,
                        function (data) {
                            var obj = jQuery.parseJSON(data);
                            $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\"></pre>");
                            $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Permanent link: <a href=\"" + obj.Url + "\">" + obj.Url + "</a></pre>");                         
                        }, 'text');
            }
            $("#Pattern").focus();
        });
    // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="regexpicker">
        <a href="<%:Utils.BaseUrl+"tester"%>" class="selectedregex">.net regex tester</a>&nbsp;|
        <a href="<%:Utils.BaseUrl+"replace"%>" class="regex">replace</a>&nbsp;|
        <a href="<%:Utils.BaseUrl+"reference"%>" class="regex">reference</a>&nbsp;|
        <a href="<%:Utils.BaseUrl+"diff"%>" class="regex">diff checker</a>&nbsp;
    </div>
    <% using (Html.BeginForm("Index", "tester"))
       {%>
       <div class="formcontent">
            <table style="width: 95%; margin:0">
                <tr>
                    <td>
                        Pattern:<br />
                        <%: Html.TextAreaFor(model => model.Pattern, new { style = "width: 96%", rows = 10, cols = 1000, spellcheck = "false" })%>
                    </td>
                    <td>
                        Text:<br />
                        <%: Html.TextAreaFor(model => model.Text, new { style = "width: 100%", rows = 10, cols = 1000, spellcheck = "false" })%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <%for (int i = 0; i < Model.Options.Count; i++)
                          {%>
                                <span class="options">
                                <%: Html.CheckBoxFor(model => model.Options[i], new { title = Model.Describtions[i].Comment, id = Model.Describtions[i].Title })%>
                                <%: Html.Label(Model.Describtions[i].Title)%>
                                </span>
                        <%} %>
                    </td>
                </tr>
            </table>
            <div style="margin-top:1em;">
                <input style="margin-left:0.5em" id="Test" type="button" value="Test it (F8)" />
                <input style="margin-left:1em" id="Save" type="button" value="Save it" />
            </div>
            <input id="SavedOutput" name="SavedOutput" type="hidden" value="<%=Model.SavedOutput%>" />
        </div>
    <%} %>
    <br/>
    <pre class="resultarea" id="NonResultMessage"></pre>
    <pre class="resultarea" id="Result"><%:@Html.Raw(@Model.Result)%></pre>
</asp:Content>
