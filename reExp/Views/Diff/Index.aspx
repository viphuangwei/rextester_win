<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Diff checker
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="regexpicker">
        <a href="<%:Utils.BaseUrl+"tester"%>" class="regex">.net regex tester</a>&nbsp;|
        <a href="<%:Utils.BaseUrl+"replace"%>" class="regex">replace</a>&nbsp;|
        <a href="<%:Utils.BaseUrl+"reference"%>" class="regex">reference</a>&nbsp;|
        <a href="<%:Utils.BaseUrl+"diff"%>" class="selectedregex">diff checker</a>&nbsp;
    </div>

     <div class="formcontent">
        <table style="width: 95%; margin:0">
            <tr>
                <td>
                    Left:<br />
                    <textarea cols="1000" id="Left" name="Left" rows="10" spellcheck="false" style="width: 96%"></textarea>
                </td>
                <td>
                    Right:<br />
                    <textarea cols="1000" id="Right" name="Right" rows="10" spellcheck="false" style="width: 100%"></textarea>
                </td>
            </tr>
        </table>
        <div style="margin-top:1em;">
            <input style="margin-left:0.5em" id="Compare" type="button" value="Compare" />
        </div>
    </div>

    <pre id="info" class="resultarea" style="text-align:center;"></pre>
    <br/>
    <div id="diff" style="width:100%; margin-top:1em;"></div> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Keywords" content="online diff checker, text comparison, online diff, diff, diff tool, quick diff" />
    <meta name="Description" content="online diff checker" />    
    <link rel="Stylesheet" href="http://rextester.com:8080/Content/Tester.css" />
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
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
    // <![CDATA[
        $(document).ready(function () {
            $.ajaxSetup({
                timeout: 40000,
                error: function (request, status, err) {
                    $("#info").replaceWith("<pre id=\"info\" style=\"text-align:center;\" class=\"resultarea\">Error occurred ("+err+"). Try again later.</pre>");
                }
            });

            $("#Compare").click(function () {
                Submit();
            });

            function Submit() {
                $("#info").replaceWith("<pre id=\"info\" style=\"text-align:center;\" class=\"resultarea\">Comparing...</pre>");
                $("#diff").replaceWith("<div id=\"diff\" style=\"width:100%\"></div>");

                $.post('/diff/Diff', { left: $("#Left").val(), right: $("#Right").val() },
                    function (data) {
                        var obj = jQuery.parseJSON(data);
                        if (obj.IsError == true) {
                            if (obj.Errors != null)
                                $("#info").replaceWith("<pre id=\"info\" style=\"text-align:center;\" class=\"resultarea\">"+obj.Errors+"</pre>");
                            else
                                $("#info").replaceWith("<pre id=\"info\" style=\"text-align:center;\" class=\"resultarea\">Error occurred. Try again later.</pre>");
                            return;
                        }

                        $("#info").replaceWith("<pre id=\"info\" style=\"text-align:center;\" class=\"resultarea\"></pre>");

                        if (obj.Result != null)
                            $("#diff").html(obj.Result);

                        $('html, body').animate({ scrollTop: $("#diff").offset().top }, 500);

                    }, 'html');
            }
        });
    // ]]>
    </script>
</asp:Content>
