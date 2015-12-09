<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.regex.RegexData>" %>


<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
Regex replacement
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MetaContent" runat="server">    
    <meta name="Keywords" content=".net regex replacement" />
    <meta name="Description" content=".net regex replacement" />
    <link rel="Stylesheet" href="http://stats.rextester.com/Content/Tester.css" /> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
     <script src="http://stats.rextester.com/Scripts/ZeroClipboard.min.js" type="text/javascript">
     </script>
    <script type="text/javascript">
    // <![CDATA[
        function prepareCopyText() {
            var res = '';
            var interm = $("#ResultText").html().replace(new RegExp("<br/?>", "gi"), "<br/><div/>");
            var arr = interm.split(new RegExp("<br/?>", "i"));
            var i;
            var l;
            for (i = 0, l = arr.length; i < l; i++) {
                res += $('<div/>').html(arr[i].replace(/<.*?>/g, '')).text();
                if (i < l - 1) {
                    res += "\r\n";
                }
            }
            return res;
        }

        $(document).ready(function () {
            ZeroClipboard.setMoviePath('http://stats.rextester.com/Content/ZeroClipboard.swf');
            var clip = new ZeroClipboard.Client();
            clip.setHandCursor(true);
            if ($("#ResultText").length > 0) {
                clip.setText(prepareCopyText());
            }
            else {
                clip.setText(' ');
            }
            clip.glue('d_clip_button', 'd_clip_container');


            $("#Replace").click(function () {
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

            var Match = function () {
                var maxChar = 500000;
                if ($("#Text").val().length > maxChar) {
                    $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\"></pre>");
                    $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Text too long (max " + maxChar + " characters).</pre>");
                    return;
                }

                $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\"></pre>");
                $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Working...</pre>");

                var serializedData = $("form").serialize();
                $.post('/replace/TakeText', serializedData,
                            function (data) {
                                $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\"></pre>");
                                $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\">" + data + "</pre>");
                                if ($("#ResultText").length > 0) {
                                    clip.setText(prepareCopyText());
                                }
                                else {
                                    clip.setText(' ');
                                }
                            }, 'html');
            };

            function Save() {
                $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\"></pre>");
                $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Saving...</pre>");
                var serializedData = $("form").serialize();
                $.post('/replace/Save', serializedData,
                            function (data) {
                                var obj = jQuery.parseJSON(data);
                                $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\"></pre>");
                                $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Permanent link: <a href=\"" + obj.Url + "\">" + obj.Url + "</a></pre>");
                            }, 'text');
            };

            $("#Pattern").focus();

        });
    // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    <div class="regexpicker">
        <a href="<%:Utils.BaseUrl+"tester"%>" class="regex">.net regex tester</a>&nbsp;|
        <a href="<%:Utils.BaseUrl+"replace"%>" class="selectedregex">replace</a>&nbsp;|
        <a href="<%:Utils.BaseUrl+"reference"%>" class="regex">reference</a>&nbsp;|
        <a href="<%:Utils.BaseUrl+"diff"%>" class="regex">diff checker</a>&nbsp;
    </div>
    <% using (Html.BeginForm("Index", "replace"))
       {%>
       <div class="formcontent">
            <table style="width: 95%; margin: 0">
                <tr>
                    <td valign="top">
                        Pattern:<br />
                        <%: Html.TextAreaFor(model => model.Pattern, new { style = "width: 96%", rows = 4, cols = 1000, tabindex = 1, spellcheck = "false" })%>
                    </td>
                    <td rowspan="2">
                        Text:<br />
                        <%: Html.TextAreaFor(model => model.Text, new { style = "width: 100%", rows = 11, cols = 1000, tabindex = 3, spellcheck = "false" })%>
                    </td>
                </tr>
                <tr>
                    <td valign="bottom">
                        Substitution:<br />
                        <%: Html.TextAreaFor(model => model.Substitution, new { style = "width: 96%", rows = 4, cols = 1000, tabindex = 2, spellcheck = "false" })%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <%for (int i = 0; i < Model.Options.Count; i++)
                          {%>
                            <span class="options">
                                <%: Html.CheckBoxFor(model => model.Options[i], new {tabindex=4+i, title = Model.Describtions[i].Comment, id = Model.Describtions[i].Title })%>
                                <%: Html.Label(Model.Describtions[i].Title)%>     
                            </span>              
                        <%} %>
                    </td>
                </tr>
            </table>
            <table style="width: 95%;margin-top:1em;">
                <tr>
                    <td align="left">
                        <input style="margin-left:0.5em" id="Replace" type="button" value="Replace it (F8)" />
                        <input style="margin-left:1em" id="Save" type="button" value="Save it" />
                    </td>
                    <td align="right">
                        <div id="d_clip_container" style="position:relative">
                           <div id="d_clip_button" style="text-decoration: underline;color:Gray;">Copy result</div>
                        </div>
                    </td>
                </tr>
            </table>
            <input id="SavedOutput" name="SavedOutput" type="hidden" value="<%=Model.SavedOutput%>" />
        </div>
    <%} %>
    <br/>
    <pre class="resultarea" id="NonResultMessage"></pre>
    <pre class="resultarea" id="Result"><%:@Html.Raw(@Model.Result)%></pre>
    
</asp:Content>
