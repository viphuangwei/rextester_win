(function () {

function scriptHint(editor, getToken, callback) {
    if (typeof editor.cache != 'undefined' && editor.cache !== null)
    {
        var cur = editor.getCursor(), token = getToken(editor, cur), tprop = token, position = editor.indexFromPos(cur);
        var result = editor.cache;
        //result.from = { line: cur.line, ch: cur.ch },
        result.to = { line: cur.line, ch: cur.ch }
        var matches = [];
        if (token != null && token.string !== "." && result.initial_list != null && result.initial_list.length > 0) {
            for (i = 0; i < result.initial_list.length; i++)
                if (result.initial_list[i].substring(0, token.string.length).toLowerCase() === token.string.toLowerCase())
                    matches.push(result.initial_list[i]);
            result.list = matches;
        }
        callback(result);
        return;
    }

    // Find the token at the cursor
    var cur = editor.getCursor(), token = getToken(editor, cur), tprop = token, position = editor.indexFromPos(cur);
    $.post('/service/codecompletion', { code: editor.getValue(), position: position, language: 1, line: cur.line, ch: cur.ch },
            function (data) {
                var cur = editor.getCursor()
                var obj = jQuery.parseJSON(data);
                var result = {
                    list: obj === null ? [] : obj,
                    initial_list: obj,
                    from: { line: cur.line, ch: cur.ch },
                    to: { line: cur.line, ch: cur.ch }
                };

                var cur = editor.getCursor(), token = editor.getTokenAt(cur), pos = editor.indexFromPos(cur);
                if (pos < position) {
                    result.list = [];
                    return result;
                }

                var index = token.string.indexOf(".");
                if (token.string.length > 1 && index !== -1) {
                    token.string = token.string.substring(index + 1);
                    token.start = token.start + index + 1;
                }

                var matches = [];
                if (token != null && token.string !== "." && result.initial_list != null && result.initial_list.length > 0) {
                    for (i = 0; i < result.initial_list.length; i++)
                        if (result.initial_list[i].substring(0, token.string.length).toLowerCase() === token.string.toLowerCase())
                            matches.push(result.initial_list[i]);
                    result.list = matches;
                }
                if (token.string === ".")
                    result.list = result.initial_list === null ? [] : result.initial_list;
                result.from = { line: cur.line, ch: token.string === "." ? token.start + 1 : token.start };
                result.to = { line: cur.line, ch: token.end };
                if (result.list == 1) {
                    result.list.push(" ");
                }
                result.list.sort();
                editor.cache = result;
                callback(result);
            }, 'text');
  }

  function csharpHint(editor, callback) {
    return scriptHint(editor, function (e, cur) {return e.getTokenAt(cur);}, callback);
  }
  CodeMirror.csharpHint = csharpHint; // deprecated
  CodeMirror.registerHelper("hint", "csharp", csharpHint);
})();
