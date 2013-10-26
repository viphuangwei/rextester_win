CodeMirror.defineMode("nasm", function (config, parserConfig) {
    var indentUnit = config.indentUnit;
    var curPunc;

    function tokenBase(stream, state) {
        var ch = stream.next();

        if (ch == '"' || ch == "'" || ch == "`") {
            state.tokenize = tokenString(ch);
            return state.tokenize(stream, state);
        }

        if (ch == ";") {
            stream.skipToEnd();
            return "comment";
        }

        stream.eatWhile(/[\w\$_]/);

        return "word";
    }

    function tokenString(quote) {
        return function (stream, state) {
            var escaped = false, next, end = false;
            while ((next = stream.next()) != null) {
                if (next == quote && !escaped) { end = true; break; }
                escaped = !escaped && next == "\\";
            }
            if (end || !escaped)
                state.tokenize = tokenBase;
            return "string";
        };
    }

    function Context(indented, column, type, align, prev) {
        this.indented = indented;
        this.column = column;
        this.type = type;
        this.align = align;
        this.prev = prev;
    }

    // Interface

    return {
        startState: function (basecolumn) {
            return {
                tokenize: null,
                context: new Context((basecolumn || 0) - indentUnit, 0, "top", false),
                indented: 0,
                startOfLine: true
            };
        },

        token: function (stream, state) {
            var ctx = state.context;
            if (stream.sol()) {
                if (ctx.align == null) ctx.align = false;
                state.indented = stream.indentation();
                state.startOfLine = true;
            }
            if (stream.eatSpace()) return null;
            curPunc = null;
            var style = (state.tokenize || tokenBase)(stream, state);
            if (style == "comment") return style;
            if (ctx.align == null) ctx.align = true;

            state.startOfLine = false;
            return style;
        },

        indent: function (state, textAfter) {
            var ctx = state.context;
            return ctx.indented;
        },

        electricChars: ""
    };
});

(function () {
    function words(str) {
        var obj = {}, words = str.split(" ");
        for (var i = 0; i < words.length; ++i) obj[words[i]] = true;
        return obj;
    }
    var cKeywords = "";


    CodeMirror.defineMIME("text/x-nasm", {
        name: "nasm",
        keywords: words(cKeywords),
        blockKeywords: words(""),
        atoms: words(""),
        hooks: {  }
    });
    
} ());
