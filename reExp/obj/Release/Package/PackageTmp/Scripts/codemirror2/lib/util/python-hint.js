(function () {

    function scriptHint(editor, callback) {
        var cur = editor.getCursor(), token = editor.getTokenAt(cur);
        if (token.string[0] === "(") {
            $.post('http://api.rextester.com/service/Service.asmx/GetPythonParenCompletions', { source: $('<div/>').text(editor.getValue()).html(), line: cur.line + 1, column: cur.ch },
                function (data) {
                    Do(editor, callback, data, '(')
                }, 'text')
        }
        else {
            $.post('http://api.rextester.com/service/Service.asmx/GetPythonDotCompletions', { source: $('<div/>').text(editor.getValue()).html(), line: cur.line + 1, column: cur.ch },
                function (data) {
                    Do(editor, callback, data, '.')
                }, 'text')
        }         
    }

    function Do(editor, callback, data, symbol)
    {
        var cur = editor.getCursor(), token = editor.getTokenAt(cur);
        var position = editor.indexFromPos(cur);
        var obj = jQuery.parseJSON(data);
        var result = {
            list: obj === null ? [] : obj,
            initial_list: obj,
            from: { line: cur.line, ch: cur.ch },
            to: { line: cur.line, ch: cur.ch }
        };
        callback(editor, function (editor) {
            var cur = editor.getCursor(), token = editor.getTokenAt(cur), pos = editor.indexFromPos(cur);
            if (pos < position) {
                result.list = [];
                return result;
            }

            var index = token.string.indexOf(symbol);
            if (token.string.length > 1 && index !== -1) {
                token.string = token.string.substring(index + 1);
                token.start = token.start + index + 1;
            }

            var matches = [];
            if (token != null && $.trim(token.string) !== "" && token.string !== symbol && result.initial_list != null && result.initial_list.length > 0) {
                for (i = 0; i < result.initial_list.length; i++)
                    if (result.initial_list[i].substring(0, token.string.length).toLowerCase() === token.string.toLowerCase())
                        matches.push(result.initial_list[i]);
                result.list = matches;
            }
            if (token.string === symbol || $.trim(token.string) === "")
                result.list = result.initial_list === null ? [] : result.initial_list;
            result.from = { line: cur.line, ch: token.string === symbol || $.trim(token.string) === "" ? token.start + 1 : token.start };
            result.to = { line: cur.line, ch: token.end };
            return result;
        });
    }

    CodeMirror.LanguageHint = function (editor, callback) {
        scriptHint(editor, callback);
    }

})();
