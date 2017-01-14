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
      case LanguagesEnum.VCPP:
          mode = "text/x-c++src";
          syntax = "cpp";
          js = "mode/clike/clike.js";
          break;
      case LanguagesEnum.VC:
          mode = "text/x-csrc";
          syntax = "c";
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
          mode = "text/x-mssql";
          syntax = "tsql";
          js = "mode/sql/sql.js";
          break;
      case LanguagesEnum.MySql:
          mode = "text/x-mysql";
          syntax = "mysql";
          js = "mode/sql/sql.js";
          break;
      case LanguagesEnum.Oracle:
          mode = "text/x-plsql";
          syntax = "oracle";
          js = "mode/sql/sql.js";
          break;
      case LanguagesEnum.Postgresql:
          mode = "text/x-pgsql";
          syntax = "postgres";
          js = "mode/sql/sql.js";
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
      case LanguagesEnum.D:
          mode = "text/x-d";
          syntax = "c";
          js = "mode/d/d.js";
          break;
      case LanguagesEnum.R:
          mode = "text/x-rsrc";
          syntax = "r";
          js = "mode/r/r.js";
          break;
      case LanguagesEnum.Tcl:
          mode = "text/x-tcl";
          syntax = "tcl";
          js = "mode/tcl/tcl.js";
          break;
      case LanguagesEnum.ClientSide:
          mode = "text/html";
          syntax = "html";
          js = "mode/htmlmixed/htmlmixed.js";
          additionalJs.Add("mode/xml/xml.js");
          additionalJs.Add("mode/javascript/javascript.js");
          additionalJs.Add("mode/css/css.js");       
          break;
      case LanguagesEnum.Swift:
          mode = "text/x-swift";
          syntax = "swift";
          js = "mode/swift/swift.js";
          break;
      case LanguagesEnum.Bash:
          mode = "text/x-sh";
          syntax = "shell";
          js = "mode/shell/shell.js";
          break;
      case LanguagesEnum.Ada:
         mode = "text/x-ada";
         syntax = "ada";
         js = "mode/ada/ada.js";
         break;
      case LanguagesEnum.Erlang:
        mode = "text/x-erlang";
        syntax = "erlang";
        js = "mode/erlang/erlang.js";
        break;
      case LanguagesEnum.Elixir:
        mode = "text/x-elixir";
        syntax = "elixir";
        js = "mode/elixir/elixir.js";
        break;
      case LanguagesEnum.Ocaml:
        mode = "text/x-ocaml";
        syntax = "ocaml";
        js = "mode/ocaml/ocaml.js";
        break;
  }
%>
<%if(Model.EditorChoice == EditorsEnum.Codemirror) 
{%>
    <%if(js != null)
    {%>
        <script type="text/javascript" src="/Scripts/codemirror3/<%:js%>"></script>
    <%} %>
    <%foreach(var a in additionalJs)
    {%>
        <script type="text/javascript" src="/Scripts/codemirror3/<%:a%>"></script>
    <%}%>
    <script type="text/javascript">
        //<![CDATA[
        $(document).ready(function () {
            var editor = CodeMirror.fromTextArea(document.getElementById("Program"), {
                //matchBrackets : true,
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
                      //Model.LanguageChoice == LanguagesEnum.Javascript ||
                      //Model.LanguageChoice == LanguagesEnum.Nodejs ||
                      Model.LanguageChoice == LanguagesEnum.Scala || 
                      Model.LanguageChoice == LanguagesEnum.Prolog ||
                      Model.LanguageChoice == LanguagesEnum.FSharp)
                {%>
                    smartIndent: false,
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

                    <%if(!Model.IsLive)
                    { %>
                        "F11": function(cm) {
                                cm.setOption("fullScreen", !cm.getOption("fullScreen"));
                            },
                        "Esc": function(cm) {
                                if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
                            }, 
                        "F8": function(cm) {
                                if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
                        }
                    <%}%>
                }
            });
            GlobalEditor = editor;
            
            <%if (Model.IsIntellisense) 
            {%>
                    CodeMirror.commands.autocomplete = function (cm) {
                        CodeMirror.showHint(cm, CodeMirror.hint.csharp, { async: true });
                    };
            <%}%>

            <%if(Model.IsLive && Model.EditorChoice == EditorsEnum.Codemirror)
            { %>
                var guid = '<%:Model.CodeGuid %>';
                var firepadRef = new Firebase('https://rextester.firebaseio.com/firepads/' + guid);
                firepadRef.once('value', function (snapshot) {
                    if (snapshot.val() === null) {
                        editor.setValue('');
                        var firepad = Firepad.fromCodeMirror(firepadRef, editor);
                        firepad.on('ready', function () {
                            firepad.setText($("#InitialCode").val());
                        });
                    }
                    else {
                        editor.setValue('');
                        var firepad = Firepad.fromCodeMirror(firepadRef, editor);
                    }
                });
                
                var firepadChatRef = new Firebase('https://rextester.firebaseio.com/chats/' + guid);
                var displayName = '<%:Model.DisplayName%>';

                firepadChatRef.limit(20).on('child_added', function (snapshot) {
                    var message = snapshot.val();
                    if ($("#chatsign").text() == "+")
                        $("#chatsign").css('color', 'red');
                    $('#chatAreaText').val($("#chatAreaText").val() + message.name + ': ' + message.text);
                    ScrollToBottom($("#chatAreaText"));
                });

                $("#chatBoxText").keydown(function (event) {
                    if (event.keyCode == 13) {
                        firepadChatRef.push({ name: displayName, text: $("#chatBoxText").val()+'\n' });
                        $("#chatBoxText").val("");
                        return false;
                    }
                });

                function ScrollToBottom(textArea) {
                    textArea.scrollTop(
                        textArea[0].scrollHeight - textArea.height()
                    );
                }
                
                var listRef = new Firebase('https://rextester.firebaseio.com/presence/' + guid);
                var userRef = listRef.push();

                var presenceRef = new Firebase("https://rextester.firebaseio.com/.info/connected");
                presenceRef.on("value", function (snap) {
                    if (snap.val()) {
                        userRef.set(true);
                        // Remove ourselves when we disconnect.
                        userRef.onDisconnect().remove();
                    }
                });

                // Number of online users is the number of objects in the presence list.
                listRef.on("value", function (snap) {
                    $("#UsersCount").replaceWith("<span id=\"UsersCount\">" + snap.numChildren() + "</span>");
                });


                $('#delete_link').on('click', function () {
                    var a = confirm('Are you sure?');
                    if (a) {
                        firepadRef.remove();
                        firepadChatRef.remove();
                        window.location = "<%:Utils.BaseUrl+"delete"+"/"+Model.PrimaryGuid%>";
                    }
                });

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