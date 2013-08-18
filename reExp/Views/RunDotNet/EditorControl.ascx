<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<reExp.Controllers.rundotnet.RundotnetData>" %>

<%string mode = null;
  string theme = null;
  string syntax = null;
  string js = null;
  List<string> additionalJs = new List<string>();
  switch (Model.LanguageChoice)
  {
      case LanguagesEnum.CSharp:
          mode = "text/x-csharp";
          theme = "csharp";
          syntax = "csharp";
          js = "mode/clike/clike.js";
          break;
      case LanguagesEnum.VB:
          mode = "text/x-vb";
          syntax = "vb1";
          theme = "csharp";
          js = "mode/vb/vb.js";        
          break;
      case LanguagesEnum.FSharp:          
          mode = "text/x-fsharp";
          syntax = "fsharp";
          theme = "csharp";
          js = "mode/clike/clike.js";
          break;
      case LanguagesEnum.Java:
          mode = "text/x-java";
          theme = "java";
          syntax = "java";
          js = "mode/clike/clike.js";
          break;
      case LanguagesEnum.Javascript:
          mode = "text/javascript";
          syntax = "js";
          js = "mode/javascript/javascript.js";
          break;
      case LanguagesEnum.Python:
          mode = "python";
          syntax = "python";
          js = "mode/python/python.js";
          break;
      case LanguagesEnum.Python3:
          mode = "python";
          syntax = "python";
          js = "mode/python/python.js";
          break;
      case LanguagesEnum.C:
          mode = "text/x-csrc";
          syntax = "c";
          js = "mode/clike/clike.js";
          break;
      case LanguagesEnum.CClang:
          mode = "text/x-csrc";
          syntax = "c";
          js = "mode/clike/clike.js";
          break;
      case LanguagesEnum.CPP:
          mode = "text/x-c++src";
          syntax = "cpp";
          js = "mode/clike/clike.js";
          break;
      case LanguagesEnum.CPPClang:
          mode = "text/x-c++src";
          syntax = "cpp";
          js = "mode/clike/clike.js";
          break;
      case LanguagesEnum.Php:
          mode = "application/x-httpd-php";
          syntax = "php";
          js = "mode/clike/clike.js";
          additionalJs.Add("mode/xml/xml.js");
          additionalJs.Add("mode/javascript/javascript.js");
          additionalJs.Add("mode/css/css.js");
          additionalJs.Add("mode/php/php.js");          
          break;
      case LanguagesEnum.Pascal:
          mode = "text/x-pascal";
          syntax = "pas";
          js = "mode/pascal/pascal.js";
          break;
      case LanguagesEnum.ObjectiveC:
          mode = "text/x-csrc";
          syntax = "c";
          js = "mode/clike/clike.js";
          break;
      case LanguagesEnum.Haskell:
          mode = "text/x-haskell";
          syntax = "haskell";
          js = "mode/haskell/haskell.js";
          break;
      case LanguagesEnum.Ruby:
          mode = "text/x-ruby";
          syntax = "ruby";
          js = "mode/ruby/ruby.js";
          break;
      case LanguagesEnum.Perl:
          mode = "text/x-perl";
          syntax = "perl";
          js = "mode/perl/perl.js";
          break;
      case LanguagesEnum.Lua:
          mode = "text/x-lua";
          syntax = "lua";
          js = "mode/lua/lua.js";
          break;
      case LanguagesEnum.Nasm:
          mode = "text/x-nasm";
          syntax = "nasm";
          js = "mode/nasm/nasm.js";
          break;
      case LanguagesEnum.SqlServer:
          mode = "text/x-mysql";
          syntax = "tsql";
          js = "mode/mysql/mysql.js";
          break;
      case LanguagesEnum.Go:
          mode = "text/x-go";
          js = "mode/go/go.js";
          syntax = "go";
          break;
      case LanguagesEnum.Lisp:
          mode = "text/x-common-lisp";
          js = "mode/clisp/commonlisp.js";
          syntax = "clisp";
          break;
      case LanguagesEnum.Prolog:
          mode = "text/x-prolog";
          js = "mode/prolog/prolog.js";
          syntax = "prolog";
          break;
      case LanguagesEnum.Scala:
          mode = "text/x-scala";
          theme = "java";
          js = "mode/scala/scala.js";
          syntax = "scala";
          break;
      case LanguagesEnum.Scheme:
          mode = "text/x-scheme";
          js = "mode/scheme/scheme.js";
          syntax = "scheme";
          break;
      case LanguagesEnum.Nodejs:
          mode = "text/javascript";
          syntax = "js";
          js = "mode/javascript/javascript.js";
          break;
      case LanguagesEnum.Octave:
          mode = "text/x-octave";
          syntax = "octave";
          js = "mode/octave/octave.js";
          break;
  }
%>
<%if(Model.EditorChoice == EditorsEnum.Codemirror) 
{%>
    <%if(js != null)
    {%>
        <script type="text/javascript" src="../../Scripts/codemirror2/<%:js%>"></script>
    <%} %>
    <%foreach(var a in additionalJs)
    {%>
        <script type="text/javascript" src="../../Scripts/codemirror2/<%:a%>"></script>
    <%}%>
    <script type="text/javascript">
        //<![CDATA[
        $(document).ready(function () {
            var editor = CodeMirror.fromTextArea(document.getElementById("Program"), {
                <% if(Model.LanguageChoice == LanguagesEnum.Python)
                {%>
                    mode:
                    {
                        name: "<%:mode%>",
                        version: 2,
                        singleLineStringErrors: false
                    },
                <%} 
                else if(Model.LanguageChoice == LanguagesEnum.Python3)
                {%>
                    mode:
                    {
                        name: "<%:mode%>",
                        version: 3,
                        singleLineStringErrors: false
                    },
                <%} 
                else
                {%>
                    mode: "<%:mode%>",
                <%} %>
                <% if(Model.LanguageChoice == LanguagesEnum.Nasm || 
                      Model.LanguageChoice == LanguagesEnum.Javascript ||
                      Model.LanguageChoice == LanguagesEnum.Nodejs ||
                      Model.LanguageChoice == LanguagesEnum.Scala || 
                      Model.LanguageChoice == LanguagesEnum.Prolog ||
                      Model.LanguageChoice == LanguagesEnum.FSharp)
                {%>
                    //smartIndent: false,
                <%} %>
                lineNumbers: true,
                indentUnit: 4,
                matchBrackets: true,
                <%if(!string.IsNullOrEmpty(theme)) 
                {%>
                    theme: "<%:theme%>",
                <%}%>
                onKeyEvent: keyEvent,
            extraKeys: {
                <%if(Model.IsIntellisense) 
                {%>
                        "Ctrl-Space" : "autocomplete",
                <%}%>
                    "Tab": "indentMore", 
                    "Shift-Tab": "indentLess",
                    "F11": function() {
                          var scroller = editor.getScrollerElement();
                          if (scroller.className.search(/\bCodeMirror-fullscreen\b/) === -1) {
                            scroller.className += " CodeMirror-fullscreen";
                            scroller.style.height = "100%";
                            scroller.style.width = "100%";
                            editor.refresh();
                          } else {
                            scroller.className = scroller.className.replace(" CodeMirror-fullscreen", "");
                            scroller.style.height = '';
                            scroller.style.width = '';
                            editor.refresh();
                          }
                        },
                    "Esc": function() {
                      var scroller = editor.getScrollerElement();
                      if (scroller.className.search(/\bCodeMirror-fullscreen\b/) !== -1) {
                        scroller.className = scroller.className.replace(" CodeMirror-fullscreen", "");
                        scroller.style.height = '';
                        scroller.style.width = '';
                        editor.refresh();
                      }
                    }, 
                    "F8": function() {
                      var scroller = editor.getScrollerElement();
                      if (scroller.className.search(/\bCodeMirror-fullscreen\b/) !== -1) {
                        scroller.className = scroller.className.replace(" CodeMirror-fullscreen", "");
                        scroller.style.height = '';
                        scroller.style.width = '';
                        editor.refresh();
                      }
                   }                
                }
            });
            GlobalEditor = editor;
            
            <%if(Model.IsIntellisense) 
            {%>
                CodeMirror.commands.autocomplete = function(cm) {
                    CodeMirror.LanguageHint(cm, CodeMirror.simpleHint);
                    //setTimeout(function(){CodeMirror.LanguageHint(cm, CodeMirror.simpleHint)}, 50);
                }
            <%}%>
            <%if(Model.IsLive && Model.EditorChoice == EditorsEnum.Codemirror)
            { %>
                var guid = '<%:Model.CodeGuid %>';
                var connection = sharejs.open(guid.toUpperCase(), 'text', 'http://226589.s.dedikuoti.lt:8000/channel', function(error, doc) {
                    if (error) {
  		                $('#Link').text("Error occurred while establishing live session. Try again later.");
                        console.error(error);
                        return;
                    }

                    if (doc.created) {
                        doc.insert(0, $("#InitialCode").val());
                    }

                    doc.attach_codemirror(GlobalEditor, false);
                });

                sharejs.open('chat'+guid.toUpperCase(), 'text', 'http://226589.s.dedikuoti.lt:8000/channel', function(error, doc) {
                    var displayName = '<%:Model.DisplayName%>';
                    if (error) {
  		                $('#Link').text("Error occurred while establishing live session. Try again later.");
                        console.error(error);
                        return;
                    }

                    if (doc.created) {
                        doc.insert(0, 'Chat created on <%:DateTime.Now.ToUniversalTime().ToString()%>\n<%:"Logged in users will be represented by their nicks, others - by random letters."%>\n\n');    
                    }
                    $("#chatAreaText").val(doc.getText());
                    ScrollToBottom($("#chatAreaText"));

                    doc.on('insert', function(pos, text) {
                        if ($("#chatsign").text() == "+")
                            $("#chatsign").css('color', 'red');
                        $("#chatAreaText").val(doc.getText());
                        ScrollToBottom($("#chatAreaText"));
                    });

                    $("#chatBoxText").keydown(function(event) {
                        if(event.keyCode == 13) {
                            doc.insert(doc.getText().length, '\n'+'<'+displayName+'>: '+$("#chatBoxText").val());
                            $("#chatAreaText").val(doc.getText());
                            ScrollToBottom($("#chatAreaText"));
                            $("#chatBoxText").val("");
                            return false;
                        }
                    });
                });

                <%if(Model.ShowInput) 
                {%>
                sharejs.open('input'+guid.toUpperCase(), 'text', 'http://226589.s.dedikuoti.lt:8000/channel', function(error, doc) {
                    if (error) {
                        $('#Link').text("Error occurred while establishing live session. Try again later.");
                        console.error(error);
                        return;
                    }

                    if (doc.created) {
                        doc.insert(0, $("#InitialInput").val());
                    }
                    if (doc.getText() !== "" && $("#Expand_input_sign").text() == "+")
                    {
                        $("#Input").toggle();
                        $("#Expand_input_sign").text("-");
                        $("#Expand_input_text").text("Hide input");
                    }
                    var elem = document.getElementById('Input');
                    doc.attach_textarea(elem);

                });
            <%}%>

            <%if(Model.ShowCompilerArgs) 
            {%>
                sharejs.open('args' + guid.toUpperCase(), 'text', 'http://226589.s.dedikuoti.lt:8000/channel', function (error, doc) {
                    if (error) {
                        $('#Link').text("Error occurred while establishing live session. Try again later.");
                        console.error(error);
                        return;
                    }

                    if (doc.created) {
                        doc.insert(0, $("#CompilerArgs").val());
                    }
                    var elem = document.getElementById('CompilerArgs');
                    doc.attach_textarea(elem);

                });
        <%}%>

                function ScrollToBottom(textArea) {
                    textArea.scrollTop(
                        textArea[0].scrollHeight - textArea.height()
                    );
                }
                var connectionError = false;
                connection.on("ok", function() {
                       if(connectionError) {
                            $('#Link').text("Connection is restored.");
                            connectionError = false;
                        }
                });
                connection.on("connecting", function() {
                        $('#Link').text("Connection lost, trying to restore...");
                });
                connection.on("disconnected", function() {
                        $('#Link').text("Connection lost, trying to restore...");
                        connectionError = true;
                });
                connection.on("stopped", function() {
                        $('#Link').text("Connection error occurred, please refresh the page.");
                        connectionError = true;
                });

                setTimeout(CheckUserStats, 30000);
                function CheckUserStats() {
                    var serializedData = $("#liveForm").serialize();

                    $.ajax({
                        type: "POST",
                        url: "/live/UserStats",
                        data: serializedData,
                        success:  
                            function (data, textStatus, XMLHttpRequest) 
                            {
                                try
                                {
                                    var obj = jQuery.parseJSON(data);                                        
                                    $("#UsersCount").replaceWith("<span id=\"UsersCount\">"+obj.Users_count+"</span>");
                                    setTimeout(CheckUserStats, 30000);
                                }
                                catch(err)
                                {
                                    setTimeout(CheckUserStats, 30000);
                                }
                            },
                        dataType: 'text',
                        timeout: 45000,
                        error: function (request, status, err) {
                                    setTimeout(CheckUserStats, 30000);
                                }
                    });
                }
            <%} %>
        });
    //]]>
    </script>
<%}
else if(Model.EditorChoice == EditorsEnum.Editarea)
{%>
    <script type="text/javascript">
    //<![CDATA[
        $(document).ready(function () {
            editAreaLoader.init({
                id: "Program"
	            , syntax: "<%:syntax %>"			
	            , start_highlight: true	    
                , allow_toggle: false
                , replace_tab_by_spaces: 4
                , show_line_colors: true
            });
        });
    //]]>
    </script>
<%}%>